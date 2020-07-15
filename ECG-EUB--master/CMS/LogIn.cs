using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMS
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void enterButton_Click_1(object sender, EventArgs e)
        {
            if (uNameTextBox.Text == "obaidur" && passwordTextBox.Text == "12345")
            {
                this.Hide();

                var form2 = new MainForm();
                form2.Closed += (s, args) => this.Close();
                form2.Show();
            }
            else if (uNameTextBox.Text=="" && passwordTextBox.Text=="")
            {
                this.Hide();

                var form2 = new MainForm();
                form2.Closed += (s, args) => this.Close();
                form2.Show();
            }
            else
            {
                MessageBox.Show("Enter Right Username and Password");
            }
        }
    }
}
