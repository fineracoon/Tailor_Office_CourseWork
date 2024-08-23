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
    public partial class AddWorker : Form
    {
        public AddWorker()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(Program.connection); ;
            Form2 mainForm = this.Owner as Form2;
            if (mainForm != null)
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

                    DateTime bday = dateTimePicker1.Value;

                    string phone = textBox4.Text.Trim();

                    int stage = Convert.ToInt32(textBox5.Text);
                    int rank = Convert.ToInt32(textBox6.Text);

                    Double discont;
                    if (Double.TryParse(textBox4.Text, out discont))
                    {
                        
                    }

                    string queryAdd = "", mess = "";
                    switch (mainForm.GetFlag())
                    {
                        case 14:
                            queryAdd = "INSERT Рабочие (рабФИО, ДР, рабТелефон, Стаж, Разряд) " +
                                "VALUES(\'" + FIO + "\', \'" + bday + "\', \'" + phone + "\', " + stage + ", " + rank + ")";
                            mess = "Новый сотрудник успешно добавлен!";
                            break;
                        case 24:
                            int i = mainForm.GetIndex();
                            queryAdd = "UPDATE Рабочие " +
                                "SET рабФИО = \'" + FIO + "\', ДР = \'" + bday + "\', рабТелефон = \'" + phone+ "\', Стаж = " + stage + ", Разряд = " + rank + " " +
                                "WHERE Рабочие.Код = " + i;
                            mess = "Данные сотрудника успешно обновлены";
                            break;
                }
                    command = new SqlCommand(queryAdd, sqlConnection);
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show(mess);
                        sqlConnection.Close();
                        this.Close();
                    }
                    sqlConnection.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (sqlConnection.State == ConnectionState.Open)
                        sqlConnection.Close();
                }
            }
        }

        private void AddWorker_Load(object sender, EventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(Program.connection);
            Form2 mainForm = this.Owner as Form2;
            if (mainForm != null)
            {
                try
                {
                    switch (mainForm.GetFlag())
                    {
                        case 14:
                            button1.Text = "Добавить";
                            break;
                        case 24:
                            button1.Text = "Сохранить";

                            int i = mainForm.GetIndex();
                            string query = "SELECT рабФИО, ДР, рабТелефон, Стаж, Разряд " +
                                "FROM Рабочие WHERE Код = " + i;

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

                                    dateTimePicker1.Value = reader.GetDateTime(1);
                                    textBox4.Text = reader.GetString(2);
                                    textBox5.Text = reader.GetInt32(3).ToString();
                                    textBox6.Text = reader.GetInt32(4).ToString();
                                }
                            }
                            reader.Close();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (sqlConnection.State == ConnectionState.Open)
                        sqlConnection.Close();
                }
            }
        }
    }
}
