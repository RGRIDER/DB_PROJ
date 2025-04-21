using db_project;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBPROJECT
{
    public partial class WORKOUTPLANREPORT : Form
    {
        int UserId;

        public WORKOUTPLANREPORT(int userid)
        {
            InitializeComponent();
            this.UserId = userid;
        }

        private void WORKOUTPLANREPORT_Load(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem == null)
                {
                    MessageBox.Show("Please select a plan from the list.");
                    return;
                }

                var selectedItem = (dynamic)listBox1.SelectedItem;
                int selectedPlanId = selectedItem.PlanId;

                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string getCreatorIdQuery = @"SELECT creator_id FROM WorkoutPlans WHERE plan_id = @planId";
                    int creatorId;
                    using (SqlCommand getCreatorIdCommand = new SqlCommand(getCreatorIdQuery, connection))
                    {
                        getCreatorIdCommand.Parameters.AddWithValue("@planId", selectedPlanId);
                        creatorId = (int)getCreatorIdCommand.ExecuteScalar();
                    }

                    string insertQuery = @"INSERT INTO MemberPlanUsage (member_id, plan_id, creator_id) VALUES (@memberId, @planId, @creatorId)";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@memberId", this.UserId);
                        command.Parameters.AddWithValue("@planId", selectedPlanId);
                        command.Parameters.AddWithValue("@creatorId", creatorId);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Selected plan added to your usage successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
                string query = @"SELECT wp.plan_id, wp.title FROM WorkoutPlans wp JOIN Users u ON wp.creator_id = u.user_id WHERE wp.creator_id != @userId AND u.role = 'trainer'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", UserId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int planId = reader.GetInt32(0);
                                string title = reader.GetString(1);
                                listBox1.Items.Add(new { PlanId = planId, Title = title });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
                string query = "SELECT plan_id, title FROM WorkoutPlans WHERE creator_id = @userId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", this.UserId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int planId = reader.GetInt32(0);
                                string title = reader.GetString(1);
                                listBox1.Items.Add(new { PlanId = planId, Title = title });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
                string query = @"SELECT wp.plan_id, wp.title FROM WorkoutPlans wp JOIN Users u ON wp.creator_id = u.user_id WHERE wp.creator_id != @userId AND u.role = 'member'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", UserId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int planId = reader.GetInt32(0);
                                string title = reader.GetString(1);
                                listBox1.Items.Add(new { PlanId = planId, Title = title });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Member_outlook obj = new Member_outlook(this.UserId);
            obj.Show();
            this.Hide();
        }
    }
}
