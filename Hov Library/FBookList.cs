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
    public partial class FBookList : Form
    {
        HovLibraryDataContext dataContext;
        public int[] bookId { get; set; } = new int[100];
        public FBookList()
        {
            InitializeComponent();
            dataContext =  new HovLibraryDataContext();
        }

        private void FBookList_Load(object sender, EventArgs e)
        {
            var locations = from l in dataContext.Locations
                            where l.deleted_at == null
                            select l;

            foreach (var location in locations)
            {
                cbLang.Items.Add(location.name);
            }

            for (int i = 0; i < bookId.Length; i++)
            {
                var book = (from b in dataContext.BookDetails
                            join l in dataContext.Locations
                            on b.location_id equals l.id
                           where b.book_id == bookId[i]
                           select new
                           {
                               b.book_id,
                               b.code,
                               l.name
                           }).FirstOrDefault();

                int num = dataGridView1.Rows.Add();
                dataGridView1.Rows[num].Cells[0].Value = book.book_id;
                dataGridView1.Rows[num].Cells[1].Value = book.code;
                dataGridView1.Rows[num].Cells[2].Value = book.name;
            }
        }

        private void cbLang_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtCode.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
