using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace db_project
{
    public partial class member_signup : Form
    {
        public member_signup()
        {
            InitializeComponent();
            LoadGymIds();
        }

        private void LoadGymIds()
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT gym_id FROM Gyms";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int gymId = reader.GetInt32(0);
                                comboBox1.Items.Add(gymId);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
                int newMemberId;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string insertUserQuery = "INSERT INTO Users (username, password, first_name, last_name, email, phone, role) " +
                                            "VALUES (@Username, @Password, @FirstName, @LastName, @Email, @Phone, 'member'); " +
                                            "SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(insertUserQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", textBox7.Text);
                        cmd.Parameters.AddWithValue("@Password", textBox4.Text);
                        cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
                        cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Email", textBox3.Text);
                        cmd.Parameters.AddWithValue("@Phone", textBox8.Text);

                        newMemberId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    string insertMembershipQuery = @"
                        INSERT INTO Memberships (member_id, gym_id, membership_cost, weightt, Height, membership_type, start_date, end_date)
                        VALUES (@MemberId, @GymId, @MembershipCost, @Weight, @Height, @MembershipType, @StartDate, @EndDate);";

                    using (SqlCommand membershipCmd = new SqlCommand(insertMembershipQuery, conn))
                    {
                        membershipCmd.Parameters.AddWithValue("@MemberId", newMemberId);
                        membershipCmd.Parameters.AddWithValue("@GymId", comboBox1.SelectedItem);
                        membershipCmd.Parameters.AddWithValue("@Weight", float.Parse(textBox5.Text));
                        membershipCmd.Parameters.AddWithValue("@Height", float.Parse(textBox6.Text));

                        string membershipType = comboBox2.SelectedItem.ToString();
                        membershipCmd.Parameters.AddWithValue("@MembershipType", membershipType);

                        if (membershipType == "monthly")
                        {
                            membershipCmd.Parameters.AddWithValue("@MembershipCost", 100.00f);
                            membershipCmd.Parameters.AddWithValue("@EndDate", DateTime.Now.Date.AddMonths(1));
                        }
                        else if (membershipType == "yearly")
                        {
                            membershipCmd.Parameters.AddWithValue("@MembershipCost", 1000.00f);
                            membershipCmd.Parameters.AddWithValue("@EndDate", DateTime.Now.Date.AddYears(1));
                        }

                        membershipCmd.Parameters.AddWithValue("@StartDate", DateTime.Now.Date);

                        membershipCmd.ExecuteNonQuery();
                    }
                }

                Member_outlook obj = new Member_outlook(newMemberId);
                obj.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Member_login obj = new Member_login();
                obj.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Member_login obj = new Member_login();
                obj.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
