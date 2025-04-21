using db_project;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Trainer
{
    public partial class APPOINMENT : Form
    {

        string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
        bool showingApprovedAppointments = false;
        int T_ID;
        public APPOINMENT(int id)
        {
            InitializeComponent();
            listBox1.Items.Clear();
            T_ID = id;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                listBox1.Items.Add("List of Approved Appointment IDs:");

                string query = "SELECT appointment_id FROM Appointments WHERE status = 'approved' AND trainer_id=@T_ID;";

                PopulateListBoxWithAppointments(query);

                showingApprovedAppointments = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                listBox1.Items.Add("List of Pending Appointment IDs:");

                string query = "SELECT appointment_id FROM Appointments WHERE status = 'pending' AND trainer_id = @T_ID;";

                PopulateListBoxWithAppointments(query);

                showingApprovedAppointments = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex > 0)
                {
                    int selectedAppointmentID = Convert.ToInt32(listBox1.SelectedItem);

                    string query = $"UPDATE Appointments SET status = 'approved' WHERE appointment_id = {selectedAppointmentID};";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Appointment status updated to approved.");
                                if (showingApprovedAppointments)
                                    button4_Click(sender, e);
                                else
                                    button3_Click(sender, e);
                            }
                            else
                            {
                                MessageBox.Show("Failed to update appointment status.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a valid appointment ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateListBoxWithAppointments(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@T_ID", T_ID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                listBox1.Items.Add(reader["appointment_id"]);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No appointments found.");
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex > 0)
                {
                    int selectedAppointmentID = Convert.ToInt32(listBox1.SelectedItem);

                    string query = $"UPDATE Appointments SET status = 'rejected' WHERE appointment_id = {selectedAppointmentID};";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Appointment status updated to rejected.");
                                if (showingApprovedAppointments)
                                    button4_Click(sender, e);
                                else
                                    button3_Click(sender, e);
                            }
                            else
                            {
                                MessageBox.Show("Failed to update appointment status.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a valid appointment ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex > 0)
                {
                    int selectedAppointmentID = Convert.ToInt32(listBox1.SelectedItem);

                    RESCHEDULE rescheduleForm = new RESCHEDULE(selectedAppointmentID, this);
                    rescheduleForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Please select a valid appointment ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void APPOINMENT_Load(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            KHULJAA obj = new KHULJAA(this.T_ID);
            this.Hide();
            obj.Show();
        }
    }
}
