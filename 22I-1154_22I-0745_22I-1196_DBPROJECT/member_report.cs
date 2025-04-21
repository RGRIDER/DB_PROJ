using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace db_project
{
    public partial class member_report : Form
    {
        int id;
        public member_report(int id)
        {
            InitializeComponent();
            this.id = id;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(textBox3.Text, out int memberId))
                {
                    MessageBox.Show("Please enter a valid member ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string query = @"SELECT m.Height, m.weightt 
                                 FROM Memberships m
                                 INNER JOIN Users u ON m.member_id = u.user_id
                                 WHERE m.member_id = @MemberID AND u.role = 'member';";

                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True"))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MemberID", memberId);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            textBox5.Text = reader["Height"].ToString();
                            textBox6.Text = reader["weightt"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("The entered ID does not correspond to a valid member.", "Member Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
             
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
             
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
             
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(textBox3.Text, out int memberId))
                {
                    MessageBox.Show("Please enter a valid member ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string query = @"SELECT dp.title AS DietPlanTitle, dp.type AS DietPlanType, dp.goal AS DietPlanGoal
                                 FROM Users u
                                 JOIN DietPlanUsage dpu ON u.user_id = dpu.member_id
                                 JOIN DietPlans dp ON dpu.diet_id = dp.diet_id
                                 WHERE u.user_id = @MemberID;";

                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True"))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MemberID", memberId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    listBox1.Items.Clear();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string dietPlanTitle = reader["DietPlanTitle"].ToString();
                            string dietPlanType = reader["DietPlanType"].ToString();
                            string dietPlanGoal = reader["DietPlanGoal"].ToString();

                            listBox1.Items.Add($"Title: {dietPlanTitle}, Type: {dietPlanType}, Goal: {dietPlanGoal}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No diet plan found for the specified member ID.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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

        private void button6_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(textBox3.Text, out int memberId))
                {
                    MessageBox.Show("Please enter a valid member ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string query = @"SELECT dp.title AS workoutTitle, dp.goal AS workoutGoal
                                 FROM Users u
                                 JOIN MemberPlanUsage dpu ON u.user_id = dpu.member_id
                                 JOIN WorkoutPlans dp ON dpu.plan_id = dp.plan_id
                                 WHERE u.user_id = @MemberID;";

                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True"))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MemberID", memberId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    listBox1.Items.Clear();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string Title = reader["workoutTitle"].ToString();
                            string Goal = reader["workoutGoal"].ToString();

                            listBox1.Items.Add($"Title: {Title},  Goal: {Goal}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No workout plan found for the specified member ID.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void member_report_Load(object sender, EventArgs e)
        {
             
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                gymowner_outlook obj = new gymowner_outlook(id);
                obj.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
