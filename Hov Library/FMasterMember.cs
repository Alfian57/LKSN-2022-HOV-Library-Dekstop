using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hov_Library
{
    public partial class FMasterMember : Form
    {
        HovLibraryDataContext dataContext;
        string id;
        public FMasterMember()
        {
            InitializeComponent();
            dataContext = new HovLibraryDataContext();
        }

        private void LoadData()
        {
            dataGridView1.Rows.Clear();
            var members = from m in dataContext.Members
                          where m.deleted_at == null
                          select new
                          {
                              m.id,
                              m.name,
                              m.phone_number,
                              m.email,
                              m.city_of_birth,
                              m.date_of_birth,
                              m.address,
                              m.gender
                          };

            DataGridViewButtonColumn button = new DataGridViewButtonColumn();
            button.Text = "Edit";

            dataGridView1.DataSource = members;
            dataGridView1.Columns.Add(button);
        }

        private void FMasterMember_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                txtName.Enabled = true;
                txtPhone.Enabled = true;
                txtEmail.Enabled = true;
                txtAddress.Enabled = true;
                txtCity.Enabled = true;
                dateBirth.Enabled = true;
                button1.Enabled = true;

                id = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

                var member = (from m in dataContext.Members
                              where m.id == int.Parse(id)
                              select m).FirstOrDefault();

                txtName.Text = member.name;
                txtPhone.Text = member.phone_number;
                txtEmail.Text = member.email;
                txtAddress.Text = member.address;
                txtCity.Text = member.city_of_birth;
                DateTime date = Convert.ToDateTime(member.date_of_birth);
                dateBirth.Value = date;

                if (member.gender == "Male")
                {
                    radioButton1.Checked = true;
                } else
                {
                    radioButton2.Checked = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPhone.Text == "" || txtEmail.Text == "" || txtAddress.Text == "" || txtCity.Text == "")
            {
                MessageBox.Show("Field Still Blank");
                return;
            }

            var member = (from m in dataContext.Members
                          where m.id == int.Parse(id)
                          select m).FirstOrDefault();

            member.name = txtName.Text;
            member.phone_number = txtPhone.Text;
            member.email = txtEmail.Text;
            member.address = txtAddress.Text;
            member.city_of_birth = txtCity.Text;
            member.date_of_birth = dateBirth.Value;

            if (radioButton1.Checked)
            {
                member.gender = "Male";
            } else
            {
                member.gender = "Female";
            }

            LoadData();
            MessageBox.Show("Data Updated");
            dataContext.SubmitChanges();
        }
    }
}
