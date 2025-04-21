using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace db_project
{
    public partial class ADMIN_REVOKE : Form
    {
        public ADMIN_REVOKE()
        {
            InitializeComponent();
            LoadApprovedGyms();
        }

        private void LoadApprovedGyms()
        {
            listBox1.Items.Clear();

            string query = "SELECT gym_name FROM Gyms where status = 'active';";

            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True"))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string gymName = reader.GetString(0);
                        listBox1.Items.Add(gymName);
                    }

                    if (listBox1.Items.Count == 0)
                    {
                        MessageBox.Show("No gyms found.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading gyms: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string selectedGymName = listBox1.SelectedItem.ToString();
                string updateGymStatusQuery = "UPDATE Gyms SET status = 'inactive' WHERE gym_name = @GymName;";

                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True"))
                {
                    connection.Open();

                    try
                    {
                        SqlCommand updateGymStatusCommand = new SqlCommand(updateGymStatusQuery, connection);
                        updateGymStatusCommand.Parameters.AddWithValue("@GymName", selectedGymName);
                        int rowsAffected = updateGymStatusCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Gym status changed to inactive successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadApprovedGyms();
                        }
                        else
                        {
                            MessageBox.Show("Failed to change gym status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error changing gym status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a gym first.", "No Gym Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ADMINKAMAINHAI obj = new ADMINKAMAINHAI();
            obj.Show();
            this.Hide();
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
            if (listBox1.SelectedIndex != -1)
            {
                string selectedGymName = listBox1.SelectedItem.ToString();

                string query = "SELECT owner_id, status, registration_date, address FROM Gyms WHERE gym_name = @GymName;";

                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True"))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@GymName", selectedGymName);

                    try
                    {
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
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading gym details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a gym first.", "No Gym Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
             
        }

        private void ADMIN_REVOKE_Load(object sender, EventArgs e)
        {
             
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ADMINKAMAINHAI obj = new ADMINKAMAINHAI();
            obj.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
