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
    public partial class AddSpecialization : Form
    {
        SqlConnection sqlConnection;
        int wId = -1;
        public AddSpecialization()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (wId > 0)
            {
                try
                {
                    sqlConnection = new SqlConnection(Program.connection);
                    SqlCommand command;
                    sqlConnection.Open();

                    string clothe = comboBox2.Text;

                    string q = "SELECT Код FROM Изделие WHERE Название LIKE \'" + clothe + "\'";
                    command = new SqlCommand(q, sqlConnection);
                    int id = Convert.ToInt32(command.ExecuteScalar());

                    string queryAdd = "INSERT Специализация (Код_работника, Код_изделия) " +
                                "VALUES(" + wId + ", " + id + ")";

                    command = new SqlCommand(queryAdd, sqlConnection);

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Специализация для сотрудника была успешно сохранена!");
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

        private void AddSpecialization_Load(object sender, EventArgs e)
        {
            Form2 mainForm = this.Owner as Form2;
            if (mainForm != null)
            {
                try
                {
                    string query = "SELECT Рабочие.рабФИО FROM Рабочие";

                    sqlConnection = new SqlConnection(Program.connection);
                    SqlCommand command = new SqlCommand(query, sqlConnection);
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

                    query = "SELECT Изделие.Название FROM Изделие";
                    command = new SqlCommand(query, sqlConnection);
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
            try
            {
                string name = comboBox1.SelectedItem.ToString();
                string queryWk = "SELECT Рабочие.Код FROM Рабочие WHERE Рабочие.рабФИО LIKE \'" + name +"\'";
                sqlConnection = new SqlConnection(Program.connection);
                SqlCommand command = new SqlCommand(queryWk, sqlConnection);
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();
                wId = Convert.ToInt32(command.ExecuteScalar());

                string query = "SELECT Изделие.Название " +
                                "FROM Изделие, Специализация " +
                                "WHERE Изделие.Код = Специализация.Код_изделия AND Специализация.Код_работника = " + wId;
                command = new SqlCommand(query, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                List<string> str = new List<string>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        str.Add(reader.GetString(0));
                    }
                }
                reader.Close();

                string[] t = new string[str.Count];
                for (int i = 0; i < str.Count; i++)
                {
                    t[i] = str[i];
                }
                textBox1.Lines = t;

                query = "SELECT Изделие.Название " +
                                "FROM Изделие " +
                                "WHERE Код NOT IN (SELECT Код_изделия FROM Специализация " +
                                    "WHERE Код_работника = " + wId + ")";

                command = new SqlCommand(query, sqlConnection);
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
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
