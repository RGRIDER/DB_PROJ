using db_project;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Trainer
{
    public partial class D_REPORT : Form
    {
        int T_ID;
        string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

        public D_REPORT(int id)
        {
            InitializeComponent();
            T_ID = id;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItems.Count == 1)
                {
                    int selectedDietID = (int)listBox1.SelectedItem;
                    ADJUST_DP adjustDpForm = new ADJUST_DP(selectedDietID, this.T_ID);
                    adjustDpForm.Show();
                    this.Hide();
                }
                else if (listBox1.SelectedItems.Count > 1)
                {
                    MessageBox.Show("Please select only one diet plan.");
                }
                else
                {
                    MessageBox.Show("Please select a diet plan.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PopulateDietIDs()
        {
            listBox1.Items.Clear();
            int userID = GetUserID(T_ID);
            string query = "SELECT diet_id FROM DietPlans WHERE creator_id = @UserID;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int dietID = (int)reader["diet_id"];
                        listBox1.Items.Add(dietID);
                    }
                }
            }
        }

        private int GetUserID(int trainerID)
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

        private void D_REPORT_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateDietIDs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PopulateMemberIDs()
        {
            if (listBox1.SelectedIndex != -1)
            {
                listBox2.Items.Clear();
                int selectedDietID = (int)listBox1.SelectedItem;
                string query = $"SELECT member_id FROM DietPlanUsage WHERE diet_id = {selectedDietID};";

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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                PopulateMemberIDs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
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

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
