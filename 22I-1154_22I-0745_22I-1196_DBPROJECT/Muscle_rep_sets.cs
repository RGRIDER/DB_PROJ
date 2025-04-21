using db_project;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBPROJECT
{
    public partial class Muscle_rep_sets : Form
    {
        private int userId;
        int currentPlanId;

        public Muscle_rep_sets(int userid)
        {
            InitializeComponent();
            this.userId = userid;
            this.currentPlanId = -1;

            LoadExercises();
            LoadMuscles();
        }

        private void LoadExercises()
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT exercise_id, exercise_name, description FROM Exercises";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int exerciseId = reader.GetInt32(0);
                                string exerciseName = reader.GetString(1);

                                checkedListBox1.Items.Add(new { ExerciseId = exerciseId, ExerciseName = exerciseName });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMuscles()
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT DISTINCT target_muscle FROM Exercises";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["target_muscle"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

                DateTime planDate = dateTimePicker1.Value.Date;
                int creatorId = this.userId;
                string title = textBox5.Text.Trim();
                string goal = comboBox2.SelectedItem?.ToString();

                if (string.IsNullOrWhiteSpace(title))
                {
                    MessageBox.Show("Please enter a title for the workout plan.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(goal))
                {
                    MessageBox.Show("Please enter a goal for the workout plan.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertPlanQuery = @"
                        INSERT INTO WorkoutPlans (creator_id, title, goal, plan_date, status)
                        VALUES (@creatorId, @title, @goal, @planDate, @status);
                        SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(insertPlanQuery, connection))
                    {
                        command.Parameters.AddWithValue("@creatorId", creatorId);
                        command.Parameters.AddWithValue("@title", title);
                        command.Parameters.AddWithValue("@goal", goal);
                        command.Parameters.AddWithValue("@planDate", planDate);
                        command.Parameters.AddWithValue("@status", "active");
                        currentPlanId = Convert.ToInt32(command.ExecuteScalar());
                        MessageBox.Show("Workout plan created successfully.");
                    }
                }

                AddToMemberPlanUsage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddToMemberPlanUsage()
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string insertQuery = @"
                        INSERT INTO MemberPlanUsage (member_id, plan_id, creator_id)
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
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int sets = int.Parse(textBox2.Text);
                int reps = int.Parse(textBox1.Text);
                int restInterval = int.Parse(textBox3.Text);

                if (checkedListBox1.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Please select at least one exercise.");
                    return;
                }

                if (currentPlanId == -1)
                {
                    currentPlanId = GetCurrentWorkoutPlanId();
                }

                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var selectedItem = (dynamic)checkedListBox1.CheckedItems[0];
                    int exerciseId = selectedItem.ExerciseId;

                    string insertQuery = @"
                        INSERT INTO WorkoutExercises (workout_plan_id, exercise_id, sets, reps, rest_interval)
                        VALUES (@workoutPlanId, @exerciseId, @sets, @reps, @restInterval)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@workoutPlanId", currentPlanId);
                        command.Parameters.AddWithValue("@exerciseId", exerciseId);
                        command.Parameters.AddWithValue("@sets", sets);
                        command.Parameters.AddWithValue("@reps", reps);
                        command.Parameters.AddWithValue("@restInterval", restInterval);

                        command.ExecuteNonQuery();
                    }

                    checkedListBox1.SetItemChecked(checkedListBox1.SelectedIndex, false);
                }

                MessageBox.Show("Exercise added to the workout plan successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetCurrentWorkoutPlanId()
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
                int currentPlanId = -1;
                string countQuery = "SELECT COUNT(*) FROM WorkoutPlans";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand countCommand = new SqlCommand(countQuery, connection))
                    {
                        connection.Open();
                        int existingPlanCount = (int)countCommand.ExecuteScalar();
                        currentPlanId = existingPlanCount;
                    }
                }
                return currentPlanId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedMuscle = comboBox1.SelectedItem.ToString();

                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
                string query = "SELECT exercise_id, exercise_name FROM Exercises WHERE target_muscle = @target_muscle";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@target_muscle", selectedMuscle);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        checkedListBox1.Items.Clear();

                        while (reader.Read())
                        {
                            int exerciseId = reader.GetInt32(0);
                            string exerciseName = reader.GetString(1);
                            checkedListBox1.Items.Add(new { ExerciseId = exerciseId, ExerciseName = exerciseName });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Member_outlook obj = new Member_outlook(this.userId);
            obj.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void Muscle_rep_sets_Load(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
