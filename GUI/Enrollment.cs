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
using System.Data.SqlClient;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NAudio.Wave;
using System.Collections.Concurrent;

namespace Recorder
{
    /// <summary>
    ///   Speaker Identification application.
    /// </summary>
    /// 
    public partial class Enrollment : Form
    {
        /// <summary>
        /// Data of the opened audio file, contains:
        ///     1. signal data
        ///     2. sample rate
        ///     3. signal length in ms
        /// </summary>
        private AudioSignal signal = null;
        Sequence seq = null;
       
        private string path;

        private Encoder encoder;
        private Decoder decoder;

        private bool isRecorded;
        //private bool isSaved;
        private string connectionString;
        private string username_text;
        private int id;
        //private int currId;
        public Enrollment(string username, int userId)
        {
            InitializeComponent();

            //Your project solution path////////////////
            username_text=username;
            id=userId;
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Directory.GetParent(baseDirectory).Parent.Parent.FullName;
            string trimmedRoot = projectRoot;
            if (Path.GetFileName(projectRoot).Equals("bin", StringComparison.OrdinalIgnoreCase))
            {
                trimmedRoot = Directory.GetParent(projectRoot).FullName;
            }
            //Release Path
            string dbPath = Path.Combine(trimmedRoot, "GUI", "voice_enrollment_data.mdf");
            Console.WriteLine(dbPath);
            //Debug Path
            //string dbPath = Path.Combine(projectRoot,"GUI","voice_enrollment_data.mdf");
            connectionString = $@"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            // Configure the wavechart
            chart.SimpleMode = true;
            chart.AddWaveform("wave", Color.Green, 1, false);
            updateButtons();
        }


        /// <summary>
        ///   Starts recording audio from the sound card
        /// </summary>
        /// 
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
        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();   
            updateButtons();
            updateWaveform(new float[BaseRecorder.FRAME_SIZE], BaseRecorder.FRAME_SIZE);
        }

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
            this.encoder.addNewFrame(eventArgs.Signal);
            updateWaveform(this.encoder.current, eventArgs.Signal.Length);
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
                btnAdd.Enabled = false;
                
                btnPlay.Enabled = false;
                btnStop.Enabled = true;
                btnRecord.Enabled = false;
                trackBar1.Enabled = false;
            }
            else if (this.decoder != null && this.decoder.IsRunning())
            {
                btnAdd.Enabled = false;
                
                btnPlay.Enabled = false;
                btnStop.Enabled = true;
                btnRecord.Enabled = false;
                trackBar1.Enabled = true;
            }
            else
            {
                btnAdd.Enabled = this.path != null || this.encoder != null;
               
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

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            path = saveFileDialog1.FileName;
            if (this.encoder != null)
            {
                Stream fileStream = saveFileDialog1.OpenFile();
                this.encoder.Save(fileStream);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog(this);
            //isSaved = true;
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

                Console.WriteLine("Opening File");
                signal = AudioOperations.OpenAudioFile(path);
                Console.WriteLine("Done!");
                try
                {
                    Console.WriteLine("Removing Silence");
                    if(testCase_box.Text != "Sample(Case 2)")
                    {
                        signal = AudioOperations.RemoveSilence(signal);
                    }
                    Console.WriteLine("Done!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during RemoveSilence: " + ex.Message);
                    MessageBox.Show("Error: " + ex.Message);
                    return;
                }

                Console.WriteLine("Extracting Features");
                seq = AudioOperations.ExtractFeatures(signal);
                Console.WriteLine("Done!");
                for (int i = 0; i < seq.Frames.Length; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {

                        if (double.IsNaN(seq.Frames[i].Features[j]) || double.IsInfinity(seq.Frames[i].Features[j]))
                            throw new Exception("NaN");
                    }
                }
                updateButtons();
                //isSaved = true;
            }
        }

        private void Stop()
        {
            if (this.encoder != null) { this.encoder.Stop(); }
            if (this.decoder != null) { this.decoder.Stop(); }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {           
            if ((this.encoder != null || this.decoder != null) && !string.IsNullOrWhiteSpace(username_text))// && isSaved)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                //Dynamic Path                             
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Serialize features
                    if (isRecorded)
                    {
                        try
                        {
                            // Convert stream to AudioSignal
                            this.encoder.stream.Seek(0, SeekOrigin.Begin);

                            WaveDecoder waveDecoder = new WaveDecoder(this.encoder.stream);
                            signal = new AudioSignal();
                            signal.sampleRate = waveDecoder.SampleRate;
                            signal.signalLengthInMilliSec = (int)((double)waveDecoder.Frames / waveDecoder.SampleRate * 1000.0);

                            Signal tempSignal = waveDecoder.Decode(waveDecoder.Frames);
                            signal.data = new double[waveDecoder.Frames];
                            tempSignal.CopyTo(signal.data);

                            // Remove silence
                            if (testCase_box.Text != "Sample(Case 2)")
                            {
                                signal = AudioOperations.RemoveSilence(signal);
                            }
                            

                            // Extract features
                            seq = AudioOperations.ExtractFeatures(signal);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    double[][] features = new double[seq.Frames.Length][];
                    for (int i = 0; i < seq.Frames.Length; i++)
                        features[i] = seq.Frames[i].Features;

                    List<string> frameStrings = new List<string>();

                    for (int x = 0; x < features.Length; x++)
                    {
                        frameStrings.Add(string.Join(",", features[x]));
                        // Console.WriteLine(templateString);
                    }
                    string templateString = string.Join(";", frameStrings) + ";";


                    string insertSql = "INSERT INTO voice_templates  (user_id, user_name, template_sequence) VALUES (@id, @name, @template)";
                    using (var insertCmd = new SqlCommand(insertSql, conn))
                    {                       
                        insertCmd.Parameters.AddWithValue("@id", id);
                        insertCmd.Parameters.AddWithValue("@name", username_text);
                        insertCmd.Parameters.AddWithValue("@template", templateString);
                        insertCmd.ExecuteNonQuery();
                        Console.WriteLine("Data Inserted Succesfully!");
                    }
                    MessageBox.Show("Data Inserted Succesfully!");
                    btnAdd.Enabled = false;     
                    btnPlay.Enabled = false;
                    conn.Close();
                }
                stopwatch.Stop();
                MessageBox.Show("Normal DTW--- Elapsed Time in sec: " + stopwatch.Elapsed.TotalSeconds + " s");
            }
           /* else if (!isSaved)
            {
                MessageBox.Show("Please Save the Record First!");
            }*/
            else
            {
                MessageBox.Show("Null occured");
            }
        }
        private void loadTrain1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var hobba = TestcaseLoader.LoadTestcase1Training(fileDialog.FileName);



            var userIdMap = new ConcurrentDictionary<string, int>();
            var templateTable = new DataTable();

            templateTable.Columns.Add("user_id", typeof(int));
            templateTable.Columns.Add("user_name", typeof(string));
            templateTable.Columns.Add("template_sequence", typeof(string));

            // STEP 1: Fetch or Insert Users and Get their IDs
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (var user in hobba)
                {
                    int userId = 0;

                    using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM voice_enrollment_final WHERE user_name = @Username", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", user.UserName);
                        int existingCount = (int)checkCmd.ExecuteScalar();

                        if (existingCount <= 0)
                        {
                            using (var insertCmd = new SqlCommand("INSERT INTO voice_enrollment_final (user_name) VALUES (@Username); SELECT SCOPE_IDENTITY();", conn))
                            {
                                insertCmd.Parameters.AddWithValue("@Username", user.UserName);
                                object resultId = insertCmd.ExecuteScalar();
                                userId = Convert.ToInt32(resultId);
                            }
                        }
                        else
                        {
                            using (var getIdCmd = new SqlCommand("SELECT TOP 1 user_id FROM voice_templates WHERE user_name = @Username ORDER BY user_id DESC", conn))
                            {
                                getIdCmd.Parameters.AddWithValue("@Username", user.UserName);
                                object resultId = getIdCmd.ExecuteScalar();
                                userId = resultId != null ? Convert.ToInt32(resultId) : 0;
                            }
                        }
                    }

                    userIdMap[user.UserName] = userId;
                }

                conn.Close();
            }

            // STEP 2: Process Templates and Fill DataTable
            Parallel.ForEach(hobba, new ParallelOptions { MaxDegreeOfParallelism = 4 }, user =>
            {
                if (!userIdMap.TryGetValue(user.UserName, out int currId))
                    return;

                foreach (var template in user.UserTemplates)
                {
                    var seq = AudioOperations.ExtractFeatures(template);
                    double[][] features = new double[seq.Frames.Length][];

                    for (int j = 0; j < seq.Frames.Length; j++)
                    {
                        features[j] = seq.Frames[j].Features;
                    }

                    List<string> frameStrings = new List<string>(features.Length);
                    for (int x = 0; x < features.Length; x++)
                    {
                        frameStrings.Add(string.Join(",", features[x]));
                    }

                    string templateString = string.Join(";", frameStrings) + ";";

                    lock (templateTable) // DataTable is not thread-safe
                    {
                        templateTable.Rows.Add(currId, user.UserName, templateString);
                    }
                }
            });

            // STEP 3: Bulk Insert into voice_templates
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = "voice_templates";

                    bulkCopy.ColumnMappings.Add("user_id", "user_id");
                    bulkCopy.ColumnMappings.Add("user_name", "user_name");
                    bulkCopy.ColumnMappings.Add("template_sequence", "template_sequence");

                    bulkCopy.WriteToServer(templateTable);
                }
                conn.Close();
            }
            stopwatch.Stop();
            MessageBox.Show("Completely Done Loading Training Data!");
            MessageBox.Show("Elapsed Time in sec for loading training data: " + stopwatch.Elapsed.TotalSeconds + " s");
            
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Enroll_data mainForm = new Enroll_data();
            mainForm.Show();
            this.Hide();
        }

        
    }
}
