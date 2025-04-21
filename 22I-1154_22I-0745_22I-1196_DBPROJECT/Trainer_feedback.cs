using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace db_project
{
    public partial class Trainer_feedback : Form
    {
        int userId;
        int r;

        public Trainer_feedback(int userid)
        {
            InitializeComponent();
            this.userId = userid;
            this.r = 0;
            LoadTrainers();
        }

        private void LoadTrainers()
        {
            string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

            string query = @"
                SELECT t.trainer_id, u.username
                FROM Trainers t
                JOIN Users u ON t.user_id = u.user_id
                WHERE t.status = 'active'";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            checkedListBox1.Items.Clear();

                            while (reader.Read())
                            {
                                int trainerId = reader.GetInt32(0);
                                string trainerName = reader.GetString(1);
                                checkedListBox1.Items.Add(new { TrainerId = trainerId, TrainerName = trainerName });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading trainers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string comment = textBox3.Text;

            if (checkedListBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a trainer.");
                return;
            }

            if (r < 1 || r > 5)
            {
                MessageBox.Show("Please select a rating.");
                return;
            }

            dynamic selectedTrainer = checkedListBox1.SelectedItem;
            int trainerId = selectedTrainer.TrainerId;

            string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

            string insertQuery = @"
                INSERT INTO TrainerFeedback (member_id, trainer_id, rating, comment)
                VALUES (@memberId, @trainerId, @rating, @comment)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@memberId", userId);
                        command.Parameters.AddWithValue("@trainerId", trainerId);
                        command.Parameters.AddWithValue("@rating", r);
                        command.Parameters.AddWithValue("@comment", comment);

                        connection.Open();
                        command.ExecuteNonQuery();

                        MessageBox.Show("Feedback submitted successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.r = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.r = 2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.r = 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.r = 4;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.r = 5;
        }

        // Empty functions
        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Member_outlook obj = new Member_outlook(this.userId);
            obj.Show();
            this.Hide();
        }
    }
}
