namespace Recorder
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Name_box = new System.Windows.Forms.TextBox();
            this.Name_label = new System.Windows.Forms.Label();
            this.lbLength = new System.Windows.Forms.Label();
            this.lbPosition = new System.Windows.Forms.Label();
            this.chart = new Accord.Controls.Wavechart();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnIdentify = new System.Windows.Forms.Button();
            this.btnRecord = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTrain1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.button1 = new System.Windows.Forms.Button();
            this.function_box = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.width_label = new System.Windows.Forms.Label();
            this.width_box = new System.Windows.Forms.TextBox();
            this.testCase_box = new System.Windows.Forms.ComboBox();
            this.matchedWith = new System.Windows.Forms.Label();
            this.Distance_box = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // Name_box
            // 
            this.Name_box.Location = new System.Drawing.Point(231, 188);
            this.Name_box.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name_box.Name = "Name_box";
            this.Name_box.Size = new System.Drawing.Size(143, 22);
            this.Name_box.TabIndex = 22;
            // 
            // Name_label
            // 
            this.Name_label.AutoSize = true;
            this.Name_label.Location = new System.Drawing.Point(391, 191);
            this.Name_label.Name = "Name_label";
            this.Name_label.Size = new System.Drawing.Size(60, 16);
            this.Name_label.TabIndex = 21;
            this.Name_label.Text = "Distance";
            // 
            // lbLength
            // 
            this.lbLength.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbLength.Location = new System.Drawing.Point(285, 32);
            this.lbLength.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLength.Name = "lbLength";
            this.lbLength.Size = new System.Drawing.Size(91, 50);
            this.lbLength.TabIndex = 18;
            this.lbLength.Text = "Length: 00.00 sec.";
            this.lbLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPosition
            // 
            this.lbPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbPosition.Location = new System.Drawing.Point(3, 32);
            this.lbPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbPosition.Name = "lbPosition";
            this.lbPosition.Size = new System.Drawing.Size(91, 50);
            this.lbPosition.TabIndex = 19;
            this.lbPosition.Text = "Position: 00.00 sec.";
            this.lbPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chart
            // 
            this.chart.BackColor = System.Drawing.Color.Black;
            this.chart.ForeColor = System.Drawing.Color.DarkGreen;
            this.chart.Location = new System.Drawing.Point(99, 32);
            this.chart.Margin = new System.Windows.Forms.Padding(4);
            this.chart.Name = "chart";
            this.chart.RangeX = ((AForge.DoubleRange)(resources.GetObject("chart.RangeX")));
            this.chart.RangeY = ((AForge.DoubleRange)(resources.GetObject("chart.RangeY")));
            this.chart.SimpleMode = false;
            this.chart.Size = new System.Drawing.Size(179, 50);
            this.chart.TabIndex = 17;
            this.chart.Text = "chart1";
            // 
            // btnPlay
            // 
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPlay.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPlay.Location = new System.Drawing.Point(231, 132);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(4);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(69, 38);
            this.btnPlay.TabIndex = 12;
            this.btnPlay.Text = "4";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click_1);
            // 
            // btnIdentify
            // 
            this.btnIdentify.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnIdentify.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnIdentify.Location = new System.Drawing.Point(13, 132);
            this.btnIdentify.Margin = new System.Windows.Forms.Padding(4);
            this.btnIdentify.Name = "btnIdentify";
            this.btnIdentify.Size = new System.Drawing.Size(133, 38);
            this.btnIdentify.TabIndex = 13;
            this.btnIdentify.Text = "s";
            this.btnIdentify.UseVisualStyleBackColor = true;
            this.btnIdentify.Click += new System.EventHandler(this.btnIdentify_Click);
            // 
            // btnRecord
            // 
            this.btnRecord.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRecord.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnRecord.Location = new System.Drawing.Point(307, 132);
            this.btnRecord.Margin = new System.Windows.Forms.Padding(4);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(69, 38);
            this.btnRecord.TabIndex = 15;
            this.btnRecord.Text = "=";
            this.btnRecord.UseVisualStyleBackColor = true;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click_1);
            // 
            // btnStop
            // 
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStop.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnStop.Location = new System.Drawing.Point(155, 132);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(69, 38);
            this.btnStop.TabIndex = 16;
            this.btnStop.Text = "<";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click_1);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(775, 28);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(221, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadTrain1ToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.optionsToolStripMenuItem.Text = "Edit";
            // 
            // loadTrain1ToolStripMenuItem
            // 
            this.loadTrain1ToolStripMenuItem.Name = "loadTrain1ToolStripMenuItem";
            this.loadTrain1ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.loadTrain1ToolStripMenuItem.Text = "Load Test1";
            this.loadTrain1ToolStripMenuItem.Click += new System.EventHandler(this.loadTrain1ToolStripMenuItem_Click_1);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(3, 90);
            this.trackBar1.Margin = new System.Windows.Forms.Padding(4);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(373, 56);
            this.trackBar1.TabIndex = 20;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 191);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 23);
            this.button1.TabIndex = 23;
            this.button1.Text = "back";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // function_box
            // 
            this.function_box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.function_box.FormattingEnabled = true;
            this.function_box.Items.AddRange(new object[] {
            "DTW",
            "DTW(Time Sync)",
            "Pruning(Search Path)",
            "Pruning(Path cost)",
            "Beam(Time sync)"});
            this.function_box.Location = new System.Drawing.Point(402, 143);
            this.function_box.Name = "function_box";
            this.function_box.Size = new System.Drawing.Size(139, 24);
            this.function_box.TabIndex = 24;
            this.function_box.SelectedIndexChanged += new System.EventHandler(this.function_box_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(440, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 16);
            this.label1.TabIndex = 25;
            this.label1.Text = "Function";
            // 
            // width_label
            // 
            this.width_label.AutoSize = true;
            this.width_label.Location = new System.Drawing.Point(621, 191);
            this.width_label.Name = "width_label";
            this.width_label.Size = new System.Drawing.Size(41, 16);
            this.width_label.TabIndex = 26;
            this.width_label.Text = "Width";
            // 
            // width_box
            // 
            this.width_box.Location = new System.Drawing.Point(665, 188);
            this.width_box.Name = "width_box";
            this.width_box.Size = new System.Drawing.Size(98, 22);
            this.width_box.TabIndex = 27;
            // 
            // testCase_box
            // 
            this.testCase_box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.testCase_box.FormattingEnabled = true;
            this.testCase_box.Items.AddRange(new object[] {
            "Sample(Case 2)"});
            this.testCase_box.Location = new System.Drawing.Point(402, 41);
            this.testCase_box.Name = "testCase_box";
            this.testCase_box.Size = new System.Drawing.Size(121, 24);
            this.testCase_box.TabIndex = 28;
           
            // 
            // matchedWith
            // 
            this.matchedWith.AutoSize = true;
            this.matchedWith.Location = new System.Drawing.Point(141, 191);
            this.matchedWith.Name = "matchedWith";
            this.matchedWith.Size = new System.Drawing.Size(84, 16);
            this.matchedWith.TabIndex = 29;
            this.matchedWith.Text = "Matched with";
            // 
            // Distance_box
            // 
            this.Distance_box.Location = new System.Drawing.Point(457, 188);
            this.Distance_box.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Distance_box.Name = "Distance_box";
            this.Distance_box.Size = new System.Drawing.Size(143, 22);
            this.Distance_box.TabIndex = 30;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 222);
            this.Controls.Add(this.Distance_box);
            this.Controls.Add(this.matchedWith);
            this.Controls.Add(this.testCase_box);
            this.Controls.Add(this.width_box);
            this.Controls.Add(this.width_label);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.function_box);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Name_box);
            this.Controls.Add(this.Name_label);
            this.Controls.Add(this.lbLength);
            this.Controls.Add(this.lbPosition);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnIdentify);
            this.Controls.Add(this.btnRecord);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.trackBar1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Identification";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Name_box;
        private System.Windows.Forms.Label Name_label;
        private System.Windows.Forms.Label lbLength;
        private System.Windows.Forms.Label lbPosition;
        private Accord.Controls.Wavechart chart;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnIdentify;
        private System.Windows.Forms.Button btnRecord;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTrain1ToolStripMenuItem;
        private System.Windows.Forms.ComboBox function_box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label width_label;
        private System.Windows.Forms.TextBox width_box;
        private System.Windows.Forms.ComboBox testCase_box;
        private System.Windows.Forms.Label matchedWith;
        private System.Windows.Forms.TextBox Distance_box;
    }
}