using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Accord.Audio;
using Accord.Audio.Formats;
using Accord.DirectSound;
using Accord.Audio.Filters;
using Recorder.Recorder;
using Recorder.MFCC;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using AForge.Math.Metrics;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace Recorder
{
    public partial class Form1 : Form
    {
        private AudioSignal signal = null;
        Sequence seq = null;

        private string path;

        private Encoder encoder;
        private Decoder decoder;
        private string connectionString;
        private bool isRecorded;
        private int frameCount = 0;
        private bool isMatching = false;
        private Queue<double> audioBuffer = new Queue<double>();
        private const int MaxBufferSize = 16000;
        public Form1()
        {
            InitializeComponent();
            Name_box.ReadOnly = true;
            Distance_box.ReadOnly = true;
            chart.SimpleMode = true;
            chart.AddWaveform("wave", Color.Green, 1, false);
            width_label.Visible = false;
            width_box.Visible = false;
            function_box.SelectedIndex = 0;
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Directory.GetParent(baseDirectory).Parent.Parent.FullName;
            string trimmedRoot = projectRoot;
            if (Path.GetFileName(projectRoot).Equals("bin", StringComparison.OrdinalIgnoreCase))
            {
                trimmedRoot = Directory.GetParent(projectRoot).FullName;
            }
            //Release Path
            string dbPath = Path.Combine(trimmedRoot, "GUI", "voice_enrollment_data.mdf");
            //Debug Path
            //string dbPath = Path.Combine(projectRoot,"GUI","voice_enrollment_data.mdf");

            connectionString = $@"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30;";
            updateButtons();
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            isRecorded = true;
            this.encoder = new Encoder(source_NewFrame, source_AudioSourceError);
            this.encoder.Start();
            updateButtons();
        }

        /// <summary>
        ///   Plays the recorded audio stream.
        /// </summary>
        /// 
        private void btnPlay_Click(object sender, EventArgs e)
        {
            InitializeDecoder();
            // Configure the track bar so the cursor
            // can show the proper current position
            if (trackBar1.Value < this.decoder.frames)
                this.decoder.Seek(trackBar1.Value);
            trackBar1.Maximum = this.decoder.samples;
            this.decoder.Start();
            
            updateButtons();
        }

        private void InitializeDecoder()
        {
            if (isRecorded)
            {
                // First, we rewind the stream
                this.encoder.stream.Seek(0, SeekOrigin.Begin);
                this.decoder = new Decoder(this.encoder.stream, this.Handle, output_AudioOutputError, output_FramePlayingStarted, output_NewFrameRequested, output_PlayingFinished);
            }
            else
            {
                this.decoder = new Decoder(this.path, this.Handle, output_AudioOutputError, output_FramePlayingStarted, output_NewFrameRequested, output_PlayingFinished);
            }
        }

        /// <summary>
        ///   Stops recording or playing a stream.
        /// </summary>
        /// 
       
        /// <summary>
        ///   This callback will be called when there is some error with the audio 
        ///   source. It can be used to route exceptions so they don't compromise 
        ///   the audio processing pipeline.
        /// </summary>
        /// 
        private void source_AudioSourceError(object sender, AudioSourceErrorEventArgs e)
        {
            throw new Exception(e.Description);
        }

        /// <summary>
        ///   This method will be called whenever there is a new input audio frame 
        ///   to be processed. This would be the case for samples arriving at the 
        ///   computer's microphone
        /// </summary>
        /// 
        private void source_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            Signal newSignal = eventArgs.Signal;
            this.encoder.addNewFrame(newSignal);
            updateWaveform(this.encoder.current, newSignal.Length);

            double[] samples = newSignal.ToDouble();
            lock (audioBuffer)
            {
                foreach (var sample in samples)
                {
                    audioBuffer.Enqueue(sample);
                    if (audioBuffer.Count > MaxBufferSize)
                        audioBuffer.Dequeue();
                }
            }
            frameCount++;
            if (frameCount % 10 != 0 || isMatching)
                return;

            isMatching = true;

            Task.Factory.StartNew(() =>
            {
                double[] signalData;
                lock (audioBuffer)
                {
                    signalData = audioBuffer.ToArray();
                }
                if (signalData == null || signalData.Length < 4000)
                {
                    isMatching = false;
                    return;
                }
                var signal = new AudioSignal
                {
                    data = signalData,
                    sampleRate = 16000,
                    signalLengthInMilliSec = signalData.Length / 16.0
                };
                signal = AudioOperations.RemoveSilence(signal);
                if (signal.data == null || signal.data.Length < 1000)
                {
                    isMatching = false;
                    return;
                }
                Sequence sequence = AudioOperations.ExtractFeatures(signal);
                if (sequence == null || sequence.Frames == null || sequence.Frames.Length == 0)
                {
                    isMatching = false;
                    return;
                }

                MFCCFrame[] mfccFrames = sequence.Frames;

                PerformRealTimeMatching(mfccFrames);
                isMatching = false;
            });
        }
        private void PerformRealTimeMatching(MFCCFrame[] inputFrames)
        {
            if (inputFrames.Length < 5) return;

            try
            {
                var templates = new Dictionary<string, List<MFCCFrame[]>>();

                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT user_name, template_sequence FROM voice_templates";
                    using (var cmd = new SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string user = reader.GetString(0);
                            string templateString = reader.GetString(1);
                            MFCCFrame[] parsedTemplate = ParseTemplate(templateString);

                            if (!templates.ContainsKey(user))
                                templates[user] = new List<MFCCFrame[]>();

                            templates[user].Add(parsedTemplate);
                        }
                    }
                }
                var matchResult = DTW.MatchingVoicesTimeSync(inputFrames, templates);
                if (matchResult!=null)
                {
                    string matchedUser = matchResult.Item1;
                    double score = matchResult.Item2;
                    Action updateUI = () =>
                    {
                        Name_box.Text = matchedUser;
                        Distance_box.Text = score.ToString();
                    };

                    Invoke(updateUI);
                }
                else
                    Invoke(new Action(() => Name_box.Text = "No match found"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
            }

        }


        /// <summary>
        ///   This event will be triggered as soon as the audio starts playing in the 
        ///   computer speakers. It can be used to update the UI and to notify that soon
        ///   we will be requesting additional frames.
        /// </summary>
        /// 
        private void output_FramePlayingStarted(object sender, PlayFrameEventArgs e)
        {
            updateTrackbar(e.FrameIndex);

            if (e.FrameIndex + e.Count < this.decoder.frames)
            {
                int previous = this.decoder.Position;
                decoder.Seek(e.FrameIndex);

                Signal s = this.decoder.Decode(e.Count);
                decoder.Seek(previous);

                updateWaveform(s.ToFloat(), s.Length);
            }
        }

        /// <summary>
        ///   This event will be triggered when the output device finishes
        ///   playing the audio stream. Again we can use it to update the UI.
        /// </summary>
        /// 
        private void output_PlayingFinished(object sender, EventArgs e)
        {
            updateButtons();
            updateWaveform(new float[BaseRecorder.FRAME_SIZE], BaseRecorder.FRAME_SIZE);
        }

        /// <summary>
        ///   This event is triggered when the sound card needs more samples to be
        ///   played. When this happens, we have to feed it additional frames so it
        ///   can continue playing.
        /// </summary>
        /// 
        private void output_NewFrameRequested(object sender, NewFrameRequestedEventArgs e)
        {
            this.decoder.FillNewFrame(e);
        }


        void output_AudioOutputError(object sender, AudioOutputErrorEventArgs e)
        {
            throw new Exception(e.Description);
        }

        /// <summary>
        ///   Updates the audio display in the wave chart
        /// </summary>
        /// 
        private void updateWaveform(float[] samples, int length)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    chart.UpdateWaveform("wave", samples, length);
                }));
            }
            else
            {
                if (this.encoder != null) { chart.UpdateWaveform("wave", this.encoder.current, length); }
            }
        }

        /// <summary>
        ///   Updates the current position at the trackbar.
        /// </summary>
        /// 
        private void updateTrackbar(int value)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    trackBar1.Value = Math.Max(trackBar1.Minimum, Math.Min(trackBar1.Maximum, value));
                }));
            }
            else
            {
                trackBar1.Value = Math.Max(trackBar1.Minimum, Math.Min(trackBar1.Maximum, value));
            }
        }

        private void updateButtons()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(updateButtons));
                return;
            }

            if (this.encoder != null && this.encoder.IsRunning())
            {
                
                btnIdentify.Enabled = false;
                btnPlay.Enabled = false;
                btnStop.Enabled = true;
                btnRecord.Enabled = false;
                trackBar1.Enabled = false;
            }
            else if (this.decoder != null && this.decoder.IsRunning())
            {
                
                btnIdentify.Enabled = false;
                btnPlay.Enabled = false;
                btnStop.Enabled = true;
                btnRecord.Enabled = false;
                trackBar1.Enabled = true;
            }
            else
            {
                
                btnIdentify.Enabled = seq != null;
                btnPlay.Enabled = this.path != null || this.encoder != null;//stream != null;
                btnStop.Enabled = false;
                btnRecord.Enabled = true;
                trackBar1.Enabled = this.decoder != null;
                trackBar1.Value = 0;
            }
        }

        private void MainFormFormClosed(object sender, FormClosedEventArgs e)
        {
            Stop();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (this.encoder != null) { lbLength.Text = String.Format("Length: {0:00.00} sec.", this.encoder.duration / 1000.0); }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                isRecorded = false;
                path = open.FileName;


                //Open the selected audio file

                signal = AudioOperations.OpenAudioFile(path);
                if (testCase_box.Text != "Sample(Case 2)")
                {
                    signal = AudioOperations.RemoveSilence(signal);
                }
                seq = AudioOperations.ExtractFeatures(signal);
                for (int i = 0; i < seq.Frames.Length; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {

                        if (double.IsNaN(seq.Frames[i].Features[j]) || double.IsInfinity(seq.Frames[i].Features[j]))
                            throw new Exception("NaN");
                    }
                }
                updateButtons();                
            }
        }

        private void Stop()
        {
            if (this.encoder != null) { this.encoder.Stop(); }
            if (this.decoder != null) { this.decoder.Stop(); }
        } 
        private void button1_Click(object sender, EventArgs e)
        {
            GUI mainForm = new GUI();
            mainForm.Show();
            this.Hide();
        }
        private MFCCFrame[] ParseTemplate(string templateString)
        {
            var frames = new List<MFCCFrame>();

            // Split the whole string into frame strings using ';'
            string[] frameStrings = templateString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string frame in frameStrings)
            {
                // Split each frame string into 13 MFCC values using ','
                string[] coef = frame.Split(',');

                var mfcc = new MFCCFrame();
                mfcc.Features = coef.Select(double.Parse).ToArray();  // requires using System.Linq;
                frames.Add(mfcc);
            }

            return frames.ToArray();
        }
        private void btnIdentify_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                if (seq == null)
                {
                    MessageBox.Show("Please record or load an audio first.");
                    return;
                }
                if ((function_box.Text == "Pruning(Search Path)" || function_box.Text == "Pruning(Path cost)") && string.IsNullOrWhiteSpace(width_box.Text))
                {
                    MessageBox.Show("Please add the width first.");
                    return;
                }
                var inputFrames = seq.Frames;

                //usernames of sample traning sets

                var templates = new Dictionary<string, List<MFCCFrame[]>>(); // EACH USER CAN HAVE MULTIPLE OF VOICES IN TRAINING DATA

                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT user_name, template_sequence FROM voice_templates";                  
                    using (var cmd = new SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string user = reader.GetString(0);
                            string templateString = reader.GetString(1);

                            // check if the key is already exist before in map or not (this user has voice before)
                            if (!templates.ContainsKey(user))
                            {
                                templates[user] = new List<MFCCFrame[]>();
                            }

                            templates[user].Add(ParseTemplate(templateString));

                        }
                    }
                }

                Tuple<string, double> bestMatch;  // tuple object that will be returned by matching function
                
                if (function_box.Text == "DTW")
                {
                    bestMatch = DTW.MatchingWithTemplatesDTW(inputFrames, templates);
                }
                else if (function_box.Text == "DTW(Time Sync)")
                {
                    // soon
                    bestMatch = DTW.MatchingVoicesTimeSync(inputFrames, templates);
                   // bestMatch = DTW.MatchingWithTemplatesDTW(inputFrames, templates);
                    //best_match = minDistance;
                }
                else if (function_box.Text == "Pruning(Path cost)")
                {
                    bestMatch = Prunning.PruningMatchingPathCost(inputFrames, templates, Convert.ToInt32(width_box.Text));
                }
                else if (function_box.Text == "Pruning(Search Path)")
                {
                    bestMatch = Prunning.PruningMatchingSearchPath(inputFrames, templates, Convert.ToInt32(width_box.Text));
                }
                else
                {
                    // soon
                    //Beam(Time sync) Code Here 
                    //bestMatch = DTW.MatchingVoicesTimeSync(inputFrames, templates);
                    bestMatch = DTW.MatchingWithTemplatesDTW(inputFrames, templates);
                }        

                Name_box.Text = bestMatch.Item1 ?? "No match found";
                Distance_box.Text = bestMatch.Item2.ToString() ?? "No match found";
                
            }
            catch (Exception ex)
            {
                Name_box.Text = "Error";
                Distance_box.Text = "Error";
                MessageBox.Show("Identification failed: " + ex.Message);

            }
            stopwatch.Stop();
            MessageBox.Show("Normal DTW--- Elapsed Time in sec: " + stopwatch.Elapsed.TotalSeconds + " s");
        }
        private void btnRecord_Click_1(object sender, EventArgs e)
        {
            isRecorded = true;
            this.encoder = new Encoder(source_NewFrame, source_AudioSourceError);
            this.encoder.Start();
            updateButtons();
        }

        private void btnPlay_Click_1(object sender, EventArgs e)
        {
            InitializeDecoder();
            // Configure the track bar so the cursor
            // can show the proper current position
            if (trackBar1.Value < this.decoder.frames)
                this.decoder.Seek(trackBar1.Value);
            trackBar1.Maximum = this.decoder.samples;
            this.decoder.Start();
            updateButtons();
        }


        private void btnStop_Click_1(object sender, EventArgs e)
        {
            Stop();
            if (encoder?.stream != null)
            {
                encoder.stream.Seek(0, SeekOrigin.Begin);
                path = Path.GetTempFileName() + ".wav";

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    encoder.Save(fs);
                }

                signal = AudioOperations.OpenAudioFile(path);
                signal = AudioOperations.RemoveSilence(signal);
                seq = AudioOperations.ExtractFeatures(signal);
            }
            updateButtons();
            updateWaveform(new float[BaseRecorder.FRAME_SIZE], BaseRecorder.FRAME_SIZE);

        }

        private void loadTrain1ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();

            Stopwatch stopwatch_total = new Stopwatch();
            stopwatch_total.Start();

            var hobba = TestcaseLoader.LoadTestcase1Testing(fileDialog.FileName);
            var templates = new Dictionary<string, List<MFCCFrame[]>>();

            // Validate width if needed
            if ((function_box.Text == "Pruning(Search Path)" || function_box.Text == "Pruning(Path cost)") &&
                string.IsNullOrWhiteSpace(width_box.Text))
            {
                MessageBox.Show("Please add the width first.");
                return;
            }

            // Matching function selection
            Func<MFCCFrame[], Dictionary<string, List<MFCCFrame[]>>, Tuple<string, double>> matcher;
            switch (function_box.Text)
            {
                case "DTW":
                    matcher = DTW.MatchingWithTemplatesDTW;
                    break;
                case "DTW(Time Sync)":
                    matcher = DTW.MatchingVoicesTimeSync;
                    break;
                case "Pruning(Path cost)":
                    int widthCost = Convert.ToInt32(width_box.Text);
                    matcher = (frames, temps) => Prunning.PruningMatchingPathCost(frames, temps, widthCost);
                    break;
                case "Pruning(Search Path)":
                    int widthSearch = Convert.ToInt32(width_box.Text);
                    matcher = (frames, temps) => Prunning.PruningMatchingSearchPath(frames, temps, widthSearch);
                    break;
                default:
                    matcher = DTW.MatchingWithTemplatesDTW;
                    break;
            }

            Stopwatch sw_loading_training = new Stopwatch();
            sw_loading_training.Start();

            // Load and parse training templates
            var rawTemplates = new Dictionary<string, List<string>>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT user_name, template_sequence FROM voice_templates";
                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string user = reader.GetString(0);
                        string templateString = reader.GetString(1);

                        if (!rawTemplates.ContainsKey(user))
                            rawTemplates[user] = new List<string>();

                        rawTemplates[user].Add(templateString);
                    }
                }
            }

            // Parse templates in parallel
            Parallel.ForEach(rawTemplates, entry =>
            {
                var parsedList = entry.Value.Select(ParseTemplate).ToList();
                lock (templates)
                {
                    templates[entry.Key] = parsedList;
                }
            });

            sw_loading_training.Stop();
            Console.WriteLine("Elapsed Time in sec for loading training data: " + sw_loading_training.Elapsed.TotalSeconds + " s");

            // Matching
            Stopwatch sw_matching = new Stopwatch();
            sw_matching.Start();

            // Create flattened list of test cases with global index
            var allTests = new List<Tuple<int, MFCCFrame[]>>();
            int currentIndex = 0;
            for (int i = 0; i < hobba.Count; i++)
            {
                for (int j = 0; j < hobba[i].UserTemplates.Count; j++)
                {
                    var seq = AudioOperations.ExtractFeatures(hobba[i].UserTemplates[j]);
                    var temp = new Tuple<int, MFCCFrame[]>(currentIndex++, seq.Frames);
                    allTests.Add(temp);
                }
            }

            // Store results in parallel, preserving order
           
            string[] temp_bestMatches = new string[allTests.Count];
            Parallel.ForEach(allTests, test =>
            {
                var result = matcher(test.Item2, templates);
                temp_bestMatches[test.Item1] = result.Item1;
            });
            List<string> bestMatches = temp_bestMatches.ToList();
            sw_matching.Stop();
            Console.WriteLine("Elapsed Time in sec for matching testing data: " + sw_matching.Elapsed.TotalSeconds + " s");

            stopwatch_total.Stop();
            MessageBox.Show("Elapsed Time Total: " + stopwatch_total.Elapsed.TotalSeconds + " s");

            // Now you have ordered list: bestMatches[index]



            double error = TestcaseLoader.CheckTestcaseAccuracy(hobba,bestMatches);
            double testAcc = (1 - error) * 100;
            MessageBox.Show("Accuracy = " + testAcc);
            
            
        }

        private void function_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            width_label.Visible = false;
            width_box.Visible = false;
            if(function_box.Text == "Pruning(Search Path)" || function_box.Text == "Pruning(Path cost)")
            {
                width_label.Visible = true;
                width_box.Visible = true;
            }
        }

       
    }
}
