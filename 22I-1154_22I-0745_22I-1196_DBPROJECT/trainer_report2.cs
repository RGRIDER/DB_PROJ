using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace db_project
{
    public partial class trainer_report2 : Form
    {
        int ownerid;

        public trainer_report2(int ownerid)
        {
            InitializeComponent();
            this.ownerid = ownerid;
        }

        private void Form13_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {


        }

        private void button2_Click(object sender, EventArgs e)
        {
            int trainerId;
            if (!int.TryParse(textBox8.Text, out trainerId))
            {
                MessageBox.Show("Please enter a valid member ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string query = @"SELECT t.rating, t.specialty, t.experience,t.rating_count, t.gym_id,t.status
                 FROM Trainers t
                 WHERE t.trainer_id = @TrainerID;";

            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TrainerID", trainerId);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        listBox1.Items.Clear();
                        if (reader.Read())
                        {
                            textBox5.Text = reader["rating"].ToString(); 
                            textBox6.Text = reader["specialty"].ToString();
                            textBox4.Text = reader["experience"].ToString(); 
                            listBox1.Items.Add($"Rating count: {reader["rating_count"]}"); 
                            listBox1.Items.Add($"gym_id: {reader["gym_id"]}"); 
                            listBox1.Items.Add($"status: {reader["status"]}");
                        }
                        else
                        {
                            MessageBox.Show("The entered ID does not correspond to a valid trainer.", "Trainer Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error executing query: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            trainer_report1 obj = new trainer_report1(this.ownerid);
            obj.Show();
            this.Hide();
        }

         
        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
