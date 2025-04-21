using _22I_1154_22I_0745_22I_1196_DBPROJECT;
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

namespace db_project
{
    public partial class gymowner_outlook : Form
    {
        int ownerid;
        public gymowner_outlook(int userid)
        {
            ownerid = userid;
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                member_report obj = new member_report(this.ownerid);
                obj.Show();
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
                trainer_report1 obj = new trainer_report1(this.ownerid);
                obj.Show();
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
                ADDINGNEWTRAINER obj = new ADDINGNEWTRAINER(this.ownerid);
                obj.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                ACCMNGMNT obj = new ACCMNGMNT(this.ownerid);
                obj.Show();
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
                newtrainer obj = new newtrainer(this.ownerid);
                this.Hide();
                obj.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void gymowner_outlook_Load(object sender, EventArgs e)
        {
             
        }

        private void button8_Click(object sender, EventArgs e)
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

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                owner_login obj = new owner_login();
                obj.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
