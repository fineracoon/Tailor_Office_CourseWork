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
    public partial class Rasxody : Form
    {
        SqlConnection sqlConnection;
        public Rasxody()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Rasxody_Load(object sender, EventArgs e)
        {
            Form2 mainForm = this.Owner as Form2;
            if (mainForm != null)
            {
                try
                {
                    sqlConnection = new SqlConnection(Program.connection);
                    string getCustomers = "SELECT DISTINCT Заказы.Дата FROM Заказы";
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(getCustomers, sqlConnection);
                    SqlDataReader reader = command.ExecuteReader();

                    comboBox1.Items.Clear();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader.GetDateTime(0).ToString("dd.MM.yyyy"));
                        }
                    }
                    reader.Close();

                    string materials = "SELECT Название FROM Материалы";
                    string furnitura = "SELECT Название FROM Фурнитура";

                    command = new SqlCommand(materials, sqlConnection);
                    reader = command.ExecuteReader();

                    comboBox4.Items.Clear();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            comboBox4.Items.Add(reader.GetString(0));
                        }
                    }
                    reader.Close();

                    command = new SqlCommand(furnitura, sqlConnection);
                    reader = command.ExecuteReader();

                    comboBox5.Items.Clear();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            comboBox5.Items.Add(reader.GetString(0));
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
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form2 mainForm = this.Owner as Form2;
            if (mainForm != null)
            {
                try
                {
                    DateTime date = Convert.ToDateTime(comboBox1.Text);
                    sqlConnection = new SqlConnection(Program.connection);
                    string getCustomers = "SELECT клФИО FROM Заказы, Клиенты " +
                        "WHERE Дата LIKE \'" + date.ToString("yyyy-MM-dd") + "\' AND Заказы.Код_клиента = Клиенты.Код";

                    SqlCommand command = new SqlCommand(getCustomers, sqlConnection);
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
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form2 mainForm = this.Owner as Form2;
            if (mainForm != null)
            {
                try
                {
                    string fio = comboBox2.Text;
                    DateTime date = Convert.ToDateTime(comboBox1.Text);
                    sqlConnection = new SqlConnection(Program.connection);
                    string getUslugi = "SELECT Услуги.Название FROM Услуги, Заказы, Клиенты " +
                        "WHERE Заказы.Код_услуги = Услуги.Код AND Заказы.Код_клиента = Клиенты.Код " +
                        "AND Клиенты.клФИО LIKE \'" + fio + "\' AND Заказы.Дата LIKE \'" + date.ToString("yyyy-MM-dd") + "\'";

                    SqlCommand command = new SqlCommand(getUslugi, sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    comboBox3.Items.Clear();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            comboBox3.Items.Add(reader.GetString(0));
                        }
                    }
                    reader.Close();

                    comboBox3.SelectedIndex = 0;
                    comboBox3_SelectedIndexChanged(sender, e);
                    sqlConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form2 mainForm = this.Owner as Form2;
            if (mainForm != null)
            {
                try
                {
                    string uslugi = comboBox3.Text;
                    string fio = comboBox2.Text;
                    DateTime date = Convert.ToDateTime(comboBox1.Text);
                    sqlConnection = new SqlConnection(Program.connection);
                    string getUslugi = "SELECT Срок, Статус, Цена, Изделие.Название, рабФИО " +
                        "FROM Заказы, Услуги, Специализация, Изделие, Рабочие, Клиенты " +
                        "WHERE Заказы.Код_специализации = Специализация.Код " +
                        "AND Специализация.Код_работника = Рабочие.Код " +
                        "AND Специализация.Код_изделия = Изделие.Код " +
                        "AND Заказы.Код_клиента = Клиенты.Код " +
                        "AND Клиенты.клФИО LIKE \'" + fio + "\' " +
                        "AND Заказы.Код_услуги = Услуги.Код " +
                        "AND Услуги.Название LIKE \'" + uslugi + "\' " +
                        "AND Заказы.Дата LIKE \'" + date.ToString("yyyy-MM-dd") + "\'";

                    SqlCommand command = new SqlCommand(getUslugi, sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dateTimePicker1.Value = reader.GetDateTime(0);
                            textBox1.Text = reader.GetString(1);
                            textBox2.Text = String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("be-BY"), "{0:C2}", reader.GetDecimal(2));
                            textBox3.Text = reader.GetString(3);
                            textBox4.Text = reader.GetString(4);
                        }
                    }
                    reader.Close();

                    sqlConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 mainForm = this.Owner as Form2;
            if (mainForm != null)
            {
                try
                {
                    sqlConnection = new SqlConnection(Program.connection);
                    sqlConnection.Open();
                    SqlCommand command;
                    string material = comboBox4.Text, furnitura = comboBox5.Text;
                    
                    string getMaterial = "SELECT Код FROM Материалы WHERE Название LIKE \'" + material + "\'";
                    command = new SqlCommand(getMaterial, sqlConnection);
                    int mcode = Convert.ToInt32(command.ExecuteScalar());

                    string getFurnitura = "SELECT Код FROM Фурнитура WHERE Название LIKE \'" + furnitura + "\'";
                    command = new SqlCommand(getFurnitura, sqlConnection);
                    int fcode = Convert.ToInt32(command.ExecuteScalar());

                    int count1 = Convert.ToInt32(numericUpDown1.Value), count2 = Convert.ToInt32(numericUpDown2.Value);
                    /*
                    SELECT Заказы.Код 
                        FROM Услуги, Заказы, Изделие, Рабочие, Клиенты, Специализация
WHERE Заказы.Код_клиента = Клиенты.Код AND Заказы.Код_услуги = Услуги.Код
  AND Заказы.Код_специализации = Специализация.Код AND Специализация.Код_изделия = Изделие.Код AND Специализация.Код_работника = Рабочие.Код
GO*/
                    string uslugi = comboBox3.Text, fio = comboBox2.Text, izdelie = textBox3.Text, worker = textBox4.Text;
                    string zakaz = "SELECT Заказы.Код FROM Заказы " +
                        "WHERE Заказы.Код_услуги = (SELECT Услуги.Код FROM Услуги WHERE Название LIKE \'" + uslugi + "\') " +
                        "AND Заказы.Код_клиента = (SELECT Клиенты.Код FROM Клиенты WHERE клФИО LIKE \'" + fio + "\') " +
                        "AND Заказы.Код_специализации = (SELECT Код FROM Специализация WHERE Код_изделия = " +
                                                    "(SELECT Код FROM Изделие WHERE Название LIKE \'" + izdelie + "\') " +
                                                    "AND Код_работника = (SELECT Код FROM Рабочие WHERE рабФИО LIKE \'" + worker + "\'))";
                    command = new SqlCommand(zakaz, sqlConnection);
                    int zak = Convert.ToInt32(command.ExecuteScalar());

                    string insert = "INSERT INTO Расход (Код_материала, Код_фурнитуры, Код_заказа, Затрачено_материалов, Затрачено_фурнитуры) " +
                        "VALUES(" + mcode + ", " + fcode + ", " + zak + ", " + count1 + ", " + count2 + ")";

                    command = new SqlCommand(insert, sqlConnection);

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Расходы по заказу успешно сохранены");
                        sqlConnection.Close();
                        this.Close();
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
}
