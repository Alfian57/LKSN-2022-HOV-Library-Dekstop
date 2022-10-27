using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hov_Library
{
    public partial class Form1 : Form
    {
        HovLibraryDataContext dataContext;
        public Form1()
        {
            InitializeComponent();
            dataContext = new HovLibraryDataContext();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("Field Still Empty");
                    return;
                }

                var employee = (from U in dataContext.Employees
                                where U.email == textBox1.Text
                                select U).FirstOrDefault();

                if (employee != null)
                {
                    if (employee.password == stringToSha(textBox2.Text))
                    {
                        new FMain().Show();
                        this.Hide();
                        return;
                    }
                }

                MessageBox.Show("Employee Not Found");
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string stringToSha(String s)
        {
            StringBuilder sb = new StringBuilder();

            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(s));

                for(int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }
}
