using DBPROJECT;
using System;
using System.Windows.Forms;

namespace db_project
{
    public partial class Member_outlook : Form
    {
        private int userId;

        public Member_outlook(int userid)
        {
            InitializeComponent();
            this.userId = userid;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
             
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
             
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                WORKOUTPLANREPORT obj = new WORKOUTPLANREPORT(this.userId);
                obj.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                Trainer_feedback obj = new Trainer_feedback(this.userId);
                obj.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Form7_Load(object sender, EventArgs e)
        {
             
        }

        private void button1_Click(object sender, EventArgs e)
        {
             
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                DIETPLAN obj = new DIETPLAN(this.userId);
                obj.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
             
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DIETPLANREPORT obj = new DIETPLANREPORT(this.userId);
                obj.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Muscle_rep_sets obj = new Muscle_rep_sets(this.userId);
                obj.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Personal_training obj = new Personal_training(this.userId);
                obj.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                Member_login obj = new Member_login();
                obj.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
