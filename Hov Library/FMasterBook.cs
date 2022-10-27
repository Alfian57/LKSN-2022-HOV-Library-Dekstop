using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Hov_Library
{
    public partial class FMasterBook : Form
    {
        HovLibraryDataContext dataContext;
        string id;
        public FMasterBook()
        {
            InitializeComponent();
            dataContext = new HovLibraryDataContext();
        }

        private void loadData()
        {
            var books = from b in dataContext.Books
                        join l in dataContext.Languages
                        on b.language_id equals l.id
                        join p in dataContext.Publishers
                        on b.publisher_id equals p.id
                        where b.deleted_at == null
                        select new
                        {
                            b.id,
                            l.long_text,
                            b.title,
                            b.isbn,
                            b.isbn13,
                            b.authors,
                            p.name,
                            b.publication_date,
                            b.number_of_pages,
                            b.ratings_count,
                        };

            dataGridView1.DataSource = books;
        }

        private void FMasterBook_Load(object sender, EventArgs e)
        {
            var langs = from l in dataContext.Languages
                        where l.deleted_at == null
                        select l;

            foreach (var lang in langs)
            {
                cbLang.Items.Add(lang.long_text);
            }

            loadData();

            DataGridViewButtonColumn show = new DataGridViewButtonColumn();
            DataGridViewButtonColumn edit = new DataGridViewButtonColumn();
            DataGridViewButtonColumn delete = new DataGridViewButtonColumn();
            show.HeaderText = "Show";
            edit.HeaderText = "Edit";
            delete.HeaderText = "Delete";

            dataGridView1.Columns.Add(show);
            dataGridView1.Columns.Add(edit);
            dataGridView1.Columns.Add(delete);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtKeyword.Text == "")
            {
                MessageBox.Show("Field Still Empty");
            }

            dataGridView1.Rows.Clear();
            var data = from b in dataContext.Books
                       join l in dataContext.Languages
                       on b.language_id equals l.id
                       join p in dataContext.Publishers
                       on b.publisher_id equals p.id
                       where b.deleted_at == null
                       select new
                       {
                           b.id,
                           l.long_text,
                           b.title,
                           b.isbn,
                           b.isbn13,
                           b.authors,
                           p.name,
                           b.publication_date,
                           b.number_of_pages,
                           b.ratings_count,
                       };

            if (cbSearch.Text == "Title")
            {
                data = data.Where(d => d.title.Contains(txtKeyword.Text));
            } else if (cbSearch.Text == "Author")
            {
                data = data.Where(d => d.authors.Contains(txtKeyword.Text));
            } else if (cbSearch.Text == "Publisher")
            {
                data = data.Where(d => d.name.Contains(txtKeyword.Text));
            }

            dataGridView1.DataSource = data;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            var data = from b in dataContext.Books
                       join l in dataContext.Languages
                       on b.language_id equals l.id
                       join p in dataContext.Publishers
                       on b.publisher_id equals p.id
                       where b.deleted_at == null
                       select new
                       {
                           b.id,
                           l.long_text,
                           b.title,
                           b.isbn,
                           b.isbn13,
                           b.authors,
                           p.name,
                           b.publication_date,
                           b.number_of_pages,
                           b.ratings_count,
                       };

            if (dateStart.Value != null)
            {
                data = data.Where(d => d.publication_date >= dateStart.Value);
            }
            if (dateEnd.Value != null)
            {
                data = data.Where(d => d.publication_date <= dateEnd.Value);
            }

            if (txtCountStart.Text != "")
            {
                int num = Convert.ToInt32(txtCountStart.Text);
                data = data.Where(d => d.number_of_pages >= num);
            }
            if (txtCountEnd.Text != "")
            {
                int num = Convert.ToInt32(txtCountEnd.Text);
                data = data.Where(d => d.number_of_pages <= num);
            }

            if (txtRatingStart.Text != "")
            {
                int num = Convert.ToInt32(txtRatingStart.Text);
                data = data.Where(d => d.number_of_pages >= num);
            }
            if (txtRatingEnd.Text != "")
            {
                int num = Convert.ToInt32(txtRatingEnd.Text);
                data = data.Where(d => d.number_of_pages <= num);
            }

            dataGridView1.DataSource = data;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            id = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();

            if (e.ColumnIndex == 0)
            {
                FBookList f = new FBookList();
                f.Show();
                this.Hide();
            } else if (e.ColumnIndex == 1)
            {
                txtLang.Enabled = true;
                txtTitle.Enabled = true;
                txtIsbn.Enabled = true;
                txtIsbn13.Enabled = true;
                txtAuthor.Enabled = true;
                txtPublisher.Enabled = true;
                txtPage.Enabled = true;
                txtRatings.Enabled = true;
                datePublish.Enabled = true;
                btnSave.Enabled = true;

                var book = (from b in dataContext.Books
                            join l in dataContext.Languages
                            on b.language_id equals l.id
                            join p in dataContext.Publishers
                            on b.publisher_id equals p.id
                            where b.deleted_at == null
                            select new
                            {
                                b.id,
                                l.long_text,
                                b.title,
                                b.isbn,
                                b.isbn13,
                                b.authors,
                                p.name,
                                b.publication_date,
                                b.number_of_pages,
                                b.ratings_count,
                            }).FirstOrDefault();

                txtLang.Text = book.long_text;
                txtTitle.Text = book.title;
                txtIsbn.Text = book.isbn;
                txtIsbn13.Text = book.isbn13;
                txtAuthor.Text = book.authors;
                txtPublisher.Text = book.name;
                txtPage.Text = book.number_of_pages.ToString();
                txtRatings.Text = book.ratings_count.ToString();
                datePublish.Value = book.publication_date;
            } else if (e.ColumnIndex == 2)
            {
                if (MessageBox.Show("Are You Want To Delete This Book ?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var book = (from b in dataContext.Books
                                where b.id == Convert.ToInt32(id)
                                select b).FirstOrDefault();

                    book.deleted_at = DateTime.Now;
                    loadData();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (
                txtLang.Text == "" ||
                txtTitle.Text == "" ||
                txtIsbn.Text == "" ||
                txtIsbn13.Text == "" ||
                txtAuthor.Text == "" ||
                txtPublisher.Text == "" ||
                txtPage.Text == "" ||
                txtRatings.Text == ""
                )
            {
                MessageBox.Show("Field Still Empty");
                return;
            }
            int pages = 0;
            int ratings = 0;
            if (!int.TryParse(txtPage.Text, out pages))
            {
                MessageBox.Show("Page Must A Number");
            }
            if (!int.TryParse(txtPage.Text, out ratings))
            {
                MessageBox.Show("Ratings Must A Number");
            }

            var language = (from l in dataContext.Languages
                            where l.deleted_at == null
                            select l).FirstOrDefault();
            if (language == null)
            {
                MessageBox.Show("Language Not Valid");
                return;
            }
            var publisher = (from p in dataContext.Publishers
                            where p.deleted_at == null
                            select p).FirstOrDefault();
            if (publisher == null)
            {
                MessageBox.Show("Publisher Not Valid");
                return;
            }


            var book = (from b in dataContext.Books
                        where b.id == Convert.ToInt32(id)
                        select b).FirstOrDefault();

            book.title = txtTitle.Text;
            book.isbn = txtIsbn.Text;
            book.isbn13 = txtIsbn13.Text;
            book.authors = txtAuthor.Text;
            book.publisher_id = publisher.id;
            book.language_id = language.id;
            book.publication_date = datePublish.Value;
            book.number_of_pages = pages;
            book.ratings_count = ratings;

            dataContext.SubmitChanges();

            MessageBox.Show("Data Updated");
        }
    }
}
