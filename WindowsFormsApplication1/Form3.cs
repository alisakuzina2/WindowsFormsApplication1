using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        private string connectionString =
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
            Path.Combine(Application.StartupPath, "Orders.accdb") + ";";

        private List<string> orderItems;
        private decimal orderTotal;

        public Form3(List<string> orderItems, decimal total)
        {
            InitializeComponent();

            this.orderItems = orderItems;
            this.orderTotal = total;

            listBox1.Items.Clear();

            foreach (string item in orderItems)
            {
                listBox1.Items.Add(item);
            }

            label6.Text = "Итого: " + total.ToString("0.##") + " ₽";
        }

        // Кнопка "Назад/Закрыть"
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Основное сохранение заказа
        private void button2_Click(object sender, EventArgs e)
        {
            SaveOrder();
        }

        // Этот обработчик нужен Designer.cs
        private void button2_Click_1(object sender, EventArgs e)
        {
            SaveOrder();
        }

        private void SaveOrder()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Введите ФИО получателя!");
                textBox1.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Введите номер телефона!");
                textBox2.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Введите адрес доставки!");
                textBox3.Focus();
                return;
            }

            try
            {
                string query = @"
                    INSERT INTO [Orders]
                    (FIO, Phone, [Address], [Comment], OrderItems, Total, OrderDate)
                    VALUES (?, ?, ?, ?, ?, ?, ?)";

                using (OleDbConnection connection =
                    new OleDbConnection(connectionString))
                {
                    using (OleDbCommand command =
                        new OleDbCommand(query, connection))
                    {
                        // Порядок параметров должен совпадать с ? в запросе

                        command.Parameters.Add(
                            "@p1", OleDbType.VarWChar).Value =
                            textBox1.Text.Trim();

                        command.Parameters.Add(
                            "@p2", OleDbType.VarWChar).Value =
                            textBox2.Text.Trim();

                        command.Parameters.Add(
                            "@p3", OleDbType.VarWChar).Value =
                            textBox3.Text.Trim();

                        command.Parameters.Add(
                            "@p4", OleDbType.LongVarWChar).Value =
                            textBox4.Text.Trim();

                        command.Parameters.Add(
                            "@p5", OleDbType.LongVarWChar).Value =
                            string.Join(Environment.NewLine, orderItems);

                        command.Parameters.Add(
                            "@p6", OleDbType.Currency).Value =
                            orderTotal;

                        command.Parameters.Add(
                            "@p7", OleDbType.Date).Value =
                            DateTime.Now;

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Заказ успешно сохранён в базе данных!\nСтатус: Принят",
                    "Успех",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка сохранения заказа:\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Обработчики, которые уже могут быть указаны в Designer.cs

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}