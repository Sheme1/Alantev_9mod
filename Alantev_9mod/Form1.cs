using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text;

namespace Alantev_9mod
{
    public partial class Form1 : Form
    {
        DataBase dataBase = new DataBase();
        int selectedRow;
        public Form1()
        {
            InitializeComponent();
        }
        private void CreateColums()
        {
            dataGridView1.Columns.Add("StudID", "id");
            dataGridView1.Columns["StudID"].Visible = false;
            dataGridView1.Columns.Add("FirstName", "Имя");
            dataGridView1.Columns.Add("LastName", "Фамилия");
            dataGridView1.Columns.Add("Speciality", "Специальность");
            dataGridView1.Columns.Add("DateOfHire", "Дата рождения");
        }

        private void ReadSingleRow(DataGridView dgw, IDataReader record)
        {
            DateTime dateTime = record.GetDateTime(4);
            string dateOnlyString = dateTime.ToString("dd.MM.yyyy");
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), dateOnlyString);
        }


        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string query = $"select * from stud";

            SqlCommand command = new SqlCommand(query, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            CreateColums();
            RefreshDataGrid(dataGridView1);
        }

        private void Addbutton_Click_1(object sender, EventArgs e)
        {
            string FirstName = textBox1.Text;
            string LastName = textBox2.Text;
            string Speciality = textBox3.Text;
            DateTime newDateOfHire = DateTime.Parse(dateTimePicker1.Text);
            string formattedDate = newDateOfHire.ToString("yyyy-MM-dd");
            string query = $"insert into stud (FirstName, LastName, Speciality, DateOfHire) values ('{FirstName}','{LastName}','{Speciality}','{formattedDate}')";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());

            dataBase.openConnection();

            int affectedRows = command.ExecuteNonQuery();

            dataBase.closeConnection();

            if (affectedRows > 0)
            {
                RefreshDataGrid(dataGridView1);
            }
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }


        private void Changebutton_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                int rowID = int.Parse(dataGridView1[0, selectedIndex].Value.ToString());

                // Получите новые данные, которые вы хотите обновить
                string newFirstName = textBox1.Text;
                string newLastName = textBox2.Text;
                string newSpeciality = textBox3.Text;
                DateTime newDateOfHire = DateTime.Parse(dateTimePicker1.Text);
                string formattedDate = newDateOfHire.ToString("yyyy-MM-dd");

                string query = $"UPDATE stud SET FirstName = '{newFirstName}', LastName = '{newLastName}', Speciality = '{newSpeciality}', DateOfHire = '{formattedDate}' WHERE StudID = {rowID}";
                SqlCommand command = new SqlCommand(query, dataBase.getConnection());

                dataBase.openConnection();

                int affectedRows = command.ExecuteNonQuery();

                dataBase.closeConnection();

                if (affectedRows > 0)
                {
                    dataGridView1.Rows[selectedIndex].Cells["FirstName"].Value = newFirstName;
                    dataGridView1.Rows[selectedIndex].Cells["LastName"].Value = newLastName;
                    dataGridView1.Rows[selectedIndex].Cells["Speciality"].Value = newSpeciality;
                    dataGridView1.Rows[selectedIndex].Cells["DateOfHire"].Value = formattedDate;
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для обновления");
            }
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }


        private void Deletebutton_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                int rowID = int.Parse(dataGridView1[0, selectedIndex].Value.ToString());

                string query = $"DELETE FROM stud WHERE StudID = {rowID}";
                SqlCommand command = new SqlCommand(query, dataBase.getConnection());

                dataBase.openConnection();

                int affectedRows = command.ExecuteNonQuery();

                dataBase.closeConnection();

                if (affectedRows > 0)
                {
                    dataGridView1.Rows.RemoveAt(selectedIndex);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления");
            }
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBox1.SelectedIndex;
            if (selectedIndex == 0)
            {
                dataGridView1.Sort(dataGridView1.Columns["DateOfHire"], ListSortDirection.Ascending);
            }

            else if (selectedIndex == 1)
            {
                dataGridView1.Sort(dataGridView1.Columns["FirstName"], ListSortDirection.Ascending);
            }
            else if (selectedIndex == 2)
            {
                dataGridView1.Sort(dataGridView1.Columns["Speciality"], ListSortDirection.Ascending);
            }

            else if (selectedIndex == 3)
            {
                dataGridView1.Sort(dataGridView1.Columns["LastName"], ListSortDirection.Ascending);
            }
            else
            {
                MessageBox.Show("Выберите пункт для сортировки");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Selected = false;
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                        if (dataGridView1.Rows[i].Cells[j].Value.ToString().Contains(textBox5.Text))
                        {
                            dataGridView1.Rows[i].Selected = true;
                            break;
                        }
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV File|*.csv";
            saveFileDialog.Title = "Сохранить данные DataGridView в CSV";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    stringBuilder.Append(dataGridView1.Columns[i].HeaderText);
                    stringBuilder.Append(i == dataGridView1.Columns.Count - 1 ? "\n" : ",");
                }

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        stringBuilder.Append(row.Cells[j].Value);
                        stringBuilder.Append(j == row.Cells.Count - 1 ? "\n" : ",");
                    }
                }

                File.WriteAllText(saveFileDialog.FileName, stringBuilder.ToString());
            }
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                textBox1.Text = row.Cells["FirstName"].Value.ToString();
                textBox2.Text = row.Cells["LastName"].Value.ToString();
                textBox3.Text = row.Cells["Speciality"].Value.ToString();
                textBox4.Text = row.Cells["DateOfHire"].Value.ToString();
            }
        }


        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV File|*.csv";
            openFileDialog.Title = "Загрузить данные CSV в DataGridView";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                if (lines.Length > 0)
                {
                    string[] headerColumns = lines[0].Split(',');
                    foreach (string headerColumn in headerColumns)
                    {
                        dataGridView1.Columns.Add(headerColumn, headerColumn);
                    }
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] dataColumns = lines[i].Split(',');
                        dataGridView1.Rows.Add(dataColumns);
                    }
                }
            }
        }
    }
}