using db_project;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Trainer
{
    public partial class FEEDBACK : Form
    {
        int T_ID;
        string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

        public FEEDBACK(int iD)
        {
            InitializeComponent();
            T_ID = iD;
            try
            {
                DisplayTrainerFeedback(T_ID);
                FEEDBACK_Load(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading feedback: {ex.Message}");
            }
        }

        private void FEEDBACK_Load(object sender, EventArgs e)
        {
            try
            {
                int userID = GetUserID(T_ID);
                double averageRating = GetAverageRating(userID);
                textBox1.Text = averageRating.ToString();
                DisplayTrainersRatings(T_ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading feedback: {ex.Message}");
            }
        }

        private int GetUserID(int trainerID)
        {
            int userID = -1;
            string query = $"SELECT user_id FROM Trainers WHERE trainer_id = {trainerID}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        userID = Convert.ToInt32(result);
                    }
                }
            }

            return userID;
        }

        private void DisplayTrainersRatings(int trainerID)
        {
            richTextBox2.Clear();
            int userID = GetUserID(trainerID);
            string query = $"SELECT rating FROM Trainers WHERE user_id = {userID};";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            richTextBox2.AppendText($"Rating: {reader["rating"]}\n");
                        }
                    }
                }
            }
        }

        private double GetAverageRating(int userID)
        {
            double averageRating = 0;
            string query = $"SELECT AVG(f.rating) " +
                           $"FROM TrainerFeedback f " +
                           $"JOIN Trainers t ON f.trainer_id = t.trainer_id " +
                           $"WHERE t.user_id = {userID}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        averageRating = Convert.ToDouble(result);
                    }
                }
            }

            return averageRating;
        }

        private void DisplayTrainerFeedback(int trainerID)
        {
            richTextBox1.Clear();
            richTextBox1.ReadOnly = true;
            string query = $"SELECT comment FROM TrainerFeedback WHERE trainer_id = {trainerID};";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            richTextBox1.AppendText(reader["comment"].ToString() + "\n");
                        }
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            KHULJAA obj = new KHULJAA(this.T_ID);
            this.Hide();
            obj.Show();
        }
    }
}
