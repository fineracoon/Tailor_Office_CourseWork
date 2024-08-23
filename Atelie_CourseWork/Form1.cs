using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Atelie_CourseWork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {      
            if (textBox1.Text.Trim() != "" && textBox2.Text.Trim() != "")
            {
                SqlConnection sqlConnection;
                Program.connection = @"Data Source=DESKTOP-05RLPUM\SQLEXPRESS;Initial Catalog=Ателье;User Id=" + textBox1.Text + ";Password=" + textBox2.Text + ";"; // Integrated Security=True";
                try
                {
                    sqlConnection = new SqlConnection(Program.connection);
                    sqlConnection.Open();
                    Form2 newForm = new Form2();
                    newForm.Show();
                    this.Hide();
                    sqlConnection.Close();
                }
                catch
                {
                    MessageBox.Show("Неверное имя пользователя или пароль. Повторите ввод снова.", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Введены не все данные!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
