using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRC
{
    public partial class Form1 : Form
    {
        private long crc;
        private long inputCode;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите полином!", "Ошибка!");
                return;
            }
            CRC.SetPoly = Convert.ToInt64(textBox1.Text, 2);

            if (textBox2.Text == "")
            {
                MessageBox.Show("Введите выходные данные!", "Ошибка!");
                return;
            }


            try
            {
                inputCode = Convert.ToInt64(textBox2.Text, 16);
            }
            catch
            {
                MessageBox.Show("Неверный формат входных данных!", "Ошибка!");
                return;
            }
            crc = CRC.CalculateCRC(inputCode);
            textBox3.Text = Convert.ToString(crc, 2);
            //textBox5.Text = Convert.ToString(CRC.GetMessagePlusCRC(inputCode, crc), 2);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                MessageBox.Show("Введите входные данные для проверки!", "Ошибка!");
                return;
            }

            long inputMessage;
            try
            {
                inputMessage = Convert.ToInt64(textBox2.Text, 16);
            }
            catch
            {
                MessageBox.Show("Неверный формат входных данных!", "Ошибка!");
                return;
            }

            if (CRC.CheckMessage(inputMessage, crc))
            {
                label5.Text = "Результат: Данные приняты верно!";
            }
            else
            {
                label5.Text = "Результат: Данные приняты с ошибкой!";
            }
        }
    }
}
