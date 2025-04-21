using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace db_project
{
    public partial class trainer_report1 : Form
    {
        int ownerid;

        public trainer_report1(int ownerid)
        {
            InitializeComponent();
            this.ownerid = ownerid;
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            trainer_report2 obj = new trainer_report2(this.ownerid);
            obj.Show();
            this.Hide();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();

                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string selectQuery = "SELECT user_id, gym_id, experience, specialty " +
                                         "FROM Trainers " +
                                         "ORDER BY experience DESC;";

                    using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int userId = reader.GetInt32(0);
                                int gymId = reader.GetInt32(1);
                                int experience = reader.GetInt32(2);
                                string specialty = reader.GetString(3);

                                string trainerInfo = $"UserID: {userId}, GymID: {gymId}, Experience: {experience}, Specialty: {specialty}";

                                listBox1.Items.Add(trainerInfo);
                            }
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();

                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string selectQuery = "SELECT trainer_id, user_id, gym_id, specialty, rating" +
                                         " FROM Trainers" +
                                         " ORDER BY rating DESC;";

                    using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int userId = reader.GetInt32(0);
                                int gymId = reader.GetInt32(1);
                                string specialty = reader.GetString(3);
                                double rating = reader.GetDouble(4);

                                string trainerInfo = $"UserID: {userId}, GymID: {gymId}, Specialty: {specialty}, Rating: {rating}";

                                listBox1.Items.Add(trainerInfo);
                            }
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void trainer_report1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            gymowner_outlook obj = new gymowner_outlook(this.ownerid);
            obj.Show();
            this.Hide();
        }

       

        
    }
}
