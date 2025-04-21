using db_project;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Trainer
{
    public partial class D_CREATION : Form
    {
        int T_ID;
        string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

        public D_CREATION(int iD)
        {
            InitializeComponent();
            T_ID = iD;
        }

        private bool IsValidDietType(string type)
        {
            string[] validTypes = { "omnivore", "vegan", "vegetarian", "pescatarian", "gluten-free", "lactose-free" };
            return validTypes.Contains(type.ToLower());
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

        private int InsertDietPlan(int creatorID, string title, string type, string goal)
        {
            int dietID = 0;

            string query = $"INSERT INTO DietPlans (creator_id, title, type, goal) OUTPUT INSERTED.diet_id " +
                           $"VALUES (@CreatorID, @Title, @Type, @Goal);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CreatorID", creatorID);
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Type", type);
                    command.Parameters.AddWithValue("@Goal", goal);

                    connection.Open();
                    dietID = (int)command.ExecuteScalar();
                }
            }

            return dietID;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox4.Text))
                {
                    string type = textBox4.Text.Trim();
                    if (IsValidDietType(type))
                    {
                        string title = textBox1.Text;
                        string goal = textBox2.Text;

                        int trainerID = GetUserID(T_ID);

                        int dietID = InsertDietPlan(trainerID, title, type, goal);

                        MessageBox.Show("Diet plan created successfully.");

                        DIET_DETAILS dietDetailsForm = new DIET_DETAILS(dietID,this.T_ID);
                        dietDetailsForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid diet type.");
                    }
                }
                else
                {
                    MessageBox.Show("Please fill in all the fields.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void D_CREATION_Load(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            KHULJAA obj = new KHULJAA(this.T_ID);
            this.Hide();
            obj.Show();
        }
    }
}
