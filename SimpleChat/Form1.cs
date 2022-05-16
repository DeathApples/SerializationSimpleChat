using System.Windows.Forms;
using BusinessLogic.Control;

namespace SimpleChat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            UpdateDataBaseHandle();
            DataBase.ChangeDataBase += UpdateDataBaseHandle;
        }

        private void UpdateDataBaseHandle()
        {
            textBox2.Text = DataBase.CurrentName;
            label2.Text = DataBase.GetAllPersons();
            richTextBox1.Text = DataBase.GetAllMessages();
            label3.Text = label3.Text.Substring(0, 17) + DataBase.CountMessage.ToString();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataBase.SendMessage(textBox1.Text);
                textBox1.Text = string.Empty;

                UpdateDataBaseHandle();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox2.Text != string.Empty)
            {
                DataBase.EditCurrentPerson(textBox2.Text);

                UpdateDataBaseHandle();
            }
        }
    }
}
