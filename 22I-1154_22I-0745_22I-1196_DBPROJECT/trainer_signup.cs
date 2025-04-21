using DBPROJECT;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace db_project
{
    public partial class trainer_signup : Form
    {
        string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

        public trainer_signup()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e) { }

        private void button6_Click(object sender, EventArgs e) { }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            trainer_login obj = new trainer_login();
            obj.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string username = textBox2.Text;
                string firstName = textBox7.Text;
                string lastName = textBox6.Text;
                string email = textBox1.Text;
                string password = textBox4.Text;
                string phone = textBox8.Text;
                string role = "trainer";
                DateTime registrationDate = DateTime.Today;
                string status = "inactive";

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(firstName) ||
                    string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(phone))
                {
                    throw new Exception("Please fill in all the fields.");
                }

                if (!IsEmailUnique(email))
                {
                    throw new Exception("Email already exists. Please choose a different email.");
                }

                InsertNewTrainer(username, password, firstName, lastName, email, phone, role, registrationDate, status);
                MessageBox.Show("New trainer added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool IsEmailUnique(string email)
        {
            try
            {
                string query = $"SELECT COUNT(*) FROM Users WHERE email = '{email}';";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int count = (int)command.ExecuteScalar();
                        return count == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking email uniqueness: " + ex.Message);
            }
        }

        private void InsertNewTrainer(string username, string password, string firstName, string lastName,
                                       string email, string phone, string role, DateTime registrationDate, string status)
        {
            try
            {
                string userInsertQuery = "INSERT INTO Users (username, password, first_name, last_name, email, phone, role, registration_date, status) " +
                                         $"VALUES ('{username}', '{password}', '{firstName}', '{lastName}', '{email}', '{phone}', '{role}', '{registrationDate:yyyy-MM-dd}', '{status}');";

                int userId;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(userInsertQuery, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        string getUserIdQuery = $"SELECT user_id FROM Users WHERE email = '{email}';";
                        command.CommandText = getUserIdQuery;
                        userId = (int)command.ExecuteScalar();
                    }
                }

                string trainerInsertQuery = "INSERT INTO Trainers (user_id, gym_id, status) " +
                                            $"VALUES ({userId}, @gymId, 'inactive');";

                int gymId = Convert.ToInt32(listBox1.SelectedItem);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(trainerInsertQuery, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@gymId", gymId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting new trainer: " + ex.Message);
            }
        }

        private void trainer_signup_Load(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT gym_id FROM Gyms";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        listBox1.Items.Clear();

                        while (reader.Read())
                        {
                            int gymID = (int)reader["gym_id"];
                            listBox1.Items.Add(gymID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading gyms: " + ex.Message);
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox7_TextChanged(object sender, EventArgs e) { }
        private void textBox6_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged_1(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void textBox8_TextChanged(object sender, EventArgs e) { }
        private void textBox5_TextChanged(object sender, EventArgs e) { }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) 
        { 
            Application.Exit(); 
        }
        private void button4_Click(object sender, EventArgs e) 
        { 
            trainer_login obj = new trainer_login(); 
            obj.Show(); 
            this.Hide(); 
        }
    }
}
