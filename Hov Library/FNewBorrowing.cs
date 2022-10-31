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
    public partial class FNewBorrowing : Form
    {
        HovLibraryDataContext dataContext;
        public FNewBorrowing()
        {
            InitializeComponent();
            dataContext = new HovLibraryDataContext();
        }

        private void FNewBorrowing_Load(object sender, EventArgs e)
        {
            AutoCompleteStringCollection titleAutoCom = new AutoCompleteStringCollection();
            AutoCompleteStringCollection nameAutoCom = new AutoCompleteStringCollection();

            var books = from b in dataContext.Books
                         where b.deleted_at == null
                         select b;

            var members = from b in dataContext.Members
                        where b.deleted_at == null
                        select b;

            foreach (var book in books)
            {
                titleAutoCom.Add(book.title);
            }

            foreach (var member in members)
            {
                nameAutoCom.Add(member.name);
            }

            txtTitle.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTitle.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTitle.AutoCompleteCustomSource = titleAutoCom;

            txtMemberName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtMemberName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtMemberName.AutoCompleteCustomSource = nameAutoCom;


            var borrows = from d in dataContext.BookDetails
                         join l in dataContext.Locations
                         on d.location_id equals l.id
                         where d.deleted_at == null
                         select new
                         {
                             d.id,
                             d.code,
                             l.name,
                         };

            DataGridViewCheckBoxColumn check = new DataGridViewCheckBoxColumn();
            dataGridView1.Columns.Add(check);
            dataGridView1.ReadOnly = false;
            
            foreach (var borrow in borrows)
            {
                int num = dataGridView1.Rows.Add();
                dataGridView1.Rows[num].Cells[0].Value = borrow.id;
                dataGridView1.Rows[num].Cells[1].Value = borrow.code;
                dataGridView1.Rows[num].Cells[2].Value = borrow.name;
            }
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            var borrows = from b in dataContext.Books
                          join d in dataContext.BookDetails
                          on b.id equals d.book_id
                          join l in dataContext.Locations
                          on d.location_id equals l.id
                          where d.deleted_at == null
                          select new
                          {
                              b.title,
                              d.id,
                              d.code,
                              l.name
                          };

            borrows = borrows.Where(b => b.title.Contains(txtTitle.Text));

            foreach (var borrow in borrows)
            {
                int num = dataGridView1.Rows.Add();
                dataGridView1.Rows[num].Cells[0].Value = borrow.id;
                dataGridView1.Rows[num].Cells[1].Value = borrow.code;
                dataGridView1.Rows[num].Cells[2].Value = borrow.name;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtMemberName.Text == "")
            {
                MessageBox.Show("Field Still Empty");
                return;
            }

            var member = (from m in dataContext.Members.Where(m => m.name == txtMemberName.Text) select m).FirstOrDefault();
            if (member == null)
            {
                MessageBox.Show("Member Not Found");
                return;
            }
            List<int> bookIdList = new List<int>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells[4].Value))
                {
                    bookIdList.Add(Convert.ToInt32(row.Cells[0].Value));
                }
            }

            var f = new FAllBorrowing();
            f.Data = bookIdList;
            f.Show();
            f.memberId = member.id;
            this.Hide();
        }

        private void txtTitle_Enter(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
