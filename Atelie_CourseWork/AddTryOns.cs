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
    public partial class AddTryOns : Form
    {
        SqlConnection sqlConnection;
        int flag = 0;
        public AddTryOns()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddTryOns_Load(object sender, EventArgs e)
        {
            Form2 mainForm = this.Owner as Form2;
            if (mainForm != null)
            {
                try
                {
                    string queryCl = "SELECT Клиенты.клФИО FROM Клиенты WHERE Клиенты.Код IN (SELECT Заказы.Код_клиента FROM Заказы)";
                    sqlConnection = new SqlConnection(Program.connection);
                    SqlCommand command = new SqlCommand(queryCl, sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    comboBox1.Items.Clear();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader.GetString(0));
                        }
                    }
                    reader.Close();

                    if (mainForm.GetFlag() == 23)
                    {
                        int i = mainForm.GetIndex();
                        string q = "SELECT Клиенты.клФИО " +
                            "FROM Клиенты, Заказы " +
                            "WHERE Заказы.Код_клиента = Клиенты.Код AND Заказы.Код = " +
                                    "(SELECT Примерки.Код_заказа FROM Примерки WHERE Примерки.Код = " + i + ")";
                        if (sqlConnection.State != ConnectionState.Open)
                            sqlConnection.Open();
                        command = new SqlCommand(q, sqlConnection);

                        comboBox1.Text = command.ExecuteScalar().ToString();
                        comboBox2_SelectedIndexChanged(sender, e);
                        
                        string query = "SELECT Примерки.Дата, FORMAT(CAST(Примерки.Время AS DATETIME), 'HH:mm') Время FROM Примерки WHERE Примерки.Код = " + i;
                        command = new SqlCommand(query, sqlConnection);
                        sqlConnection.Open();
                        reader = command.ExecuteReader();
                        
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                dateTimePicker3.Value = reader.GetDateTime(0);
                                DateTime time = Convert.ToDateTime(reader.GetValue(1));
                                dateTimePicker4.Value = time;
                            }
                        }
                        reader.Close();
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
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string name = comboBox1.Text;
                string queryOrds = "SELECT Услуги.Название FROM Услуги " + 
                    "WHERE Услуги.Код IN (SELECT Заказы.Код_услуги FROM Заказы " +
                                            "WHERE Заказы.Код_клиента = (SELECT Клиенты.Код FROM Клиенты "+ 
                                                                            "WHERE Клиенты.клФИО LIKE \'" + name + "\'))";

                sqlConnection = new SqlConnection(Program.connection);
                SqlCommand command = new SqlCommand(queryOrds, sqlConnection);
                sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

                comboBox2.Items.Clear();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comboBox2.Items.Add(reader.GetString(0));
                    }
                }
                reader.Close();
                comboBox2.SelectedIndex = 0;
                comboBox2_SelectedIndexChanged(sender, e);

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string order = comboBox2.Text;
                string name = comboBox1.Text;
                string queryOrder = "SELECT Заказы.Дата, Заказы.Срок, Заказы.Цена, Заказы.Статус, Изделие.Название, Рабочие.рабФИО " +
                    "FROM Заказы, Специализация, Изделие, Рабочие " +
                    "WHERE Заказы.Код_специализации = Специализация.Код AND Специализация.Код_работника = Рабочие.Код " +
                    "AND Заказы.Код_клиента = (SELECT Клиенты.Код FROM Клиенты WHERE Клиенты.клФИО = \'" + name + "\') " +
                    "AND Заказы.Код_услуги = (SELECT Услуги.Код FROM Услуги WHERE Услуги.Название = \'" + order + "\') " +
                    "AND Специализация.Код_изделия = Изделие.Код";

                sqlConnection = new SqlConnection(Program.connection);
                SqlCommand command = new SqlCommand(queryOrder, sqlConnection);
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dateTimePicker1.Value = reader.GetDateTime(0);
                        dateTimePicker2.Value = reader.GetDateTime(1);
                        textBox3.Text = String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("be-BY"), "{0:C2}", reader.GetDecimal(2));
                        textBox4.Text = reader.GetString(3);
                        textBox6.Text = reader.GetString(4);
                        textBox7.Text = reader.GetString(5);
                    }
                }
                reader.Close();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 mainForm = this.Owner as Form2;
            try
            {
                DateTime date = dateTimePicker3.Value;
                DateTime time = dateTimePicker4.Value;

                int codOrd = -1;
                string order = comboBox2.Text;
                string name = comboBox1.Text;

                string queryOrd = "SELECT Заказы.Код FROM Заказы, Специализация, Изделие, Рабочие " +
                    "WHERE Заказы.Код_клиента = (SELECT Клиенты.Код FROM Клиенты WHERE Клиенты.клФИО = \'" + name + "\') " +
                    "AND Заказы.Код_услуги = (SELECT Услуги.Код FROM Услуги WHERE Услуги.Название = \'" + order + "\') ";

                sqlConnection = new SqlConnection(Program.connection);
                SqlCommand command = new SqlCommand(queryOrd, sqlConnection);
                sqlConnection.Open();
                codOrd = Convert.ToInt32(command.ExecuteScalar());
                string queryAdd = "", mess = "";
                if (codOrd > -1)
                {
                    switch (mainForm.GetFlag())
                    {
                        case 13:
                            queryAdd = "INSERT Примерки (Код_заказа, Дата, Время) " +
                                "VALUES(" + codOrd + ", \'" + date.ToString("yyyy-MM-dd") + "\', \'" + time.ToString("HH:mm") + "\')";
                            mess = "Примерка успешно назначена!";
                                break;
                        case 23:
                            int i = mainForm.GetIndex();
                            queryAdd = "UPDATE Примерки SET Код_заказа = " + codOrd + ", Дата = \'" + date.ToString("yyyy-MM-dd") + "\', " +
                                "Время = \'"+ time.ToString("HH:mm") +"\' " +
                                "WHERE Код = " + i;
                            mess = "Примерка успешно перенесена!";
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
    }
}
