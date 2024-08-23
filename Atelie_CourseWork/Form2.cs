using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using OfficeOpenXml;

namespace Atelie_CourseWork
{
    public partial class Form2 : Form
    {
        private int flag = 1;
        private int i = 0;
        bool acs = true;
        SqlConnection sqlConnection;

        public int num()
        {
            int n = i;
            int j = 0;
            int id = 1;
            string table = "";
            switch (flag)
            {
                case 21:
                    table = "Заказы";
                    break;
                case 22:
                    table = "Клиенты";
                    break;
                case 23:
                    table = "Примерки";
                    break;
                case 24:
                    table = "Рабочие";
                    break;
                case 25:
                    table = "Материалы";
                    break;
                case 26:
                    table = "Фурнитура";
                    break;
                case 27:
                    table = "Услуги";
                    break;
                case 28:
                    table = "Специализация";
                    break;
                case 29:
                    table = "Изделие";
                    break;
                case 30:
                    table = "Расход";
                    break;
            }

            string iter = "SELECT Код FROM " + table;
            sqlConnection = new SqlConnection(Program.connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand(iter, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            List<int> codes = new List<int>();
            if (reader.HasRows)
            {
                while (reader.Read())
                    codes.Add(reader.GetInt32(0));
            }
            reader.Close();

            while (j < n)
            {
                id = codes[j];
                j++;
            }

            sqlConnection.Close();

            return id;
        }

        public int GetFlag()
        {
            return flag;
        }

        public void SetFlag(int f)
        {
            flag = f;
        }
        public int GetIndex()
        {
            return num();
        }
        public Form2()
        {
            InitializeComponent();

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            Program.connection = @"Data Source=DESKTOP-05RLPUM\SQLEXPRESS;Initial Catalog=Ателье;Integrated Security=True";
            /*using (SqlConnection connection = new SqlConnection(Program.connection))
            {
                string VIEW_Orders = "VIEW_Заказы";
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(VIEW_Orders, sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
            }*/
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT * FROM VIEW_Все_Заказы";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void материалыИФурнитураToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            switch (flag)
            { 
                case 1:
                    Addition form3 = new Addition();
                    form3.Owner = this;
                    flag = 11;
                    form3.Text = "Новый заказ";
                    form3.Show();
                    break;
                case 2:
                    AddClient formCl = new AddClient();
                    formCl.Owner = this;
                    flag = 12;
                    formCl.Text = "Новый клиент";
                    formCl.Show();
                    break;
                case 3:
                    AddTryOns formTryOns = new AddTryOns();
                    formTryOns.Owner = this;
                    flag = 13;
                    formTryOns.Text = "Добавить примерку";
                    formTryOns.Show();
                    break;
                case 4:
                    AddWorker formWk = new AddWorker();
                    formWk.Owner = this;
                    flag = 14;
                    formWk.Text = "Новый сотрудник";
                    formWk.Show();
                    break;
                case 5:
                    AddMaterials formMt = new AddMaterials();
                    formMt.Owner = this;
                    flag = 15;
                    formMt.Text = "Привоз материалов";
                    formMt.Show();
                    break;
                case 6:
                    AddMaterials formFr = new AddMaterials();
                    formFr.Owner = this;
                    flag = 16;
                    formFr.Text = "Привоз фурнитуры";
                    formFr.Show();
                    break;
                case 7:
                    AtelieUslugi formUs = new AtelieUslugi();
                    formUs.Owner = this;
                    flag = 17;
                    formUs.Text = "Услуги ателье";
                    formUs.Show();
                    break;
                case 8:
                    AddSpecialization formSp = new AddSpecialization();
                    formSp.Owner = this;
                    flag = 18;
                    formSp.Text = "Специализация сотрудника";
                    formSp.Show();
                    break;
                case 9:
                    AtelieUslugi formIz = new AtelieUslugi();
                    formIz.Owner = this;
                    flag = 19;
                    formIz.Text = "Изделия";
                    formIz.Show();
                    break;
                case 10:
                    Rasxody formRas = new Rasxody();
                    formRas.Owner = this;
                    flag = 20;
                    formRas.Text = "Расходы по заказам";
                    formRas.Show();
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label3.Text = "Заказы";
            flag = 1;
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT * FROM VIEW_Все_Заказы";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
                i = dataGridView1.CurrentRow.Index;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label3.Text = "Клиенты";
            flag = 2;
            try
            {
                dataGridView1.Columns[0].Visible = false;
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT * FROM VIEW_Клиенты";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
                i = dataGridView1.CurrentRow.Index;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            label3.Text = "Примерки";
            flag = 3;
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT * FROM VIEW_Примерки";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
                i = dataGridView1.CurrentRow.Index;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label3.Text = "Сотрудники";
            flag = 4;
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT * FROM VIEW_Сотрудники";

                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
                i = dataGridView1.CurrentRow.Index;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label3.Text = "Матриалы";
            flag = 5;
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT Название, Количество, Метраж, Стоимость AS [Стоимость за ед.] FROM Материалы";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
                i = dataGridView1.CurrentRow.Index;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label3.Text = "Фурнитура";
            flag = 6;
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT Название, Количество, Стоимость AS [Стоимость за ед.] FROM Фурнитура";

                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
                i = dataGridView1.CurrentRow.Index;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            label3.Text = "Специализация сотрудников";
            flag = 8;
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT * FROM VIEW_Специализация";

                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
                i = dataGridView1.CurrentRow.Index;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            switch (flag)
            {
                case 1:
                    Addition form3 = new Addition();
                    form3.Owner = this;
                    form3.Text = "Редактировать заказ";
                    i = dataGridView1.CurrentRow.Index+1;
                    flag = 21;
                    form3.Show();
                    break;
                case 2:
                    AddClient formCl = new AddClient();
                    formCl.Owner = this;
                    formCl.Text = "Редактировать данные клиента";
                    flag = 22;
                    formCl.Show();
                    break;
                case 3:
                    AddTryOns formTryOns = new AddTryOns();
                    formTryOns.Owner = this;
                    formTryOns.Text = "Редактировать время примерки";
                    flag = 23;
                    formTryOns.Show();
                    break;
                case 4:
                    AddWorker formWk = new AddWorker();
                    formWk.Owner = this;
                    formWk.Text = "Редактирование данных сотрудника";
                    flag = 24;
                    formWk.Show();
                    break;
                case 5:
                    AddMaterials formMt = new AddMaterials();
                    formMt.Owner = this;
                    flag = 25;
                    formMt.Text = "Привоз материалов";
                    formMt.Show();
                    break;
                case 6:
                    AddMaterials formFr = new AddMaterials();
                    formFr.Owner = this;
                    flag = 26;
                    formFr.Text = "Привоз фурнитуры";
                    formFr.Show();
                    break;
                case 7:
                    AtelieUslugi formUs = new AtelieUslugi();
                    formUs.Owner = this;
                    flag = 27;
                    formUs.Text = "Услуги ателье";
                    formUs.Show();
                    break;
                case 8:
                    AddSpecialization formSp = new AddSpecialization();
                    formSp.Owner = this;
                    flag = 28;
                    formSp.Text = "Специализация сотрудника";
                    formSp.Show();
                    break;
                case 9:
                    AtelieUslugi formIz = new AtelieUslugi();
                    formIz.Owner = this;
                    flag = 29;
                    formIz.Text = "Изделия";
                    formIz.Show();
                    break;
                case 10:
                    Rasxody formRas = new Rasxody();
                    formRas.Owner = this;
                    flag = 30;
                    formRas.Text = "Расходы по заказам";
                    formRas.Show();
                    break;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            i = dataGridView1.CurrentRow.Index + 1;
            MessageBox.Show("i = " + i);
        }

        private void button20_Click(object sender, EventArgs e)
        {/*
           DialogResult res = MessageBox.Show("Вы уверены, что хотите удалить выбранные данные?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            bool yes = Convert.ToBoolean(res);
            if (yes)
            {
                try
                {
                    i = dataGridView1.CurrentRow.Index + 1;
                    sqlConnection = new SqlConnection(Program.connection);
                    sqlConnection.Open();
                    string query = "", mess = "";
                    string check = "SELECT * FROM Заказы WHERE EXISTS (SELECT Заказы.Код_клиента WHERE Заказы.Код_клиента = " + i + ")";
                    SqlCommand command = new SqlCommand(check, sqlConnection);
                    bool exists = Convert.ToBoolean(command.ExecuteScalar());
                    if (exists)
                    {
                        MessageBox.Show("Для данного клиента существует запись в таблице Заказы. Сначала удалите записи, связанные с этим клиентом, а затем удалите данные клиента");
                        return;
                    }
                    query = "DELETE FROM Клиенты WHERE Код = " + i;
                    mess = "Данные клиента успешно удалены!";
                    
                    switch (flag)
                    {
                        case 1:
                            query = "DELETE FROM Заказы WHERE Код = " + i;
                            mess = "Заказ успешно удален!";
                            break;
                        case 2:
                            query = "DELETE FROM Клиенты WHERE Код = " + i;
                            mess = "Данные клиента успешно удалены!";
                            break;
                        case 3:
                            query = "DELETE FROM Примерки WHERE Код = " + i;
                            mess = "Назначенное время примерки успешно удалено!";
                            break;
                        case 4:
                            query = "DELETE FROM Рабочие WHERE Код = " + i;
                            mess = "Данные сторудника успешно удалены!";
                            break;
                        case 5:
                            query = "DELETE FROM Материалы WHERE Код = " + i;
                            mess = "Материал удален из списка учета!";
                            break;
                        case 6:
                            query = "DELETE FROM Фурнитура WHERE Код = " + i;
                            mess = "Фурнитура удалена из списка учета!";
                            break;
                        case 7:
                            query = "DELETE";
                            mess = "";
                            break;
                        case 8:
                            query = "DELETE";
                            mess = "Работник больше не специализируется по предмету: ";
                            break;
                    }
                    command = new SqlCommand(query, sqlConnection);
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show(mess);
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
            }*/
        }

        private void button9_Click(object sender, EventArgs e)
        {/*
            string query = "";

            switch (flag)
            {
                case 1:
                    query = "SELECT * FROM VIEW_Все_Заказы ORDER BY Дата";
                    break;
                case 2:
                    query = "SELECT * FROM VIEW_Клиенты";
                    break;
                case 3:
                    query = "SELECT * FROM VIEW_Примерки";
                    break;
                case 4:
                    query = "SELECT * FROM VIEW_Сотрудники";
                    break;
                case 5:
                    query = "SELECT Название, Количество, Метраж, Стоимость AS [Стоимость за ед.] FROM Материалы";
                    break;
                case 6:
                    query = "SELECT Название, Количество, Стоимость AS [Стоимость за ед.] FROM Фурнитура";
                    break;
                case 7:
                    query = "SELECT Название FROM Услуги";
                    break;
                case 8:
                    query = "SELECT * FROM VIEW_Специализация";
                    break;
            }*/
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Файлы Excel (*.xls; *.xlsx) | *.xls; *.xlsx";
                saveDialog.DefaultExt = ".xls";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    path = saveDialog.FileName;
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                    if (xlApp == null)
                    {
                        MessageBox.Show("Excel is not properly installed!!");
                        return;
                    }
                    object misValue = Missing.Value;
                    Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                    Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;

                    xlWorkBook = xlApp.Workbooks.Add(misValue);

                    if (File.Exists(path))
                    {
                        Microsoft.Office.Interop.Excel.Workbook xlWorkBook1 = xlApp.Workbooks.Open(path, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                        Microsoft.Office.Interop.Excel.Sheets worksheets = xlWorkBook1.Worksheets;
                        var xlSheets = xlWorkBook1.Sheets as Microsoft.Office.Interop.Excel.Sheets;
                        var xlNewSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);
                        xlSheets.Delete();
                        xlWorkBook1.Save();
                        xlWorkBook1.Close();
                    }

                    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    for (int k = 1; k < dataGridView1.ColumnCount + 1; k++)
                    {
                        xlWorkSheet.Cells[1, k] = dataGridView1.Columns[k - 1].HeaderText;
                    }

                    for (int i = 1; i < dataGridView1.Rows.Count + 1; i++)
                    {
                        for (int j = 1; j < dataGridView1.ColumnCount + 1; j++)
                        {
                            xlWorkSheet.Cells[i + 1, j] = dataGridView1.Rows[i - 1].Cells[j - 1].Value;
                        }
                    }

                    xlWorkBook.SaveAs(path, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();

                    Marshal.ReleaseComObject(xlWorkSheet);
                    Marshal.ReleaseComObject(xlWorkBook);
                    Marshal.ReleaseComObject(xlApp);
                    MessageBox.Show("Сохранение в файл прошло успешно!");
                }
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            label3.Text = "Услуги ателье";
            flag = 7;
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT Название FROM Услуги";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            label3.Text = "Изделия";
            flag = 9;
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT Название FROM Изделие";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            label3.Text = "Расходы";
            flag = 10;
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT * FROM VIEW_Расход_всего";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void материалыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
        }

        private void фурнитураToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button5_Click(sender, e);
        }

        private void материаловToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddMaterials formMt = new AddMaterials();
            formMt.Owner = this;
            flag = 15;
            formMt.Text = "Привоз материалов";
            formMt.Show();
        }

        private void фурнитурыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddMaterials formFr = new AddMaterials();
            formFr.Owner = this;
            flag = 16;
            formFr.Text = "Привоз фурнитуры";
            formFr.Show();
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddClient formCl = new AddClient();
            formCl.Owner = this;
            flag = 12;
            formCl.Text = "Новый клиент";
            formCl.Show();
        }

        private void просмотретьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            label3.Text = "Клиенты";
            flag = 2;
            try
            {
                dataGridView1.Columns[0].Visible = false;
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT * FROM VIEW_Клиенты";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
                i = dataGridView1.CurrentRow.Index;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void редактироватьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddClient formCl = new AddClient();
            formCl.Owner = this;
            formCl.Text = "Редактировать данные клиента";
            flag = 22;
            formCl.Show();
        }

        private void новыйToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Addition form3 = new Addition();
            form3.Owner = this;
            flag = 11;
            form3.Text = "Новый заказ";
            form3.Show();
        }

        private void редактироватьДанныеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Addition form3 = new Addition();
            form3.Owner = this;
            form3.Text = "Редактировать заказ";
            i = dataGridView1.CurrentRow.Index + 1;
            flag = 21;
            form3.Show();
        }

        private void всеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label3.Text = "Заказы";
            flag = 1;
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT * FROM VIEW_Все_Заказы";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void заказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void неоплаченныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label3.Text = "Заказы";
            flag = 1;
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT * FROM VIEW_Все_Заказы WHERE Статус LIKE \'не оплачен\'";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void оплаченныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label3.Text = "Заказы";
            flag = 1;
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT * FROM VIEW_Все_Заказы WHERE Статус LIKE \'оплачен\'";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void отмененныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label3.Text = "Заказы";
            flag = 1;
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT * FROM VIEW_Все_Заказы WHERE Статус LIKE \'отменен\'";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void срочныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label3.Text = "Заказы";
            flag = 1;
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT * FROM VIEW_Все_Заказы WHERE Статус LIKE \'предоплата\'";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void сотрудникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button4_Click(sender, e);
        }

        private void просмотретьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            label3.Text = "Сотрудники";
            flag = 4;
            try
            {
                sqlConnection = new SqlConnection(Program.connection);
                sqlConnection.Open();
                string query = "SELECT * FROM VIEW_Сотрудники";

                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void новыйToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AddWorker formWk = new AddWorker();
            formWk.Owner = this;
            flag = 14;
            formWk.Text = "Новый сотрудник";
            formWk.Show();
        }

        private void редактироватьДанныеToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AddWorker formWk = new AddWorker();
            formWk.Owner = this;
            formWk.Text = "Редактирование данных сотрудника";
            flag = 24;
            formWk.Show();
        }

        private void действияToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            button17_Click(sender, e);
        }

        private void редактироватьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            button19_Click(sender, e);
        }

        private void отчетToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            button12_Click(sender, e);
        }

        private void услугиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button6_Click_1(sender, e);
        }

        private void примеркиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button7_Click(sender, e);
        }

        private void специализацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button8_Click(sender, e);
        }

        private void изделияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button13_Click(sender, e);
        }

        private void расходыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button14_Click(sender, e);
        }

        private void добавитьToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Rasxody formRas = new Rasxody();
            formRas.Owner = this;
            flag = 20;
            formRas.Text = "Расходы по заказам";
            formRas.Show();
        }

        private void добавитьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AtelieUslugi formIz = new AtelieUslugi();
            formIz.Owner = this;
            flag = 19;
            formIz.Text = "Изделия";
            formIz.Show();
        }

        private void добавитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddSpecialization formSp = new AddSpecialization();
            formSp.Owner = this;
            flag = 18;
            formSp.Text = "Специализация сотрудника";
            formSp.Show();
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AtelieUslugi formUs = new AtelieUslugi();
            formUs.Owner = this;
            flag = 17;
            formUs.Text = "Услуги ателье";
            formUs.Show();
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AtelieUslugi formUs = new AtelieUslugi();
            formUs.Owner = this;
            flag = 27;
            formUs.Text = "Услуги ателье";
            formUs.Show();
        }

        private void редактроватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSpecialization formSp = new AddSpecialization();
            formSp.Owner = this;
            flag = 28;
            formSp.Text = "Редактироваит специализацию сотрудника";
            formSp.Show();
        }

        private void редактироватьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AtelieUslugi formIz = new AtelieUslugi();
            formIz.Owner = this;
            flag = 29;
            formIz.Text = "Изделия";
            formIz.Show();
        }

        private void редактироватьToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Rasxody formRas = new Rasxody();
            formRas.Owner = this;
            flag = 30;
            formRas.Text = "Расходы по заказам";
            formRas.Show();
        }
    }
}
