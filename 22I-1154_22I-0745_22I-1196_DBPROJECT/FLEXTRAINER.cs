﻿using db_project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBPROJECT
{
    public partial class FLEXTRAINER : Form
    {
        public FLEXTRAINER()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Member_login obj = new Member_login();
            this.Hide();
            obj.Show();
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            trainer_login obj = new trainer_login();
            obj.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           owner_login obj = new owner_login();
            obj.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Admin_login obj = new Admin_login();
            obj.Show();
            this.Hide();
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            reports obj = new reports();
            obj.Show();
            this.Hide();
        }

        private void FLEXTRAINER_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
