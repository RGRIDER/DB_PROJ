using DBPROJECT;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace db_project
{
    public partial class reports : Form
    {
        private string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

        public reports()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FLEXTRAINER fLEXTRAINER = new FLEXTRAINER();
            this.Hide();
            fLEXTRAINER.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int queryNumber;
            if (int.TryParse(textBox1.Text, out queryNumber) && queryNumber >= 1 && queryNumber <= 20)
            {
                ShowQueryResult(queryNumber);
            }
            else
            {
              //  MessageBox.Show("Invalid Number, Please input number between 1 - 20");
                dataGridView1.DataSource = null;
            }
        }

        private void ShowQueryResult(int queryNumber)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = string.Empty;

                // Define queries based on the input number
                switch (queryNumber)
                {
                    case 1:
                        // Report 1: Details of members of one specific gym that get training from one specific trainer.
                        // Hardcoded parameters: Gym ID = 1, Trainer ID = 3
                        query = @"
                    SELECT u.first_name, u.last_name, u.email, m.gym_id, t.trainer_id
                    FROM Users u
                    JOIN Memberships m ON u.user_id = m.member_id
                    JOIN Trainers t ON m.gym_id = t.gym_id
                    WHERE m.gym_id = 1 AND t.trainer_id = 1;
                ";
                        break;

                    case 2:
                        // Report 2: Details of members from one specific gym that follow a specific diet plan.
                        // Hardcoded parameters: Gym ID = 1, Diet Plan ID = 2
                        query = @"
                    SELECT u.first_name, u.last_name, u.email, dp.title AS diet_plan_title
                    FROM Users u
                    JOIN DietPlanUsage dpu ON u.user_id = dpu.member_id
                    JOIN DietPlans dp ON dpu.diet_id = dp.diet_id
                    JOIN Memberships m ON u.user_id = m.member_id
                    WHERE m.gym_id = 2 AND dpu.diet_id = 2;
                ";
                        break;

                    case 3:
                        // Report 3: Details of members across all gyms of a specific trainer that follow a specific diet plan.
                        // Hardcoded parameters: Trainer ID = 2, Diet Plan ID = 3
                        query = @"
                    SELECT u.first_name, u.last_name, u.email, dp.title AS diet_plan_title
                    FROM Users u
                    JOIN DietPlanUsage dpu ON u.user_id = dpu.member_id
                    JOIN DietPlans dp ON dpu.diet_id = dp.diet_id
                    JOIN Trainers t ON dpu.creator_id = t.trainer_id
                    WHERE t.trainer_id = 1 AND dpu.diet_id = 11;
                ";
                        break;

                    case 4:
                        // Report 4: Count of members who will be using a specific machine on a given day in a specific gym.
                        // Hardcoded parameters: Gym ID = 1, Date = today, Machine Name = 'Leg Press'
                        query = @"
                   SELECT COUNT(DISTINCT m.member_id) AS member_count
                   FROM Memberships m
                   JOIN MemberPlanUsage mp ON m.member_id = mp.member_id
                   JOIN WorkoutPlans wp ON mp.plan_id = wp.plan_id
                   JOIN WorkoutExercises we ON wp.plan_id = we.workout_plan_id
                   JOIN Exercises e ON we.exercise_id = e.exercise_id
                   JOIN Machines mach ON e.machine_id = mach.machine_id
                   WHERE m.gym_id = 1
                       AND wp.plan_date = CONVERT(DATE, GETDATE())  
                       AND mach.machine_name = 'Leg Press'; 

                ";

                        break;

                    case 5:
                        // Report 5: List of Diet plans that have less than 500 calorie meals as breakfast.
                        query = @"
                    SELECT dp.title AS diet_plan_title
                    FROM DietPlans dp
                    JOIN DietPlanMeals dpm ON dp.diet_id = dpm.diet_id
                    JOIN Meals m ON dpm.meal_id = m.meal_id
                    WHERE m.calories < 500;
                ";
                        break;

                    case 6:
                        // Report 6: List of diet plans in which total carbohydrate intake is less than 300 grams.
                        query = @"
                    SELECT dp.title AS diet_plan_title
                    FROM DietPlans dp
                    JOIN DietPlanMeals dpm ON dp.diet_id = dpm.diet_id
                    JOIN Meals m ON dpm.meal_id = m.meal_id
                    GROUP BY dp.diet_id, dp.title
                    HAVING SUM(m.carbs) < 300;
                ";
                        break;

                    case 7:
                        // Report 7: List of workout plans that don’t require using a specific machine.
                        query = @"
                    SELECT wp.title AS workout_plan_title
                    FROM WorkoutPlans wp
                    LEFT JOIN WorkoutExercises we ON wp.plan_id = we.workout_plan_id
                    LEFT JOIN Exercises e ON we.exercise_id = e.exercise_id
                    WHERE e.machine_id IS NULL;
                ";
                        break;

                    case 8:
                        // Report 8: List of diet plans which don’t have peanuts as allergens.
                        query = @"
                    SELECT dp.title AS diet_plan_title
                    FROM DietPlans dp
                    JOIN DietPlanMeals dpm ON dp.diet_id = dpm.diet_id
                    JOIN Meals m ON dpm.meal_id = m.meal_id
                    WHERE NOT EXISTS (
                        SELECT 1 FROM Meals WHERE allergens LIKE '%peanuts%'
                    );
                ";
                        break;

                    case 9:
                        // Report 9: New membership data in the last 3 months (Gym Owner).
                        query = @"
                    SELECT COUNT(*) AS new_memberships, m.gym_id, g.gym_name
                    FROM Memberships m
                    JOIN Gyms g ON m.gym_id = g.gym_id
                    WHERE m.start_date >= DATEADD(MONTH, -3, GETDATE())
                    GROUP BY m.gym_id, g.gym_name;
                ";
                        break;

                    case 10:
                        // Report 10: Comparison of total members in multiple gyms, in the past 6 months.
                        query = @"
                    SELECT g.gym_name, COUNT(m.member_id) AS total_members
                    FROM Memberships m
                    JOIN Gyms g ON m.gym_id = g.gym_id
                    WHERE m.start_date >= DATEADD(MONTH, -6, GETDATE())
                    GROUP BY g.gym_name;
                ";
                        break;

                    case 11:
                        // Report 11: Details of trainers across multiple gyms with their average rating.
                        query = @"
                    SELECT t.trainer_id, u.first_name, u.last_name, g.gym_name, AVG(tf.rating) AS average_rating
                    FROM Trainers t
                    JOIN Users u ON t.user_id = u.user_id
                    JOIN Gyms g ON t.gym_id = g.gym_id
                    JOIN TrainerFeedback tf ON t.trainer_id = tf.trainer_id
                    GROUP BY t.trainer_id, u.first_name, u.last_name, g.gym_name;
                ";
                        break;

                    case 12:
                        // Report 12: List of members with active gym memberships and their total gym visits in the last month.
                        query = @"
                    SELECT u.first_name, u.last_name, m.gym_id, COUNT(a.member_id) AS total_visits
                    FROM Users u
                    JOIN Memberships m ON u.user_id = m.member_id
                    LEFT JOIN Appointments a ON m.member_id = a.member_id
                    WHERE m.status = 'active' AND a.appointment_date >= DATEADD(MONTH, -1, GETDATE())
                    GROUP BY u.first_name, u.last_name, m.gym_id;
                ";
                        break;

                    case 13:
                        // Report 13: List of active diet plans and their number of followers.
                        query = @"
                    SELECT dp.title AS diet_plan_title, COUNT(dpu.member_id) AS num_followers
                    FROM DietPlans dp
                    JOIN DietPlanUsage dpu ON dp.diet_id = dpu.diet_id
                    WHERE dp.status = 'active'
                    GROUP BY dp.diet_id, dp.title;
                ";
                        break;

                    case 14:
                        // Report 14: List of workout plans and the total number of exercises in each plan.
                        query = @"
                    SELECT wp.title AS workout_plan_title, COUNT(we.exercise_id) AS total_exercises
                    FROM WorkoutPlans wp
                    JOIN WorkoutExercises we ON wp.plan_id = we.workout_plan_id
                    GROUP BY wp.title;
                ";
                        break;

                    case 15:
                        //Report 15: Count of each type of machine across all gyms.
                        query = @"
                        SELECT mach.type, COUNT(mach.machine_id) AS total_machines
                        FROM Machines mach
                        GROUP BY mach.type;
                ";

                        break;

                    case 16:
                        // Report 16: Total revenue generated from each gym in the past year.
                        query = @"
                    SELECT g.gym_name, SUM(m.membership_cost) AS total_revenue
                    FROM Memberships m
                    JOIN Gyms g ON m.gym_id = g.gym_id
                    WHERE m.start_date >= DATEADD(YEAR, -1, GETDATE())
                    GROUP BY g.gym_name;
                ";
                        break;

                    case 17:
                        // Report 17: List of trainers with at least 5 years of experience.
                        query = @"
                    SELECT u.first_name, u.last_name, t.experience
                    FROM Trainers t
                    JOIN Users u ON t.user_id = u.user_id
                    WHERE t.experience >= 5;
                ";
                        break;

                    case 18:
                        // Report 18: List of members who have not visited the gym in the past month.
                        query = @"
                    SELECT u.first_name, u.last_name
                    FROM Users u
                    JOIN Memberships m ON u.user_id = m.member_id
                    LEFT JOIN Appointments a ON m.member_id = a.member_id
                    WHERE a.appointment_date IS NULL OR a.appointment_date < DATEADD(MONTH, -1, GETDATE());
                ";
                        break;

                    case 19:
                        // Report 19: List of members with ongoing membership extensions.
                        query = @"
                    SELECT u.first_name, u.last_name, m.start_date, m.end_date
                    FROM Users u
                    JOIN Memberships m ON u.user_id = m.member_id
                    WHERE m.start_date <= m.end_date;
                "
                        ;
                        break;

                    case 20:
                        //Report 20: Details of the most popular workout plan in each gym
                        query = @"
                    SELECT g.gym_name, wp.title AS workout_plan_title, COUNT(DISTINCT mp.member_id) AS num_followers
                    FROM Memberships m
                    JOIN Gyms g ON m.gym_id = g.gym_id
                    JOIN MemberPlanUsage mp ON m.member_id = mp.member_id
                    JOIN WorkoutPlans wp ON mp.plan_id = wp.plan_id
                    GROUP BY g.gym_name, wp.title
                    ORDER BY num_followers DESC;
                ";
                        break; 



                    default:
                        dataGridView1.DataSource = null;
                        MessageBox.Show("Invalid query number. Please enter a number between 1 and 10.");
                        return;
                }

                // Execute the query and bind the results to dataGridView1
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;

                // Adjust DataGridView settings to fit the data
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.RowTemplate.Height = 30; // Adjust row height as needed

                // Apply cell style adjustments
                var cellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Arial", 7), // Adjust font size and style
                    WrapMode = DataGridViewTriState.True, // Enable text wrapping
                    Alignment = DataGridViewContentAlignment.MiddleLeft // Adjust alignment
                };
                dataGridView1.DefaultCellStyle = cellStyle;

                // Resize columns based on the data
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void reports_Load(object sender, EventArgs e)
        {

        }
    }
}
