using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Trainer
{
    public partial class ADJUST_WP : Form
    {
        int W_ID;
        int tid;
        string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
        public ADJUST_WP(int w_ID,int tid)
        {
            InitializeComponent();
            W_ID = w_ID;
            this.tid = tid;
        }

        private void PopulateWorkPlanDetails(int planID)
        {
            string query = $"SELECT title, goal FROM WorkoutPlans WHERE plan_id = {planID};";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        textBox1.Text = reader["title"].ToString();
                        textBox2.Text = reader["goal"].ToString();
                        
                    }
                }
            }
        }

        private void ADJUST_WP_Load(object sender, EventArgs e)
        {
            PopulateWorkPlanDetails (W_ID);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void UpdateWorkoutPlanDetails(int workoutID, string newTitle, string newGoal)
        {
            string query = $"UPDATE WorkoutPlans SET title = @Title, goal = @Goal WHERE plan_id = @WorkoutID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", newTitle);
                    command.Parameters.AddWithValue("@Goal", newGoal);
                    command.Parameters.AddWithValue("@WorkoutID", workoutID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string newTitle = textBox3.Text;
            string newGoal = textBox4.Text;

            if (!string.IsNullOrWhiteSpace(newTitle) && !string.IsNullOrWhiteSpace(newGoal))
            {
                UpdateWorkoutPlanDetails(W_ID, newTitle, newGoal);
                MessageBox.Show("Successfully Updated.");
            }
            else
            {
                MessageBox.Show("Please fill in all the fields.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            W_REPORT obj = new W_REPORT(this.tid);
            this.Hide();
            obj.Show();
        }
    }
}
