using db_project;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Trainer
{
    public partial class W_CREATION : Form
    {
        int T_ID;
        string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
        bool checker = false;

        public W_CREATION(int iD)
        {
            InitializeComponent();
            T_ID = iD;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (checker)
                {
                    string workoutName = textBox1.Text;
                    int workoutID = GetWorkoutIDFromName(workoutName);

                    E_ASSOCIATION exerciseAssociationForm = new E_ASSOCIATION(workoutName, workoutID,this.T_ID);
                    exerciseAssociationForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("You have to create the Workout First.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private int GetWorkoutIDFromName(string workoutName)
        {
            try
            {
                int workoutID = 0;
                string query = "SELECT plan_id FROM WorkoutPlans WHERE title = @WorkoutName;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@WorkoutName", workoutName);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            workoutID = Convert.ToInt32(result);
                        }
                    }
                }

                return workoutID;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting workout ID: " + ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string workoutName = textBox1.Text;
                string workoutGoal = textBox2.Text;

                int creatorID = GetUserIDFromTrainerID(T_ID);

                string query = @"INSERT INTO WorkoutPlans (creator_id, title, goal) 
                                 VALUES (@CreatorID, @WorkoutName, @WorkoutGoal);";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CreatorID", creatorID);
                        command.Parameters.AddWithValue("@WorkoutName", workoutName);
                        command.Parameters.AddWithValue("@WorkoutGoal", workoutGoal);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Workout plan created successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Failed to create workout plan.");
                        }
                    }
                }
                checker = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private int GetUserIDFromTrainerID(int trainerID)
        {
            try
            {
                int userID = 0;
                string query = "SELECT user_id FROM Trainers WHERE trainer_id = @TrainerID;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TrainerID", trainerID);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            userID = Convert.ToInt32(result);
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

        private void W_CREATION_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                KHULJAA obj = new KHULJAA(this.T_ID);
                this.Hide();
                obj.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
    }
}
