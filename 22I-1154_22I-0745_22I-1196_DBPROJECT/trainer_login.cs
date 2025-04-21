using DBPROJECT;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace db_project
{
    public partial class trainer_login : Form
    {
        private string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

        public trainer_login()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            trainer_signup obj = new trainer_signup();
            obj.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string trainer_id = textBox1.Text;
            string password = textBox2.Text;

            if (string.IsNullOrWhiteSpace(trainer_id) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both trainer ID and password.");
                return;
            }

            string query = "SELECT T.trainer_id FROM Trainers T INNER JOIN Users U ON T.user_id = U.user_id WHERE T.trainer_id = @trainer_id AND U.password = @Password AND T.status='active';";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@trainer_id", trainer_id);
                        command.Parameters.AddWithValue("@Password", password);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            int trainerID = Convert.ToInt32(result);
                            KHULJAA khuljaaForm = new KHULJAA(trainerID);
                            khuljaaForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid trainer ID or password.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void trainer_login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FLEXTRAINER obj = new FLEXTRAINER();
            obj.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
