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
    public partial class FAllBorrowing : Form
    {
        public List<int> Data { get; set; } = new List<int>();
        public int memberId { get; set; } = 0;
        HovLibraryDataContext dataContext;
        public FAllBorrowing()
        {
            InitializeComponent();
            dataContext = new HovLibraryDataContext();
        }

        private void loadData()
        {
            var borrow = from b in dataContext.Borrowings
                         join d in dataContext.BookDetails
                         on b.bookdetails_id equals d.id
                         join m in dataContext.Members
                         on b.member_id equals m.id
                         join book in dataContext.Books
                         on d.book_id equals book.id
                         where b.deleted_at == null &
                         b.member_id == memberId
                         select new
                         {
                             b.id,
                             m.name,
                             book.title,
                             d.code,
                             b.borrow_date,
                             b.return_date,
                             b.fine,
                             Status = (Convert.ToDateTime(b.borrow_date) - Convert.ToDateTime(b.return_date)).TotalDays > 7 ? "Late" : "Returned"
                         };

            dataGridView1.Rows.Clear();
            dataGridView1.DataSource = borrow;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            var borrow = from b in dataContext.Borrowings
                         join d in dataContext.BookDetails
                         on b.bookdetails_id equals d.id
                         join m in dataContext.Members
                         on b.member_id equals m.id
                         join book in dataContext.Books
                         on d.book_id equals book.id
                         where b.deleted_at == null
                         select new
                         {
                             b.id,
                             m.name,
                             book.title,
                             d.code,
                             b.borrow_date,
                             b.return_date,
                             b.fine
                         };

            borrow = borrow.Where(b => b.borrow_date > dateStart.Value);
            borrow = borrow.Where(b => b.borrow_date < dateEnd.Value);

            if (cbStatus.Text == "Ongoing")
            {
                borrow = borrow.Where(b => b.return_date == null);
            } else if (cbStatus.Text == "Late")
            {
                borrow = borrow.Where(b => b.borrow_date <= b.return_date.Value.AddDays(7));
            } else
            {
                borrow = borrow.Where(b => b.return_date != null);
            }

            dataGridView1.DataSource = borrow;
        }

        private void FAllBorrowing_Load(object sender, EventArgs e)
        {
            DataGridViewButtonColumn button = new DataGridViewButtonColumn();
            button.Text = "Return";
            button.UseColumnTextForButtonValue = true;

            loadData();
            dataGridView1.Columns.Add(button);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                var borrow = (from b in dataContext.Borrowings
                              where b.id == id
                              select b).FirstOrDefault();

                var days = (Convert.ToDateTime(borrow.borrow_date) - Convert.ToDateTime(borrow.return_date)).TotalDays;
                var fine = days * 1000;
                borrow.fine = Convert.ToDecimal(fine);

                dataContext.SubmitChanges();

                loadData();

                MessageBox.Show("Data Updated");
            }
        }
    }
}
