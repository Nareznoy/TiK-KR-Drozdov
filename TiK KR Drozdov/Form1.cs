using System;
using System.Linq;
using System.Windows.Forms;

namespace TiK_KR_Drozdov
{
    public partial class Form1 : Form
    {
        HuffmanTree huffmanTree;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите строку!", "Ошибка!");
                return;
            }
            dataGridView1.Rows.Clear();

            huffmanTree = new HuffmanTree(textBox1.Text);

            // Вывод кодов на интерфейс
            for (int i = 0; i < huffmanTree.CodesDictionary.Count; i++)
            {
                dataGridView1.Rows.Add(huffmanTree.CodesDictionary.Keys.ElementAt(i), huffmanTree.CodesDictionary.Values.ElementAt(i));
            }

            textBox2.Text = huffmanTree.Encode(textBox1.Text);
            textBox5.Text = huffmanTree.GetEntropy.ToString();
            textBox6.Text = huffmanTree.GetAverageLength.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.Text = huffmanTree.Decode(textBox3.Text);
        }
    }
}
