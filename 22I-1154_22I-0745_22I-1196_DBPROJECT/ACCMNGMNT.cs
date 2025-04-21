using db_project;
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



namespace DBPROJECT
{
    public partial class ACCMNGMNT : Form
    {
        int ownerid;

        public ACCMNGMNT(int ownerid)
        {
            InitializeComponent();
            this.ownerid = ownerid;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ACCMNGMNT_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    string selectedMemberInfo = listBox1.SelectedItem.ToString();
                    int userId;
                    string[] infoParts = selectedMemberInfo.Split(',');
                    if (infoParts.Length >= 1)
                    {
                        string userIdString = infoParts[0].Trim().Split(':')[1].Trim(); 
                        if (int.TryParse(userIdString, out userId))
                        {
                            string updateQuery = @"UPDATE Users SET status = 'inactive' WHERE user_id = @UserID;";

                            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True"))
                            {
                                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                                {
                                    command.Parameters.AddWithValue("@UserID", userId);

                                    connection.Open();
                                    int rowsAffected = command.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        MessageBox.Show("Member status updated to 'inactive'.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        button1_Click(sender, e); 
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to update member status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Failed to parse user ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Selected member information is not in the expected format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a member from the list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                string query = @"SELECT user_id, username, email, phone FROM Users WHERE role = 'member' and status='active';";
                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True"))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int userId = reader.GetInt32(0);
                            string username = reader.GetString(1);
                            string email = reader.GetString(2);
                            string phone = reader.GetString(3);

                            string memberInfo = $"UserID: {userId}, Username: {username}, Email: {email}, Phone: {phone}";
                            listBox1.Items.Add(memberInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing query: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                string query = @"SELECT user_id, username, email, phone FROM Users WHERE role = 'trainer' AND status = 'active';";

                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True"))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int userId = reader.GetInt32(0);
                            string username = reader.GetString(1);
                            string email = reader.GetString(2);
                            string phone = reader.GetString(3);

                            string memberInfo = $"UserID: {userId}, Username: {username}, Email: {email}, Phone: {phone}";

                            listBox1.Items.Add(memberInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing query: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
