using db_project;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DBPROJECT
{
    public partial class Owner_signup : Form
    {
        public Owner_signup()
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
            owner_login obj = new owner_login();
            obj.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Connection string
                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
                int adminUserId;

                // Open the connection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Retrieve the user ID of the admin
                    string adminUsername = textBox9.Text; // Assuming the admin username is provided in textBox9
                    string getAdminUserIdQuery = "SELECT user_id FROM Users WHERE username = @AdminUsername;";

                    using (SqlCommand getUserIdCmd = new SqlCommand(getAdminUserIdQuery, conn))
                    {
                        getUserIdCmd.Parameters.AddWithValue("@AdminUsername", adminUsername);
                        adminUserId = Convert.ToInt32(getUserIdCmd.ExecuteScalar());
                    }

                    // Create SqlCommand for inserting a new user (gym owner)
                    string insertUserQuery = "INSERT INTO Users (username, password, first_name, last_name, email, phone, role) " +
                                             "VALUES (@Username, @Password, @FirstName, @LastName, @Email, @Phone, 'gym_owner'); " +
                                             "SELECT SCOPE_IDENTITY();"; // This retrieves the last inserted user_id

                    using (SqlCommand cmd = new SqlCommand(insertUserQuery, conn))
                    {
                        // Add parameters for the new user (gym owner)
                        cmd.Parameters.AddWithValue("@Username", textBox4.Text);
                        cmd.Parameters.AddWithValue("@Password", textBox6.Text);
                        cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
                        cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Email", textBox5.Text);
                        cmd.Parameters.AddWithValue("@Phone", textBox7.Text);

                        // Execute the command and retrieve the new user's ID (gym owner)
                        int newGymOwnerId = Convert.ToInt32(cmd.ExecuteScalar());

                        // Insert a new gym record into the Gyms table
                        string insertGymQuery = "INSERT INTO Gyms (owner_id, gym_name, address, status) " +
                                                "VALUES (@OwnerId, @GymName, @Address, 'active');";

                        using (SqlCommand gymCmd = new SqlCommand(insertGymQuery, conn))
                        {
                            // Add parameters for the new gym
                            gymCmd.Parameters.AddWithValue("@OwnerId", adminUserId); // Assign admin user ID as gym owner ID
                            gymCmd.Parameters.AddWithValue("@GymName", textBox3.Text);
                            gymCmd.Parameters.AddWithValue("@Address", textBox8.Text);

                            // Execute the command
                            gymCmd.ExecuteNonQuery();
                        }
                    }

                    // Close the connection
                    conn.Close();
                }

                gymowner_outlook obj = new gymowner_outlook(adminUserId);
                obj.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //gym name
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void Owner_signup_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            owner_login obj = new owner_login();
            obj.Show();
            this.Hide();
        }
    }
}
