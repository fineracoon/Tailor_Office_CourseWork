using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atelie_CourseWork
{
    public partial class AddClient : Form
    {
        SqlConnection sqlConnection;
        Form2 mainForm;
        public AddClient()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string FIO;
                if (textBox3.Text.Trim() != "")
                    FIO = textBox1.Text.Trim() + " " + textBox2.Text.Trim() + " " + textBox3.Text.Trim();
                else
                    FIO = textBox1.Text.Trim() + " " + textBox2.Text.Trim();


                sqlConnection = new SqlConnection(Program.connection);
                SqlCommand command;
                sqlConnection.Open();
                

                string queryCl = "SELECT Клиенты.Код FROM Клиенты WHERE Клиенты.клФИО LIKE \'" + FIO + "\'";
                sqlConnection = new SqlConnection(Program.connection);
                command = new SqlCommand(queryCl, sqlConnection);
                sqlConnection.Open();

                int codClienta = -1;
                bool newOne = false;
                codClienta = Convert.ToInt32(command.ExecuteScalar());

                if (checkBox1.Visible == true)
                {
                    if (codClienta <= 0)
                    {
                        newOne = true;
                    }
                    else
                    {
                        newOne = false;
                    }
                    if (!newOne || checkBox1.Checked)
                    {
                        MessageBox.Show("Клиент с такой фамилией и именем уже есть в базе данных!");
                        sqlConnection.Close();
                        return;
                    }
                }

                string phone = textBox5.Text.Trim();

                Double discont;
                if (Double.TryParse(textBox4.Text, out discont))
                {
                    discont = discont;
                }

                string queryAdd = "", mess = "";

                if (codClienta > -1)
                {
                    switch (mainForm.GetFlag())
                    {
                        case 12:
                            queryAdd = "INSERT Клиенты (клФИО, клТелефон, клСкидка) " +
                        "VALUES(\'" + FIO + "\', \'" + phone + "\', " + String.Format("{0:F2}", discont).Replace(',', '.') + ")";
                            mess = "Новый клиент успешно добавлен!";
                            break;
                        case 22:
                            int i = mainForm.GetIndex();
                            queryAdd = "UPDATE Клиенты SET клФИО = \'" + FIO + "\', " +
                                "клТелефон = \'" + phone + "\', клСкидка = " + String.Format("{0:F2}", discont).Replace(',', '.') + 
                                " WHERE Код = " + i;
                            mess = "Данные клиента успешно изменены!";
                            break;
                    }

                    command = new SqlCommand(queryAdd, sqlConnection);
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show(mess);
                        sqlConnection.Close();
                        this.Close();
                    }
                }
                sqlConnection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox4.Enabled = false;
            else
            {
                textBox4.Enabled = true;
                textBox4.Text = "0.0";
            }
        }

        private void AddClient_Load(object sender, EventArgs e)
        {
            mainForm = this.Owner as Form2;
            if (mainForm != null)
            {
                try
                {
                    switch (mainForm.GetFlag())
                    {
                        case 12:
                            checkBox1.Visible = true;
                            button1.Text = "Добавить";
                            break;
                        case 22:
                            checkBox1.Visible = false;
                            button1.Text = "Сохранить";
                            int i = mainForm.GetIndex();
                            string query = "SELECT Клиенты.клФИО, Клиенты.клТелефон, Клиенты.клСкидка " +
                                "FROM Клиенты WHERE Клиенты.Код = " + i;

                            sqlConnection = new SqlConnection(Program.connection);
                            SqlCommand command = new SqlCommand(query, sqlConnection);
                            sqlConnection.Open();
                            SqlDataReader reader = command.ExecuteReader();

                            string[] FIO;
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    FIO = reader.GetString(0).Split(separator: new char[] { ' ' });
                                    textBox1.Text = FIO[0];
                                    textBox2.Text = FIO[1];
                                    if (FIO.Length > 2)
                                        textBox3.Text = FIO[2];
                                    textBox5.Text = reader.GetString(1);
                                    textBox4.Text = String.Format("{0:F2}", reader.GetDouble(2));
                                }
                            }
                            reader.Close();
                            
                            sqlConnection.Close();
                            break;
                        case 13:
                            Addition mf = this.Owner as Addition;
                            
                            button1.Text = "Добавить";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
