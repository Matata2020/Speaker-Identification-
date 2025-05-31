//using Recorder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Recorder
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Enroll_data mainForm = new Enroll_data();
            mainForm.Show();
            this.Hide();
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

            string connectionString = $@"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            /*using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string selectQuery = "SELECT template_sequence FROM voice_templates";

                using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    int row = 0;
                    while (reader.Read())
                    {
                        string template = reader["template_sequence"] as string;
                        Console.WriteLine($"Row {++row}:\n");
                        Console.WriteLine(template);
                        Console.WriteLine("------------------------------------------------------\n");
                    }
                }
            }*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 mainForm = new Form1();
            mainForm.Show();
            this.Hide();
        }
    }
}
