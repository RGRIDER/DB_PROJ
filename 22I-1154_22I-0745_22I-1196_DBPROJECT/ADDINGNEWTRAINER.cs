using DBPROJECT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace db_project
{
    public partial class ADDINGNEWTRAINER : Form
    {
        int ownerid;
        public ADDINGNEWTRAINER(int ownerId)
        {
            InitializeComponent();
            string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT gym_id FROM Gyms";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int gymId = reader.GetInt32(0);
                                listBox1.Items.Add(gymId);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading gym IDs: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }


        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox4.Text;
            string firstName = textBox2.Text;
            string lastName = textBox6.Text;
            string email = textBox7.Text;
            string phone = textBox3.Text;

            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a gym.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int gymId = Convert.ToInt32(listBox1.SelectedItem);
            int experience;
            if (!int.TryParse(textBox5.Text, out experience) || experience < 0)
            {
                MessageBox.Show("Please enter a valid experience value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string specialty = textBox8.Text;
            if (string.IsNullOrWhiteSpace(specialty))
            {
                MessageBox.Show("Please enter a specialty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string insertUserQuery = "INSERT INTO Users (username, password, first_name, last_name, email, phone, role) " +
                                             "VALUES (@Username, @Password, @FirstName, @LastName, @Email, @Phone, 'trainer'); " +
                                             "SELECT SCOPE_IDENTITY();"; 

                    int newTrainerUserId;
                    using (SqlCommand cmd = new SqlCommand(insertUserQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        newTrainerUserId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    string insertTrainerQuery = "INSERT INTO Trainers (user_id, gym_id, experience, specialty) " +
                                                "VALUES (@UserId, @GymId, @Experience, @Specialty);";


                    using (SqlCommand trainerCmd = new SqlCommand(insertTrainerQuery, conn))
                    {
                        trainerCmd.Parameters.AddWithValue("@UserId", newTrainerUserId);
                        trainerCmd.Parameters.AddWithValue("@GymId", gymId);
                        trainerCmd.Parameters.AddWithValue("@Experience", experience);
                        trainerCmd.Parameters.AddWithValue("@Specialty", specialty); 

                        trainerCmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

                MessageBox.Show("Trainer added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // MessageBox.Show(listBox1.SelectedItem.ToString());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void ADDINGNEWTRAINER_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            gymowner_outlook obj = new gymowner_outlook(this.ownerid);
            obj.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
