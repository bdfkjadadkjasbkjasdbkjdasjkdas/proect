using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp11
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            UpdateBookList();

            btnManageUsers.Visible = Form1.CurrentUser.Role == "Admin";
            btnAddBook.Visible = Form1.CurrentUser.Role != "Client";
        }

        private void UpdateBookList()
        {
            dgvBooks.DataSource = DataService.LoadBooks();
        }

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            new Form3().ShowDialog();
            UpdateBookList();
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            new Form4().ShowDialog();
        }
    }
}
