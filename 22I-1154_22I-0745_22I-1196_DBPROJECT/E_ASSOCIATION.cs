using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Trainer;

namespace db_project
{
    public partial class E_ASSOCIATION : Form
    {
        string W_NAME;
        int W_ID;
        int T_ID;
        string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

        public E_ASSOCIATION(string w_NAME, int w_ID, int Tid)
        {
            InitializeComponent();
            W_NAME = w_NAME;
            W_ID = w_ID;
            T_ID = Tid;
            label2.Text = w_NAME;

            listBox1.Items.Clear();
            listBox1.Items.Add("List of all the exercises");

            label2.Text = w_NAME;
            PopulateListBoxWithExercises();
        }

        private void PopulateListBoxWithExercises()
        {
            string query = "SELECT exercise_name FROM Exercises;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string exerciseName = reader["exercise_name"].ToString();
                                    listBox1.Items.Add(exerciseName);
                                }
                            }
                            else
                            {
                                MessageBox.Show("No exercises found.");
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error populating exercises list: {ex.Message}");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > 0)
            {
                if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox3.Text))
                {
                    string selectedExerciseName = listBox1.SelectedItem.ToString();
                    int exerciseID = GetExerciseIDFromName(selectedExerciseName);
                    int sets = Convert.ToInt32(textBox1.Text);
                    int reps = Convert.ToInt32(textBox2.Text);
                    int intervals = Convert.ToInt32(textBox3.Text);

                    try
                    {
                        InsertWorkoutExercise(W_ID, exerciseID, sets, reps, intervals);
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        MessageBox.Show("Exercise added to workout successfully.");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"Error inserting workout exercise: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter values for sets, reps, and intervals.");
                }
            }
            else
            {
                MessageBox.Show("Please select an exercise from the list.");
            }
        }

        private int GetExerciseIDFromName(string exerciseName)
        {
            int exerciseID = 0;
            string query = "SELECT exercise_id FROM Exercises WHERE exercise_name = @ExerciseName;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ExerciseName", exerciseName);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            exerciseID = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error retrieving exercise ID: {ex.Message}");
            }

            return exerciseID;
        }

        private void InsertWorkoutExercise(int workoutID, int exerciseID, int sets, int reps, int intervals)
        {
            string query = @"INSERT INTO WorkoutExercises (workout_plan_id, exercise_id, sets, reps, rest_interval) 
                     VALUES (@WorkoutID, @ExerciseID, @Sets, @Reps, @Intervals);";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@WorkoutID", workoutID);
                        command.Parameters.AddWithValue("@ExerciseID", exerciseID);
                        command.Parameters.AddWithValue("@Sets", sets);
                        command.Parameters.AddWithValue("@Reps", reps);
                        command.Parameters.AddWithValue("@Intervals", intervals);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("Failed to add exercise to workout.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error inserting workout exercise: {ex.Message}");
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void E_ASSOCIATION_Load(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            W_CREATION obj = new W_CREATION(this.T_ID);
            this.Hide();
            obj.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
