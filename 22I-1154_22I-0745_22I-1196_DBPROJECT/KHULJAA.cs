using DBPROJECT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trainer;

namespace db_project
{
    public partial class KHULJAA : Form
    {

        int T_ID;
        public KHULJAA(int iD)
        {
            InitializeComponent();
            T_ID = iD;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                APPOINMENT aPPOINMENT = new APPOINMENT(T_ID);
                aPPOINMENT.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                W_CREATION w_CREATION = new W_CREATION(T_ID);
                w_CREATION.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                D_CREATION d_CREATION = new D_CREATION(T_ID);
                d_CREATION.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                FEEDBACK fEEDBACK = new FEEDBACK(T_ID);
                fEEDBACK.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                W_REPORT w_REPORT = new W_REPORT(T_ID);
                w_REPORT.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                D_REPORT d_REPORT = new D_REPORT(T_ID);
                d_REPORT.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void KHULJAA_Load(object sender, EventArgs e)
        {
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                trainer_login trainer_Login = new trainer_login();
                trainer_Login.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
