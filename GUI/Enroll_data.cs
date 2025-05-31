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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Recorder
{
    public partial class Enroll_data : Form
    {
        private bool isNew;        
        private string connectionString;
        private int currID;
        public Enroll_data()
        {
            InitializeComponent();
            ID_label.Visible = false;
            ID_box.Visible = false;
            Save_button.Enabled = false;
           
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
           // Console.WriteLine(dbPath);
            connectionString = $@"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        private void user_button_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(user_box.Text))
            {
                DialogResult result = MessageBox.Show(
                    "Are you new?", // message
                    "Confirmation",            // title
                    MessageBoxButtons.YesNo,   // buttons
                    MessageBoxIcon.Question    // icon
                );

                if (result == DialogResult.Yes)
                {
                    isNew = true;
                    string username = user_box.Text;
                    //int userid = Convert.ToInt32(ID_box.SelectedItem);              
                    // Create the SQL insert command

                    string query = @"
                                INSERT INTO voice_enrollment_final (user_name) 
                                VALUES (@Username); 
                                SELECT SCOPE_IDENTITY();";

                    string checkQuery = @"SELECT COUNT(*) FROM voice_enrollment_final WHERE user_name = @Username";

                    try
                    {
                        // Open the connection
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            // Create the command
                            using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                            {
                                checkCmd.Parameters.AddWithValue("@Username", username);
                                int existingCount = (int)checkCmd.ExecuteScalar();

                                if (existingCount > 0)
                                {
                                    MessageBox.Show($"The username '{username}' already exists. Please choose a different one.");
                                    return;
                                }
                            }
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                // Add parameters to avoid SQL injection                            
                                cmd.Parameters.AddWithValue("@Username", username);

                                object resultId = cmd.ExecuteScalar();
                                int newId = Convert.ToInt32(resultId);
                                currID = newId;                              
                                MessageBox.Show($"Record inserted successfully!\nUsername: {username}\nUser ID: {newId}");
                            }
                        }
                        Save_button.Enabled = true;
                        user_button.Enabled = false;
                        ID_label.Visible = false;
                        ID_box.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
                else if (result == DialogResult.No)
                {
                    user_button.Enabled = false;
                    string username = user_box.Text;
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();

                            string query = "SELECT id FROM voice_enrollment_final WHERE user_name = @Username";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@Username", username);

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    ID_box.Items.Clear();

                                    while (reader.Read())
                                    {
                                        ID_box.Items.Add(reader["id"].ToString());
                                    }
                                }
                            }
                        }

                        if (ID_box.Items.Count == 0)
                        {
                            MessageBox.Show("No IDs found for the entered username.");
                            user_button.Enabled = true;
                        }
                        else
                        {
                            ID_box.SelectedIndex = 0;
                            isNew = false;
                            ID_label.Visible = true;
                            ID_box.Visible = true;
                            Save_button.Enabled = true;
                        }                       
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Fill the Username Field!");
            }
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            Enrollment mainForm;
            
            if (isNew)
            {
                mainForm = new Enrollment(user_box.Text, currID);
            }
            else
            {
                mainForm = new Enrollment(user_box.Text, int.Parse(ID_box.Text));
            }
            mainForm.Show();
            this.Hide();
        }      
        private void back_button_Click(object sender, EventArgs e)
        {
            GUI mainForm = new GUI();
            mainForm.Show();
            this.Hide();
        }

        private void Enroll_data_Load(object sender, EventArgs e)
        {

        }
    }
}
