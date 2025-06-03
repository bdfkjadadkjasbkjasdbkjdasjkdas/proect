using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WinFormsApp11;

namespace WinFormsApp11
{
    public partial class Form5 : Form
    {
        private const string BooksFile = "books.txt";
        private List<Book> books = new List<Book>();
        private int selectedBookIndex;

        public Form5()
        {
            InitializeComponent();
            SetupListView();
            LoadBooks();
        }

        private void SetupListView()
        {
            listViewBooks.View = View.Details;
            listViewBooks.Columns.Add("Номер", 100);
            listViewBooks.Columns.Add("Название", 200);
            listViewBooks.Columns.Add("Автор", 150);
            listViewBooks.Columns.Add("Год", 80);
            listViewBooks.Columns.Add("Статус", 100);
        }

        private void LoadBooks()
        {
            try
            {
                books.Clear();
                listViewBooks.Items.Clear();

                if (File.Exists(BooksFile))
                {
                    foreach (string line in File.ReadAllLines(BooksFile))
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length >= 4)
                        {
                            bool available = parts[4].Trim() != "взята";
                            books.Add(new Book(
                                parts[0].Trim(),
                                parts[1].Trim(),
                                parts[2].Trim(),
                                parts[3].Trim(),
                                available));
                        }
                    }
                    ShowBooks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }

        private void ShowBooks()
        {
            listViewBooks.Items.Clear();
            foreach (Book book in books)
            {
                ListViewItem item = new ListViewItem(book.Id);
                item.SubItems.Add(book.Title);
                item.SubItems.Add(book.Author);
                item.SubItems.Add(book.Year);
                item.SubItems.Add(book.IsAvailable ? "Доступна" : "Взята");
                item.BackColor = book.IsAvailable ? Color.LightGreen : Color.LightPink;
                listViewBooks.Items.Add(item);
            }
        }

        private void SaveBooks()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (Book book in books)
                {
                    string status = book.IsAvailable ? "доступна" : "взята";
                    lines.Add($"{book.Id},{book.Title},{book.Author},{book.Year},{status}");
                }
                File.WriteAllLines(BooksFile, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void listViewBooks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewBooks.SelectedItems.Count > 0)
            {
                selectedBookIndex = listViewBooks.SelectedIndices[0];
                Book selected = books[selectedBookIndex];

                txtId.Text = selected.Id;
                txtTitle.Text = selected.Title;
                txtAuthor.Text = selected.Author;
                txtYear.Text = selected.Year;
            }
        }

        private void ClearFields()
        {
            txtId.Clear();
            txtTitle.Clear();
            txtAuthor.Clear();
            txtYear.Clear();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBooks();
            ClearFields();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) ||
    string.IsNullOrWhiteSpace(txtTitle.Text) ||
    string.IsNullOrWhiteSpace(txtAuthor.Text) ||
    string.IsNullOrWhiteSpace(txtYear.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (books.Exists(b => b.Id == txtId.Text))
            {
                MessageBox.Show("Книга с таким номером уже есть!");
                return;
            }

            Book newBook = new Book(txtId.Text, txtTitle.Text, txtAuthor.Text, txtYear.Text, true);
            books.Add(newBook);
            SaveBooks();
            ShowBooks();
            ClearFields();
            MessageBox.Show("Книга добавлена!");
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtId.Text) ||
                string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtAuthor.Text) ||
                string.IsNullOrWhiteSpace(txtYear.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            books[selectedBookIndex].Id = txtId.Text;
            books[selectedBookIndex].Title = txtTitle.Text;
            books[selectedBookIndex].Author = txtAuthor.Text;
            books[selectedBookIndex].Year = txtYear.Text;

            SaveBooks();
            ShowBooks();
            ClearFields();
            selectedBookIndex = 0;
            MessageBox.Show("Изменения сохранены!");
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {

            if (MessageBox.Show("Удалить эту книгу?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                books.RemoveAt(selectedBookIndex);
                SaveBooks();
                ShowBooks();
                ClearFields();
                selectedBookIndex = 0;
                MessageBox.Show("Книга удалена!");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Удалить эту книгу?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                books.RemoveAt(selectedBookIndex);
                SaveBooks();
                ShowBooks();
                ClearFields();
                selectedBookIndex = 0;
                MessageBox.Show("Книга удалена!");
            }
        }
    }
}
