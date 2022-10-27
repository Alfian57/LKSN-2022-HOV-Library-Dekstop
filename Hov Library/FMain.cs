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
    public partial class FMain : Form
    {
        FMasterMember FMasterMember;
        FMasterBook FMasterBook;
        FAllBorrowing FAllBorrowing;
        FNewBorrowing FNewBorrowing;
        FBookList FBookList;
        public FMain()
        {
            InitializeComponent();
            FMasterMember = new FMasterMember();
            FMasterBook = new FMasterBook();
            FAllBorrowing = new FAllBorrowing();
            FNewBorrowing = new FNewBorrowing();
            FBookList = new FBookList();

            FMasterMember.MdiParent = this;
            FMasterBook.MdiParent = this;
            FAllBorrowing.MdiParent = this;
            FNewBorrowing.MdiParent = this;
            FBookList.MdiParent = this;
        }

        private void showForm(Form form)
        {
            FMasterMember.Hide();
            FMasterBook.Hide();
            FAllBorrowing.Hide();
            FNewBorrowing.Hide();
            FBookList.Hide();

            form.Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are Want To Logout ?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                new Form1().Show();
                this.Hide();
            }
        }

        private void memberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(FMasterMember);
        }

        private void bookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(FMasterBook);
        }

        private void newBorrowingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(FNewBorrowing);
        }

        private void allBorrowingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm(FAllBorrowing);
        }

        private void FMain_Load(object sender, EventArgs e)
        {

        }
    }
}
