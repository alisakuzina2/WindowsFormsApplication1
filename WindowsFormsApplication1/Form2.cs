using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        private decimal total = 0;
        public Form2()
        {
            InitializeComponent();

            button1.Click += button1_Click;
            button2.Click += button2_Click;

            listBox1.Items.Clear();

            listBox1.Items.Add("Пицца «Маргарита» — 450 ₽");
            listBox1.Items.Add("Бургер классический — 320 ₽");
            listBox1.Items.Add("Картофель фри — 150 ₽");
            listBox1.Items.Add("Паста «Карбонара» — 390 ₽");
            listBox1.Items.Add("Цезарь с курицей — 350 ₽");

            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 20;
            numericUpDown1.Value = 1;

            label2.Text = "Итого: 0 ₽";

            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            listView1.Columns.Clear();

            listView1.Columns.Add("Блюдо", 180);
            listView1.Columns.Add("Количество", 100);
            listView1.Columns.Add("Сумма", 100);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Сначала выберите блюдо!");
                return;
            }

            string selectedDish = listBox1.SelectedItem.ToString();
            string dishName = selectedDish.Split(new string[] { " — " }, StringSplitOptions.None)[0];

            decimal price;

            if (dishName.Contains("Маргарита"))
                price = 450;
            else if (dishName.Contains("Бургер"))
                price = 320;
            else if (dishName.Contains("Картофель фри"))
                price = 150;
            else if (dishName.Contains("Карбонара"))
                price = 390;
            else
                price = 350;

            int quantity = (int)numericUpDown1.Value;
            decimal sum = price * quantity;

            ListViewItem item = new ListViewItem(dishName);
            item.SubItems.Add(quantity.ToString());
            item.SubItems.Add(sum.ToString("0.##") + " ₽");

            listView1.Items.Add(item);

            total += sum;
            label2.Text = "Итого: " + total.ToString("0.##") + " ₽";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Сначала добавьте блюдо в заказ!");
                return;
            }

            List<string> orderItems = new List<string>();

            foreach (ListViewItem item in listView1.Items)
            {
                string dish = item.Text;
                string quantity = item.SubItems[1].Text;
                string sum = item.SubItems[2].Text;

                orderItems.Add(dish + " — " + quantity + " шт. — " + sum);
            }

            Form3 form3 = new Form3(orderItems, total);
            form3.ShowDialog(this);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
