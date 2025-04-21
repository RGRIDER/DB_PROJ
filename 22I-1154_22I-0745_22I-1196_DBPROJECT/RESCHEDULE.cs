using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Trainer
{
    public partial class RESCHEDULE : Form
    {
        private int appointmentID;
        string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
        private Form previousForm;

        public RESCHEDULE(int appointmentID, Form previousForm)
        {
            InitializeComponent();

            this.appointmentID = appointmentID;
            this.previousForm = previousForm;

            label2.Text = "Appointment ID: " + appointmentID.ToString();

            string query = $"SELECT appointment_date, start_time, end_time FROM Appointments WHERE appointment_id = {appointmentID};";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DateTime appointmentDate = Convert.ToDateTime(reader["appointment_date"]);
                                label9.Text = "Appointment Date: " + appointmentDate.ToShortDateString();
                                label10.Text = "Start Time: " + reader["start_time"].ToString();
                                label11.Text = "End Time: " + reader["end_time"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Appointment details not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while retrieving appointment details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTimePicker1.Value;
            DateTime currentDate = DateTime.Today;

            if (selectedDate < currentDate)
            {
                dateTimePicker1.MinDate = currentDate;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime newDate = dateTimePicker1.Value.Date;

            TimeSpan startTime;
            if (TimeSpan.TryParse(textBox3.Text, out startTime))
            {
                TimeSpan endTime;
                if (TimeSpan.TryParse(textBox4.Text, out endTime))
                {
                    if (endTime > startTime)
                    {
                        string query = $"UPDATE Appointments SET appointment_date = @NewDate, start_time = @StartTime, end_time = @EndTime WHERE appointment_id = {appointmentID};";

                        try
                        {
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.Parameters.AddWithValue("@NewDate", newDate);
                                    command.Parameters.AddWithValue("@StartTime", startTime);
                                    command.Parameters.AddWithValue("@EndTime", endTime);

                                    connection.Open();
                                    int rowsAffected = command.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        MessageBox.Show("Appointment details updated successfully.");
                                        this.Hide();
                                        previousForm.Show();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to update appointment details.");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred while updating appointment details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("End time must be after start time.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid end time format. Please enter a valid time in HH:mm format.");
                }
            }
            else
            {
                MessageBox.Show("Invalid start time format. Please enter a valid time in HH:mm format.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            APPOINMENT obj = new APPOINMENT(this.appointmentID);
            this.Hide();
            obj.Show();
        }

         
        private void label5_Click(object sender, EventArgs e)
        {

        }

         
        private void label2_Click(object sender, EventArgs e)
        {

        }

         
        private void label4_Click(object sender, EventArgs e)
        {

        }

         
        private void RESCHEDULE_Load(object sender, EventArgs e)
        {

        }

         
        private void button5_Click(object sender, EventArgs e)
        {

        }

         
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

         
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

         
        private void label9_Click(object sender, EventArgs e)
        {

        }

         
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

         
        private void label10_Click(object sender, EventArgs e)
        {

        }

         
        private void label11_Click(object sender, EventArgs e)
        {

        }

         
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
