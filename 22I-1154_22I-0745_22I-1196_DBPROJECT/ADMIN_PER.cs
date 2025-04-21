using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace db_project
{
    public partial class ADMIN_PER : Form
    {
        public ADMIN_PER()
        {
            InitializeComponent();
            InitializePhraseQueue();

            listBox1.Items.Clear();

            string query = "SELECT gym_name FROM Gyms;";

            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True"))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string gymName = reader.GetString(0);
                        listBox1.Items.Add(gymName);
                    }

                    if (listBox1.Items.Count == 0)
                    {
                        MessageBox.Show("No gyms found.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading gyms: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string[] phrases = { "good service", "good management", "excellent facilities", "friendly staff", "clean environment" };
        private Queue<string> phraseQueue = new Queue<string>();

        private void InitializePhraseQueue()
        {
            phrases = phrases.OrderBy(x => Guid.NewGuid()).ToArray();

            foreach (string phrase in phrases)
            {
                phraseQueue.Enqueue(phrase);
            }
        }

        private string generatestring()
        {
            if (phraseQueue.Count == 0)
            {
                InitializePhraseQueue();
            }

            return phraseQueue.Dequeue();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
             
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string customerSatisfaction = generatestring();
                string membershipGrowth = generatestring();
                string classAttendance = generatestring();
                string performance = generatestring();

                textBox1.Text = customerSatisfaction;
                textBox2.Text = membershipGrowth;
                textBox3.Text = classAttendance;
                textBox4.Text = performance;
            }
            else
            {
                MessageBox.Show("Please select a gym first.", "No Gym Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
             
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
             
        }

        private void ADMIN_PER_Load(object sender, EventArgs e)
        {
             
        }

        private void label8_Click(object sender, EventArgs e)
        {
             
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ADMINKAMAINHAI obj = new ADMINKAMAINHAI();
            obj.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
