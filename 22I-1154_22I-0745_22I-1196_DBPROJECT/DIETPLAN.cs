using db_project;
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

namespace DBPROJECT
{
    public partial class DIETPLAN : Form
    {
        int userId;
        int currentPlanId;
        public class MealItem
        {
            public int MealId { get; set; }
            public string MealName { get; set; }

            public MealItem(int mealId, string mealName)
            {
                MealId = mealId;
                MealName = mealName;
            }
        }


        public DIETPLAN(int userid)
        {
            InitializeComponent();
            this.userId = userid;
            currentPlanId = -1;
            loadMeals();
            LoadAllergens();
        }

        private void loadMeals()
        {
            string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
            string query = "SELECT meal_id, meal_name FROM Meals";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        checkedListBox1.Items.Clear();
                        while (reader.Read())
                        {
                            int meal_id = reader.GetInt32(0);
                            string meal_name = reader.GetString(1);
                            MealItem mealitem = new MealItem(meal_id, meal_name);
                            checkedListBox1.Items.Add(mealitem);
                        }
                    }
                }
            }

            checkedListBox1.DisplayMember = "MealName";
        }


        private void LoadAllergens()
        {
            string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
            string query = "SELECT DISTINCT allergens FROM Meals WHERE allergens IS NOT NULL";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        comboBox1.Items.Clear();
                        while (reader.Read())
                        {
                            string allergens = reader.GetString(0);
                            comboBox1.Items.Add(allergens);
                        }
                    }
                }
            }
        }

        private void DIETPLAN_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
            string title = textBox5.Text;
            int creatorId = this.userId;
            string goal = comboBox3.SelectedItem?.ToString();
            string type = comboBox2.SelectedItem?.ToString();
            DateTime planDate = DateTime.Now;

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Please enter a title for the diet plan.");
                return;
            }
            if (string.IsNullOrWhiteSpace(goal))
            {
                MessageBox.Show("Please enter a goal for the diet plan.");
                return;
            }
            if (string.IsNullOrWhiteSpace(type))
            {
                MessageBox.Show("Please select a type for the diet plan.");
                return;
            }

            int currentPlanId = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string insertPlanQuery = @"
        INSERT INTO DietPlans (creator_id, title, type, goal, plan_date, status)
        VALUES (@creatorId, @title, @type, @goal, @planDate, @status);
        SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(insertPlanQuery, connection))
                    {
                        command.Parameters.AddWithValue("@creatorId", creatorId);
                        command.Parameters.AddWithValue("@title", title);
                        command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@goal", goal);
                        command.Parameters.AddWithValue("@planDate", planDate);
                        command.Parameters.AddWithValue("@status", "active");

                        currentPlanId = Convert.ToInt32(command.ExecuteScalar());
                        MessageBox.Show($"Diet plan created successfully with ID: {currentPlanId}");
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string insertQuery = @"
        INSERT INTO DietPlanUsage (member_id, diet_id, creator_id)
        VALUES (@memberId, @planId, @creatorId)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@memberId", this.userId);
                        command.Parameters.AddWithValue("@planId", currentPlanId);
                        command.Parameters.AddWithValue("@creatorId", this.userId);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error creating diet plan: {ex.Message}");
            }
        }

        private int GetCurrentDietPlanId()
        {
            string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
            int currentPlanId = -1;
            string query = "SELECT TOP 1 diet_id FROM DietPlans ORDER BY diet_id DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        currentPlanId = Convert.ToInt32(result);
                    }
                }
            }

            return currentPlanId;
        }


        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int currentPlanId = GetCurrentDietPlanId();

            if (currentPlanId == -1)
            {
                MessageBox.Show("Error: Could not retrieve current diet plan ID.");
                return;
            }

            foreach (var item in checkedListBox1.CheckedItems)
            {
                MealItem meal = item as MealItem;

                if (meal != null)
                {
                    int mealId = meal.MealId;
                    string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string insertQuery = "INSERT INTO DietPlanMeals (diet_id, meal_id) VALUES (@dietId, @mealId)";

                            using (SqlCommand command = new SqlCommand(insertQuery, connection))
                            {
                                command.Parameters.AddWithValue("@dietId", currentPlanId);
                                command.Parameters.AddWithValue("@mealId", mealId);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"Error adding meal to diet plan: {ex.Message}");
                    }
                }
            }

            MessageBox.Show("Meal(s) added to the diet plan successfully.");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedAllergen = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedAllergen))
            {
                return;
            }

            string query = "SELECT meal_id, meal_name FROM Meals WHERE allergens NOT LIKE @selectedAllergen";
            string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@selectedAllergen", "%" + selectedAllergen + "%");
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        checkedListBox1.Items.Clear();

                        while (reader.Read())
                        {
                            int mealId = reader.GetInt32(0);
                            string mealName = reader.GetString(1);
                            MealItem meal = new MealItem(mealId, mealName);
                            checkedListBox1.Items.Add(meal);
                        }
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Member_outlook obj = new Member_outlook(this.userId);
            obj.Show();
            this.Hide();
        }
    }
}
