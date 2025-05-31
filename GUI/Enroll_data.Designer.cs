namespace Recorder
{
    partial class Enroll_data
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
            this.user_label = new System.Windows.Forms.Label();
            this.user_box = new System.Windows.Forms.TextBox();
            this.ID_label = new System.Windows.Forms.Label();
            this.ID_box = new System.Windows.Forms.ComboBox();
            this.Save_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.user_button = new System.Windows.Forms.Button();
            this.back_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // user_label
            // 
            this.user_label.AutoSize = true;
            this.user_label.Location = new System.Drawing.Point(13, 49);
            this.user_label.Name = "user_label";
            this.user_label.Size = new System.Drawing.Size(70, 16);
            this.user_label.TabIndex = 0;
            this.user_label.Text = "Username";
            // 
            // user_box
            // 
            this.user_box.Location = new System.Drawing.Point(89, 46);
            this.user_box.Name = "user_box";
            this.user_box.Size = new System.Drawing.Size(121, 22);
            this.user_box.TabIndex = 1;
            // 
            // ID_label
            // 
            this.ID_label.AutoSize = true;
            this.ID_label.Location = new System.Drawing.Point(389, 49);
            this.ID_label.Name = "ID_label";
            this.ID_label.Size = new System.Drawing.Size(51, 16);
            this.ID_label.TabIndex = 2;
            this.ID_label.Text = "Your ID";
            // 
            // ID_box
            // 
            this.ID_box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ID_box.FormattingEnabled = true;
            this.ID_box.Location = new System.Drawing.Point(446, 44);
            this.ID_box.Name = "ID_box";
            this.ID_box.Size = new System.Drawing.Size(64, 24);
            this.ID_box.TabIndex = 3;
            // 
            // Save_button
            // 
            this.Save_button.Location = new System.Drawing.Point(432, 105);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(64, 30);
            this.Save_button.TabIndex = 4;
            this.Save_button.Text = "Next";
            this.Save_button.UseVisualStyleBackColor = true;
            this.Save_button.Click += new System.EventHandler(this.Save_button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.IndianRed;
            this.label1.Location = new System.Drawing.Point(390, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Try Record \"run run\"";
            // 
            // user_button
            // 
            this.user_button.Location = new System.Drawing.Point(263, 52);
            this.user_button.Name = "user_button";
            this.user_button.Size = new System.Drawing.Size(78, 30);
            this.user_button.TabIndex = 6;
            this.user_button.Text = "Save";
            this.user_button.UseVisualStyleBackColor = true;
            this.user_button.Click += new System.EventHandler(this.user_button_Click);
            // 
            // back_button
            // 
            this.back_button.Location = new System.Drawing.Point(12, 105);
            this.back_button.Name = "back_button";
            this.back_button.Size = new System.Drawing.Size(55, 30);
            this.back_button.TabIndex = 12;
            this.back_button.Text = "back";
            this.back_button.UseVisualStyleBackColor = true;
            this.back_button.Click += new System.EventHandler(this.back_button_Click);
            // 
            // Enroll_data
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 146);
            this.Controls.Add(this.back_button);
            this.Controls.Add(this.user_button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.ID_box);
            this.Controls.Add(this.ID_label);
            this.Controls.Add(this.user_box);
            this.Controls.Add(this.user_label);
            this.Name = "Enroll_data";
            this.Text = "Enrollment Data";
            this.Load += new System.EventHandler(this.Enroll_data_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label user_label;
        private System.Windows.Forms.TextBox user_box;
        private System.Windows.Forms.Label ID_label;
        private System.Windows.Forms.ComboBox ID_box;
        private System.Windows.Forms.Button Save_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button user_button;
        private System.Windows.Forms.Button back_button;
    }
}