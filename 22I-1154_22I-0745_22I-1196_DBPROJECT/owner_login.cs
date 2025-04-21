using db_project;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DBPROJECT
{
    public partial class owner_login : Form
    {
        public owner_login()
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
            Owner_signup obj = new Owner_signup();
            obj.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
            string username = textBox1.Text;
            string password = textBox2.Text;

            string query = "SELECT user_id FROM Users WHERE username = @username AND password = @password AND role = 'gym_owner';";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            int ownerId = (int)result;
                            gymowner_outlook obj = new gymowner_outlook(ownerId);
                            obj.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("No such gym owner exists or incorrect username/password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error logging in: " + ex.Message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void owner_login_Load(object sender, EventArgs e)
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
    }
}
