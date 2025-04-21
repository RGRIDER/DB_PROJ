using db_project;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Trainer
{
    public partial class W_REPORT : Form
    {
        int T_ID;
        string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

        public W_REPORT(int iD)
        {
            InitializeComponent();
            T_ID = iD;
        }

        private int GetUserID(int trainerID)
        {
            try
            {
                int userID = -1;
                string query = $"SELECT user_id FROM Trainers WHERE trainer_id = {trainerID};";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            userID = (int)result;
                        }
                    }
                }

                return userID;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting user ID: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItems.Count == 1)
                {
                    int selectedWID = (int)listBox1.SelectedItem;
                    ADJUST_WP aDJUST_WP = new ADJUST_WP(selectedWID, this.T_ID);
                    aDJUST_WP.Show();
                    this.Hide();
                }
                else if (listBox1.SelectedItems.Count > 1)
                {
                    MessageBox.Show("Please select only one work plan.");
                }
                else
                {
                    MessageBox.Show("Please select a work plan.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void W_REPORT_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateW_IDS();
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
                Populate_W_IDs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Populate_W_IDs()
        {
            try
            {
                if (listBox1.SelectedIndex != -1)
                {
                    listBox2.Items.Clear();
                    int selectedWID = (int)listBox1.SelectedItem;
                    string query = $"SELECT member_id FROM MemberPlanUsage WHERE plan_id = {selectedWID};";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                int memberID = (int)reader["member_id"];
                                listBox2.Items.Add(memberID);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a diet plan first.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error populating workout IDs: " + ex.Message);
            }
        }

        private void PopulateW_IDS()
        {
            try
            {
                listBox1.Items.Clear();
                int userID = GetUserID(T_ID);
                string query = "SELECT plan_id FROM  WorkoutPlans WHERE creator_id = @UserID;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int planID = (int)reader["plan_id"];
                            listBox1.Items.Add(planID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error populating workout IDs: " + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            KHULJAA obj = new KHULJAA(this.T_ID);
            this.Hide();
            obj.Show();
        }


        private void label3_Click(object sender, EventArgs e) { }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
    }
}
