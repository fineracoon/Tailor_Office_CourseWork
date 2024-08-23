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
    public partial class Addition : Form
    {
        SqlConnection sqlConnection;
        Form2 mainForm;
        public Addition()
        {
            InitializeComponent();
        }

        private void Addition_Load(object sender, EventArgs e)
        {
            mainForm = this.Owner as Form2;
            if (mainForm != null)
            {
                try
                {
                    string queryCl = "SELECT Клиенты.клФИО FROM Клиенты";
                    string queryMs = "SELECT Изделие.Название FROM Изделие";
                    string queryUs = "SELECT Услуги.Название FROM Услуги";

                    sqlConnection = new SqlConnection(Program.connection);
                    SqlCommand command = new SqlCommand(queryCl, sqlConnection);
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

                    command = new SqlCommand(queryMs, sqlConnection);
                    reader = command.ExecuteReader();

                    comboBox2.Items.Clear();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            comboBox2.Items.Add(reader.GetString(0));
                        }
                    }
                    reader.Close();

                    command = new SqlCommand(queryUs, sqlConnection);
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

                    sqlConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                switch (mainForm.GetFlag())
                {
                    case 11:
                        checkBox1.Visible = true;
                        
                        break;
                    case 21:
                        checkBox1.Visible = false;
                        try
                        {
                            int i = mainForm.GetIndex();
                            string query = "SELECT Заказы.Дата, Заказы.Срок, Заказы.Статус, " +
                                                "Заказы.Цена, Клиенты.клФИО, Услуги.Название, " +
                                                "Изделие.Название, Рабочие.рабФИО " +
                                "FROM Заказы, Услуги, Клиенты, Рабочие, Изделие, Специализация " +
                                "WHERE Заказы.Код = " + i + " AND Заказы.Код_клиента = Клиенты.Код " +
                                        "AND Заказы.Код_услуги = Услуги.Код " +
                                "AND Заказы.Код_специализации = Специализация.Код " +
                                "AND Заказы.Код_специализации = Специализация.Код " +
                                "AND Специализация.Код_работника = Рабочие.Код " +
                                "AND Специализация.Код_изделия = Изделие.Код";

                            sqlConnection = new SqlConnection(Program.connection);
                            SqlCommand command = new SqlCommand(query, sqlConnection);
                            sqlConnection.Open();
                            SqlDataReader reader = command.ExecuteReader();

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    dateTimePicker1.Value = reader.GetDateTime(0);
                                    dateTimePicker2.Value = reader.GetDateTime(1);
                                    comboBox1.Text = reader.GetString(2);
                                    textBox1.Text = String.Format(CultureInfo.CreateSpecificCulture("be-BY"), "{0:C2}", reader.GetDecimal(3));
                                    comboBox3.Text = reader.GetString(4);
                                    comboBox4.Text = reader.GetString(5);
                                    comboBox2.Text = reader.GetString(6);
                                    comboBox5.Text = reader.GetString(7);
                                }
                            }
                            reader.Close();

                            sqlConnection.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainForm = this.Owner as Form2;
            try
            {
                int codUslugi = -1, codClienta = -1, codSpec = -1;
                bool newOne = false;

                string queryID = "SELECT MAX(Заказы.Код) FROM Заказы";
                sqlConnection = new SqlConnection(Program.connection);
                SqlCommand command = new SqlCommand(queryID, sqlConnection);
                sqlConnection.Open();
                int id = Convert.ToInt32(command.ExecuteScalar());
                id++;

                string client = comboBox3.Text;
                string queryCl = "SELECT Клиенты.Код FROM Клиенты WHERE Клиенты.клФИО LIKE \'" + client + "\'";
                sqlConnection = new SqlConnection(Program.connection);
                command = new SqlCommand(queryCl, sqlConnection);
                sqlConnection.Open();
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
                    if (!newOne && checkBox1.Checked)
                    {
                        MessageBox.Show("Клиент с такой фамилией уже есть в базе данных!");
                        sqlConnection.Close();
                        return;
                    }
                    else if (newOne && !checkBox1.Checked)
                    {
                        DialogResult res = MessageBox.Show("Этот клиент еще не был добален в базу данных. Желаете сделать это сейчас?", "", MessageBoxButtons.YesNo);
                        switch (res)
                        {
                            case DialogResult.Yes:
                                string qu = "INSERT Клиенты (клФИО) VALUES (\'" + client + "\')";
                                command = new SqlCommand(qu, sqlConnection);
                                if (command.ExecuteNonQuery() == 1)
                                {
                                    MessageBox.Show("Клиент добавлен в базу данных, но Вы должны редактировать его данные в дальнейшем!");
                                }
                                break;
                            case DialogResult.No:
                                MessageBox.Show("Заказ не был сохранен.");
                                return;
                        }
                    }
                }

                DateTime date1 = dateTimePicker1.Value;
                DateTime date2 = dateTimePicker2.Value;
                string status = comboBox1.Text;
                Double price;
                if (Double.TryParse(textBox1.Text, out price))
                {
                    textBox1.Text = String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("be-BY"), "{0:C2}", price);
                }
                else
                {
                    textBox1.Text = String.Empty;
                }

                string subj = comboBox4.Text;
                string clothe = comboBox2.Text;
                string master = comboBox5.Text;

                string queryIzdelie = "SELECT Изделие.Код FROM Изделие WHERE Название LIKE \'" + clothe + "\'";
                command = new SqlCommand(queryIzdelie, sqlConnection);
                int codIzdelia = Convert.ToInt32(command.ExecuteScalar());

                string querySpec = "SELECT Специализация.Код FROM Специализация, Рабочие " +
                    "WHERE Специализация.Код_работника = Рабочие.Код AND Рабочие.рабФИО LIKE \'" + master + "\'" 
                    + " AND Специализация.Код_изделия = " + codIzdelia;
                command = new SqlCommand(querySpec, sqlConnection);
                codSpec = Convert.ToInt32(command.ExecuteScalar());

                string queryUs = "SELECT Услуги.Код FROM Услуги WHERE Услуги.Название = \'" + subj + "\'";
                command = new SqlCommand(queryUs, sqlConnection);
                codUslugi = Convert.ToInt32(command.ExecuteScalar());

                string queryAdd = "";
                if (codClienta > -1 && codUslugi > -1 && codSpec > -1)
                {
                    switch (mainForm.GetFlag())
                    {
                        case 11:
                            queryAdd = "INSERT Заказы (Дата, Срок, Статус, Цена, Код_услуги, Код_клиента, Код_специализации) " +
                        "VALUES(\'" + date1.ToString("yyyy-MM-dd") + "\', \'" + date2.ToString("yyyy-MM-dd") + "\', \'" + status +
                                  "\', " + price.ToString() + ", " + codUslugi + ", " + codClienta + ", " + codSpec + ")";
                            break;
                        case 21:
                            int i = mainForm.GetIndex();
                            queryAdd = "UPDATE Заказы " +
                                "SET Дата = \'" + date1.ToString("yyyy-MM-dd") + "\', Срок = \'" + date2.ToString("yyyy-MM-dd") + "\', " +
                                "Статус = \'" + status + "\', Цена = " + price.ToString() + ", Код_услуги = " + codUslugi + ", " +
                                "Код_клиента = " + codClienta + ", Код_специализации = " + codSpec + 
                                "WHERE Заказы.Код = " + i;
                            break;
                    }
                    command = new SqlCommand(queryAdd, sqlConnection);
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Заказ успешно сохранен!");
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string clothe = comboBox2.Text;
                string queryWk = "SELECT Рабочие.рабФИО FROM Рабочие WHERE Код IN " +
                    "(SELECT Код_работника FROM Специализация WHERE Код_изделия = (SELECT Код FROM Изделие WHERE Название LIKE \'" + clothe + "\'))";

                sqlConnection = new SqlConnection(Program.connection);
                SqlCommand command = new SqlCommand(queryWk, sqlConnection);
                sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

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
}
