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

namespace Atelie_CourseWork
{
    public partial class AtelieUslugi : Form
    {
        SqlConnection sqlConnection;
        public AtelieUslugi()
        {
            InitializeComponent();
        }

        private void AtelieUslugi_Load(object sender, EventArgs e)
        {
            Form2 mainForm = this.Owner as Form2;
            int flag = mainForm.GetFlag();
            if (flag == 28 || flag == 18)
            {
                label1.Text = "Название услуги";
                if (flag == 28)
                {
                    int i = mainForm.GetIndex();
                    string init = "SELECT Название FROM Услуги WHERE Код = " + i;
                    sqlConnection = new SqlConnection(Program.connection);
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(init, sqlConnection);
                    sqlConnection.Close();

                    textBox1.Text = command.ExecuteScalar().ToString();
                }
            }
            else if (flag == 19 || flag == 29)
            {
                label1.Text = "Название изделия";
                if (flag == 29)
                {
                    int i = mainForm.GetIndex();
                    string init = "SELECT Название FROM Изделие WHERE Код = " + i;
                    sqlConnection = new SqlConnection(Program.connection);
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(init, sqlConnection);
                    sqlConnection.Close();

                    textBox1.Text = command.ExecuteScalar().ToString();
                }
                
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 mainForm = this.Owner as Form2;
            int flag = mainForm.GetFlag();
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand();

                string name = textBox1.Text;
                string insert = "", update = "", mess = "";
                
                switch (flag)
                {
                    case 17:
                        insert = "INSERT INTO Услуги (Название) VALUES(\'" + name + "\')";
                        mess = "Новая услуга успешно добавлена в список услуг ателье!";
                        break;
                    case 19:
                        insert = "INSERT INTO Изделие (Название) VALUES(\'" + name + "\')";
                        mess = "Новое изделие успешно сохранено!";
                        break;
                    case 27:
                        int i = mainForm.GetIndex();
                        update = "UPDATE Услуги SET Название = \'" + name + "\' WHERE Код = " + i;
                        mess = "Название услуги успешно обновлено";
                        break;
                    case 29:
                        int j = mainForm.GetIndex();
                        update = "UPDATE Изделие SET Название = \'" + name + "\' WHERE Код = " + j;
                        mess = "Название изделия успешно обновлено";
                        break;
                }
                if (flag < 20)
                {
                    command = new SqlCommand(insert, sqlConnection);
                }
                else
                {
                    command = new SqlCommand(update, sqlConnection);
                }
                
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show(mess);
                    mainForm.Enabled = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
