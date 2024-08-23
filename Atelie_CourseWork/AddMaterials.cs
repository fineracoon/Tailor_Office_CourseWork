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
    public partial class AddMaterials : Form
    {
        public int flag;
        bool newOne = false;
        SqlConnection sqlConnection;
        public AddMaterials()
        {
            InitializeComponent();
            if (flag == 0)
                this.Text = "Привоз материалов";
            else
                this.Text = "Привоз фурнитуры";
        }

        private void AddMaterials_Load(object sender, EventArgs e)
        {
            Form2 mainform = this.Owner as Form2;
            this.flag = mainform.GetFlag();
            if (mainform != null)
            {
                switch (flag % 2)
                {
                    case 1:
                        try
                        {
                            label4.Visible = true;
                            textBox3.Visible = true;
                            if (flag == 15)
                            {
                                checkBox1.Visible = true;
                                button2.Text = "Добавить";
                            }
                            else
                            {
                                button2.Text = "Сохранить";
                                checkBox1.Visible = false;
                            }

                            string queryMaterials = "SELECT Материалы.Название FROM Материалы";

                            sqlConnection = new SqlConnection(Program.connection);
                            SqlCommand command = new SqlCommand(queryMaterials, sqlConnection);
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

                            if (flag == 25)
                            {
                                int i = mainform.GetIndex();
                                string q = "SELECT Название, Количество, Стоимость, Метраж " +
                                    "FROM Материалы WHERE Код = " + i;

                                command = new SqlCommand(q, sqlConnection);
                                reader = command.ExecuteReader();

                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        comboBox1.Text = reader.GetString(0);
                                        textBox1.Text = reader.GetInt32(1).ToString();
                                        textBox2.Text = String.Format(CultureInfo.CreateSpecificCulture("be-BY"), "{0:C2}", reader.GetDecimal(2));
                                        textBox3.Text = String.Format("{0:F2}", reader.GetDouble(3));
                                    }
                                }
                                reader.Close();
                            }

                            sqlConnection.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        break;
                    case 0:
                        try
                        {
                            label4.Visible = false;
                            textBox3.Visible = false;
                            if (flag == 16)
                            {
                                checkBox1.Visible = true;
                                button2.Text = "Добавить";
                            }
                            else
                            {
                                button2.Text = "Сохранить";
                                checkBox1.Visible = false;
                            }

                            string queryFurnitura = "SELECT Фурнитура.Название FROM Фурнитура";

                            sqlConnection = new SqlConnection(Program.connection);
                            SqlCommand command = new SqlCommand(queryFurnitura, sqlConnection);
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

                            if (flag == 26)
                            {
                                int i = mainform.GetIndex();
                                string q = "SELECT Название, Количество, Стоимость " +
                                    "FROM Фурнитура WHERE Код = " + i;

                                command = new SqlCommand(q, sqlConnection);
                                reader = command.ExecuteReader();

                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        comboBox1.Text = reader.GetString(0);
                                        textBox1.Text = reader.GetInt32(1).ToString();
                                        textBox2.Text = String.Format(CultureInfo.CreateSpecificCulture("be-BY"), "{0:C2}", reader.GetDecimal(2));
                                    }
                                }
                                reader.Close();
                            }

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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                newOne = true;
            else
                newOne = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand command;
            Form2 mainForm = this.Owner as Form2;
            switch (flag % 2)
            {
                case 1:
                    try
                    {
                        int codMat = -1;

                        string m = comboBox1.Text;

                        string queryMt = "SELECT Материалы.Код FROM Материалы WHERE Материалы.Название LIKE \'" + m + "\'";
                        sqlConnection = new SqlConnection(Program.connection);
                        command = new SqlCommand(queryMt, sqlConnection);
                        sqlConnection.Open();
                        codMat = Convert.ToInt32(command.ExecuteScalar());

                        if (flag == 15)
                        {
                            if (codMat <= 0)
                            {
                                newOne = true;
                            }
                            else
                            {
                                newOne = false;
                            }
                            if (!newOne && checkBox1.Checked)
                            {
                                MessageBox.Show("Материал с таким названием уже есть в базе данных!");
                                sqlConnection.Close();
                                return;
                            }
                        }

                        int count = Convert.ToInt32(textBox1.Text);
                        Double price;
                        if (Double.TryParse(textBox2.Text, out price))
                        {
                            textBox2.Text = String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("be-BY"), "{0:C2}", price);
                        }
                        else
                        {
                            MessageBox.Show("Неверно введенные данные в поле Цена");
                        }

                        Double len;
                        if (Double.TryParse(textBox3.Text, out len))
                        {
                            textBox3.Text = len.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Неверно введенные данные в поле Метраж");
                        }


                        if (codMat > -1)
                        {
                            string queryAdd;
                            if (newOne)
                            {
                                queryAdd = "INSERT Материалы (Название, Количество, Стоимость, Метраж) " +
                                    "VALUES(\'" + m + "\', " + count + ", " + price + ", " + len +")";
                            }
                            else
                            {
                                queryAdd = "UPDATE Материалы SET Количество = " + count + ", Стоимость = " + price + ", Метраж = " + len +
                                    " WHERE Материалы.Код = " + codMat;
                            }
                            if (flag == 25)
                            {
                                int i = mainForm.GetIndex();
                                queryAdd = "UPDATE Материалы SET Количество = " + count + ", Стоимость = " + price +
                                    " WHERE Код = " + i;
                            }
                            command = new SqlCommand(queryAdd, sqlConnection);
                            if (command.ExecuteNonQuery() == 1)
                            {
                                MessageBox.Show("Данные успешно сохранены!");
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
                    break;
                case 0:
                    try
                    {
                        int codFur = -1;

                        string f = comboBox1.Text;

                        string queryCl = "SELECT Фурнитура.Код FROM Фурнитура WHERE Фурнитура.Название LIKE \'" + f + "\'";
                        sqlConnection = new SqlConnection(Program.connection);
                        command = new SqlCommand(queryCl, sqlConnection);
                        sqlConnection.Open();
                        codFur = Convert.ToInt32(command.ExecuteScalar());

                        if (flag == 16)
                        {
                            if (codFur <= 0)
                            {
                                newOne = true;
                            }
                            else
                            {
                                newOne = false;
                            }
                            if (!newOne && checkBox1.Checked)
                            {
                                MessageBox.Show("Фурнитура с таким названием уже есть в базе данных!");
                                sqlConnection.Close();
                                return;
                            }
                        }

                        int count = Convert.ToInt32(textBox1.Text);
                        Double price = Convert.ToDouble(textBox2.Text);
                        if (Double.TryParse(textBox2.Text, out price))
                        {
                            textBox2.Text = String.Format(CultureInfo.CreateSpecificCulture("be-BY"), "{0:C2}", price);
                        }
                        else
                        {
                            textBox2.Text = price.ToString();
                        }

                        if (codFur > -1)
                        {
                            string queryAdd;
                            if (newOne)
                            {
                                queryAdd = "INSERT Фурнитура (Название, Количество, Стоимость) " +
                                    "VALUES(\'" + f + "\', " + count + ", " + price + ")";
                            }
                            else
                            {
                                queryAdd = "UPDATE Фурнитура SET Количество = " + count + ", Стоимость = " + price +
                                    " WHERE Код = " + codFur;
                            }
                            if (flag == 26)
                            {
                                int i = mainForm.GetIndex();
                                queryAdd = "UPDATE Фурнитура SET Количество = " + count + ", Стоимость = " + price.ToString().Replace(',', '.') +
                                    " WHERE Код = " + i;
                            }
                            command = new SqlCommand(queryAdd, sqlConnection);
                            if (command.ExecuteNonQuery() == 1)
                            {
                                MessageBox.Show("Данные успешно сохранены!");
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
                    break;
            }
        }
    }
}
