using db_project;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace _22I_1154_22I_0745_22I_1196_DBPROJECT
{
    public partial class newtrainer : Form
    {
        private string connectionString = "Data Source=DESKTOP-1JL4SSV\\SQLEXPRESS;Initial Catalog=DBPROJ;Integrated Security=True";
        private int loggedInOwnerId;

        public newtrainer(int ownerId)
        {
            InitializeComponent();
            loggedInOwnerId = ownerId;
        }

        private void newtrainer_Load(object sender, EventArgs e)
        {
            LoadTrainers();
        }

        private void LoadTrainers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT T.trainer_id, U.username, U.first_name, U.last_name " +
                                   "FROM Trainers T " +
                                   "INNER JOIN Users U ON T.user_id = U.user_id " +
                                   "INNER JOIN Gyms G ON T.gym_id = G.gym_id " +
                                   "WHERE T.status = 'inactive' AND G.owner_id = @OwnerId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@OwnerId", loggedInOwnerId);

                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable trainersTable = new DataTable();
                    adapter.Fill(trainersTable);

                    dataGridView1.DataSource = trainersTable;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.RowTemplate.Height = 30;

                    var cellStyle = new DataGridViewCellStyle
                    {
                        Font = new Font("Arial", 7),
                        WrapMode = DataGridViewTriState.True,
                        Alignment = DataGridViewContentAlignment.MiddleLeft
                    };
                    dataGridView1.DefaultCellStyle = cellStyle;
                    dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading trainers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int trainerId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["trainer_id"].Value);

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "UPDATE Trainers SET status = 'active' WHERE trainer_id = @TrainerId;" +
                                       "UPDATE Users SET status = 'active' WHERE user_id = " +
                                       "(SELECT user_id FROM Trainers WHERE trainer_id = @TrainerId)";

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@TrainerId", trainerId);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Trainer added successfully.");
                            LoadTrainers();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a trainer.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding trainer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int trainerId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["trainer_id"].Value);

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "UPDATE Trainers SET status = 'banned' WHERE trainer_id = @TrainerId";

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@TrainerId", trainerId);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Trainer banned successfully.");
                            LoadTrainers();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a trainer.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error banning trainer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            gymowner_outlook obj = new gymowner_outlook(this.loggedInOwnerId);
            this.Hide();
            obj.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
