using db_project;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBPROJECT
{
    public partial class DIETPLANREPORT : Form
    {
        int userId;
        public DIETPLANREPORT(int userid)
        {
            InitializeComponent();
            this.userId = userid;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void DIETPLANREPORT_Load(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
            string query = "SELECT diet_id, title FROM DietPlans WHERE creator_id = @userId";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", this.userId);
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
            catch (SqlException ex)
            {
                MessageBox.Show($"Error fetching diet plans: {ex.Message}");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
            string query = @"
            SELECT wp.diet_id, wp.title
            FROM DietPlans wp
            JOIN Users u ON wp.creator_id = u.user_id
            WHERE wp.creator_id != @userId
            AND u.role = 'member'";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
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
            catch (SqlException ex)
            {
                MessageBox.Show($"Error fetching diet plans: {ex.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
            string query = @"
            SELECT wp.diet_id, wp.title
            FROM DietPlans wp
            JOIN Users u ON wp.creator_id = u.user_id
            WHERE wp.creator_id != @userId
            AND u.role = 'trainer'";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
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
            catch (SqlException ex)
            {
                MessageBox.Show($"Error fetching diet plans: {ex.Message}");
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem is string selectedGoal)
            {
                listBox1.Items.Clear();

                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
                string query = @"
            SELECT dp.diet_id, dp.title
            FROM DietPlans dp
            WHERE dp.goal = @selectedGoal";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@selectedGoal", selectedGoal);
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
                catch (SqlException ex)
                {
                    MessageBox.Show($"Error fetching diet plans: {ex.Message}");
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                var selectedItem = listBox1.SelectedItem;

                if (selectedItem.GetType().GetProperty("PlanId") != null)
                {
                    int planId = (int)selectedItem.GetType().GetProperty("PlanId").GetValue(selectedItem);
                    int creatorId;
                    string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
                    string query = "SELECT creator_id FROM DietPlans WHERE diet_id = @planId";

                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@planId", planId);
                                connection.Open();
                                creatorId = (int)command.ExecuteScalar();
                            }
                        }

                        string insertQuery = @"
                INSERT INTO DietPlanUsage (member_id, diet_id, creator_id)
                VALUES (@memberId, @dietId, @creatorId)";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand command = new SqlCommand(insertQuery, connection))
                            {
                                command.Parameters.AddWithValue("@memberId", userId);
                                command.Parameters.AddWithValue("@dietId", planId);
                                command.Parameters.AddWithValue("@creatorId", creatorId);

                                connection.Open();

                                try
                                {
                                    command.ExecuteNonQuery();
                                    MessageBox.Show("Diet plan usage saved successfully.");
                                }
                                catch (SqlException ex)
                                {
                                    MessageBox.Show($"Error saving diet plan usage: {ex.Message}");
                                }
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"Error retrieving diet plan creator ID: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("The selected item does not have a valid PlanId.");
                }
            }
            else
            {
                MessageBox.Show("Please select a diet plan to save.");
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Member_outlook obj = new Member_outlook(this.userId);
            this.Hide();
            obj.Show();
            
        }
    }
}
