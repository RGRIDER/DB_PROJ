using DBPROJECT;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace db_project
{
    public partial class Admin_login : Form
    {
        public Admin_login()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
                string username = textBox1.Text;
                string password = textBox2.Text;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Username and password are required.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string query = "SELECT COUNT(*) FROM Users WHERE username = @username AND password = @password AND role = 'admin';";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        connection.Open();
                        int adminCount = (int)command.ExecuteScalar();

                        if (adminCount > 0)
                        {
                            ADMINKAMAINHAI obj = new ADMINKAMAINHAI();
                            obj.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("No such admin exists.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
             
        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void Admin_login_Load(object sender, EventArgs e)
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
