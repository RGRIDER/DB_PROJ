using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Trainer
{
    public partial class ADJUST_DP : Form
    {
        int D_ID;
        int TID;
        string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
        public ADJUST_DP(int d_ID, int Tid)
        {
            InitializeComponent();
            D_ID = d_ID;
            TID = Tid;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void ADJUST_DP_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateDietPlanDetails(D_ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading diet plan details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PopulateDietPlanDetails(int dietID)
        {
            string query = $"SELECT title, goal, type FROM DietPlans WHERE diet_id = {dietID};";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        textBox1.Text = reader["title"].ToString();
                        textBox3.Text = reader["goal"].ToString();
                        textBox5.Text = reader["type"].ToString();
                    }
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpdateDietPlanDetails(int dietID, string newTitle, string newGoal, string newType)
        {
            try
            {
                string query = $"UPDATE DietPlans SET title = '{newTitle}', goal = '{newGoal}', type = '{newType}' WHERE diet_id = {dietID};";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Diet plan details updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating diet plan details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string newTitle = textBox4.Text;
                string newGoal = textBox2.Text;
                string newType = textBox6.Text;
                if (IsValidMealType(newType))
                {
                    UpdateDietPlanDetails(D_ID, newTitle, newGoal, newType);
                }
                else
                {
                    MessageBox.Show("Please enter a valid meal type.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool IsValidMealType(string type)
        {
            string[] validTypes = { "omnivore", "vegan", "vegetarian", "pescatarian", "gluten-free", "lactose-free" };

            return validTypes.Contains(type.ToLower());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            D_REPORT obj = new D_REPORT(this.TID);
            this.Hide();
            obj.Show();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
