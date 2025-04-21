using DBPROJECT;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace db_project
{
    public partial class Member_login : Form
    {
        private readonly string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

        public Member_login()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
             
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
             
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
             
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and password cannot be empty.");
                return;
            }

            (bool loginSuccess, string userRole, int userId, DateTime? endDate, string membershipStatus) = CheckUserCredentials(username, password);

            if (loginSuccess)
            {
                if (userRole == "member")
                {
                    if ((endDate.HasValue && endDate.Value < DateTime.Now) || membershipStatus == "expired" || membershipStatus == "cancelled")
                    {
                        MessageBox.Show("Your membership is inactive. Please renew your membership.");

                        if (endDate.HasValue && endDate.Value < DateTime.Now)
                        {
                            UpdateMembershipStatus(userId);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Login successful");
                        Member_outlook obj = new Member_outlook(userId);
                        this.Hide();
                        obj.Show();
                    }
                }
            }
            else
            {
                MessageBox.Show("Account does not exist or credentials are incorrect");
            }
        }

        private (bool loginSuccess, string userRole, int userId, DateTime? endDate, string membershipStatus) CheckUserCredentials(string username, string password)
        {
            bool loginSuccess = false;
            string userRole = null;
            int userId = 0;
            DateTime? endDate = null;
            string membershipStatus = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT u.user_id, u.role, m.end_date, m.status
                        FROM Users u
                        LEFT JOIN Memberships m ON u.user_id = m.member_id
                        WHERE u.username = @Username AND u.password = @Password";

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = reader.GetInt32(0);
                                userRole = reader.GetString(1);
                                endDate = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2);
                                membershipStatus = reader.IsDBNull(3) ? null : reader.GetString(3);
                                loginSuccess = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while checking credentials: " + ex.Message);
            }

            return (loginSuccess, userRole, userId, endDate, membershipStatus);
        }

        private void UpdateMembershipStatus(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string updateQuery = @"
                        UPDATE Memberships
                        SET status = 'expired'
                        WHERE member_id = @MemberId AND end_date < @CurrentDate";

                    using (SqlCommand command = new SqlCommand(updateQuery, conn))
                    {
                        command.Parameters.AddWithValue("@MemberId", userId);
                        command.Parameters.AddWithValue("@CurrentDate", DateTime.Now.Date);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating membership status: " + ex.Message);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            member_signup obj = new member_signup();
            obj.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FLEXTRAINER obj = new FLEXTRAINER();
            obj.Show();
            this.Hide();
        }

        private void Member_login_Load(object sender, EventArgs e)
        {
             
        }
    }
}
