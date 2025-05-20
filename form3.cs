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
    public partial class Form3 : Form
    {
        private Book _book;
        public Form3()
        {
            InitializeComponent();
        }

        public Form3(Book Form3) : this()
        {
            _book = Form3;
            txtTitle.Text = Form3.Title;
            txtAuthor.Text = Form3.Author;
            txtISBN.Text = Form3.ISBN;
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var books = DataService.LoadBooks();

                if (_book == null)
                {
                    books.Add(new Book
                    {
                        Title = txtTitle.Text,
                        Author = txtAuthor.Text,
                        ISBN = txtISBN.Text,
                        IsAvailable = true
                    });
                }
                else 
                {
                    _book.Title = txtTitle.Text;
                    _book.Author = txtAuthor.Text;
                    _book.ISBN = txtISBN.Text;
                }

                DataService.SaveBooks(books);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
