using db_project;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Admin
{
    public partial class M_APPROVAL : Form
    {
        public M_APPROVAL()
        {
            InitializeComponent();
            LoadApprovedGyms();
        }
        private void LoadApprovedGyms()
        {
            try
            {
                listBox1.Items.Clear();

                string query = "SELECT gym_name FROM Gyms where status = 'pending';";

                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True"))
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string gymName = reader.GetString(0);
                        listBox1.Items.Add(gymName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading gyms: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChangeGymStatus("active", "approved");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChangeGymStatus("inactive", "disapproved");
        }

        private void ChangeGymStatus(string newStatus, string action)
        {
            try
            {
                if (listBox1.SelectedIndex != -1)
                {
                    string selectedGymName = listBox1.SelectedItem.ToString();

                    string updateStatusQuery = "UPDATE Gyms SET status = @NewStatus WHERE gym_name = @GymName;";

                    using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True"))
                    {
                        SqlCommand command = new SqlCommand(updateStatusQuery, connection);
                        command.Parameters.AddWithValue("@NewStatus", newStatus);
                        command.Parameters.AddWithValue("@GymName", selectedGymName);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Gym status changed to {newStatus} successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadApprovedGyms();
                        }
                        else
                        {
                            MessageBox.Show($"Failed to {action} gym.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a gym first.", "No Gym Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing gym status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex != -1)
                {
                    string selectedGymName = listBox1.SelectedItem.ToString();

                    string query = "SELECT owner_id, status, registration_date, address FROM Gyms WHERE gym_name = @GymName;";

                    using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True"))
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@GymName", selectedGymName);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            textBox1.Text = reader.GetInt32(0).ToString();
                            textBox3.Text = reader.GetString(1);
                            textBox4.Text = reader.GetDateTime(2).ToString();
                            textBox2.Text = reader.GetString(3);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a gym first.", "No Gym Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading gym details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void M_APPROVAL_Load(object sender, EventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ADMINKAMAINHAI obj = new ADMINKAMAINHAI();
            obj.Show();
            this.Hide();
        }
    }
}
