using db_project;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBPROJECT
{
    public partial class Personal_training : Form
    {
        int userId;

        public Personal_training(int userid)
        {
            InitializeComponent();
            this.userId = userid;
            numericUpDown1.Value = 9; // Start hour
            numericUpDown2.Value = 0; // Start minute
            numericUpDown3.Value = 17; // End hour
            numericUpDown4.Value = 0; // End minute
            numericUpDown1.Minimum = 9; // Minimum start hour
            numericUpDown1.Maximum = 17; // Maximum start hour
            numericUpDown3.Minimum = 9; // Minimum end hour
            numericUpDown3.Maximum = 17; // Maximum end hour
            string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
            string query = "SELECT t.trainer_id, u.username FROM Trainers t JOIN Users u ON t.user_id = u.user_id WHERE t.status = 'active';";

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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkedListBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a trainer.");
                    return;
                }

                DateTime appointmentDate;
                if (!DateTime.TryParse(dateTimePicker1.Text, out appointmentDate))
                {
                    MessageBox.Show("Please pick a valid date.");
                    return;
                }

                var selectedTrainer = checkedListBox1.SelectedItem as dynamic;
                int trainerId = selectedTrainer.TrainerId;

                int startHour = (int)numericUpDown1.Value;
                int startMinute = (int)numericUpDown2.Value;
                int endHour = (int)numericUpDown3.Value;
                int endMinute = (int)numericUpDown4.Value;

                TimeSpan startTime = new TimeSpan(startHour, startMinute, 0);
                TimeSpan endTime = new TimeSpan(endHour, endMinute, 0);

                if (startTime >= endTime)
                {
                    MessageBox.Show("End time must be after start time.");
                    return;
                }

                if (startTime.Hours < 9 || startTime.Hours > 17 || endTime.Hours < 9 || endTime.Hours > 17)
                {
                    MessageBox.Show("Time must be between 9 AM and 5 PM.");
                    return;
                }

                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
                string insertQuery = "INSERT INTO Appointments (trainer_id, member_id, appointment_date, start_time, end_time, status) VALUES (@trainerId, @memberId, @appointmentDate, @startTime, @endTime, 'pending');";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@trainerId", trainerId);
                        command.Parameters.AddWithValue("@memberId", userId);
                        command.Parameters.AddWithValue("@appointmentDate", appointmentDate);
                        command.Parameters.AddWithValue("@startTime", startTime);
                        command.Parameters.AddWithValue("@endTime", endTime);
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Appointment scheduled successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Personal_training_Load(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Member_outlook obj = new Member_outlook(this.userId);
            obj.Show();
            this.Hide();
        }
    }
}
