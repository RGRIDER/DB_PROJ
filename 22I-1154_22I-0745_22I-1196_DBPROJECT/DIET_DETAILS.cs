using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Trainer
{
    public partial class DIET_DETAILS : Form
    {
        int D_ID;
        int TID;
        string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

        public DIET_DETAILS(int d_ID, int T_id)
        {
            InitializeComponent();
            D_ID = d_ID;
            TID = T_id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) &&
                    !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrEmpty(textBox4.Text) &&
                    !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrEmpty(textBox6.Text) &&
                    !string.IsNullOrEmpty(textBox7.Text))
                {
                    string mealName = textBox1.Text;
                    int calories = int.Parse(textBox2.Text);
                    float protein = float.Parse(textBox3.Text);
                    float carbs = float.Parse(textBox4.Text);
                    string allergens = textBox5.Text;
                    float fats = float.Parse(textBox6.Text);
                    float fiber = float.Parse(textBox7.Text);

                    int dietID = D_ID;

                    InsertMeal(dietID, mealName, calories, protein, carbs, allergens, fats, fiber);

                    MessageBox.Show("Meal added successfully.");
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

        private void InsertMeal(int dietID, string mealName, int calories, float protein, float carbs, string allergens, float fats, float fiber)
        {
            string query = $"INSERT INTO Meals (diet_id, meal_name, calories, protein, carbs, allergens, fat, fiber) " +
                           $"VALUES (@DietID, @MealName, @Calories, @Protein, @Carbs, @Allergens, @Fats, @Fiber);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DietID", dietID);
                    command.Parameters.AddWithValue("@MealName", mealName);
                    command.Parameters.AddWithValue("@Calories", calories);
                    command.Parameters.AddWithValue("@Protein", protein);
                    command.Parameters.AddWithValue("@Carbs", carbs);
                    command.Parameters.AddWithValue("@Allergens", allergens);
                    command.Parameters.AddWithValue("@Fats", fats);
                    command.Parameters.AddWithValue("@Fiber", fiber);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                D_CREATION obj = new D_CREATION(this.TID);
                this.Hide();
                obj.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void DIET_DETAILS_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
