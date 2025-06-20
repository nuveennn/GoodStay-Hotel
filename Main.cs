using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace GoodStayHMS
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            button1.Click += new EventHandler(this.button1_Click);
            button2.Click += new EventHandler(this.button2_Click);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=myDB;Integrated Security=True;";
            string username = textBox1.Text;
            string password = textBox2.Text;
            string role = comboBox1.SelectedItem.ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Users WHERE Username = @username AND Password = @password AND Role = @role";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@role", role);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();

                        // Debugging statement to check the role value
                        Console.WriteLine("Logged in as role: " + role);

                        try
                        {
                            switch (role)
                            {
                                case "Receptionist":
                                    ReceptionistDashboard receptionistDashboard = new ReceptionistDashboard(username);
                                    receptionistDashboard.Show();
                                    Console.WriteLine("ReceptionistDashboard shown successfully.");
                                    break;
                              
                                default:
                                    MessageBox.Show("Invalid role selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Show();
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error showing dashboard: " + ex.Message);
                            MessageBox.Show("An error occurred while opening the dashboard.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username, password, or role.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connecting to the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
