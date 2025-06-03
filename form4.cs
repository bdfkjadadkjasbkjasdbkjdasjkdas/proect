using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp11
{
    public partial class Form4 : Form
    {
        private const string BooksFile = "books.txt";
        private List<Book> books = new List<Book>();

        public Form4()
        {
            InitializeComponent();
            SetupListViewColumns();
            LoadBooks();
        }

        private void SetupListViewColumns()
        {
            listViewBooks.View = View.Details;
            listViewBooks.Columns.Clear();
            listViewBooks.Columns.Add("Номер", 70);
            listViewBooks.Columns.Add("Название", 250);
            listViewBooks.Columns.Add("Автор", 200);
            listViewBooks.Columns.Add("Год", 150);
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
                    string[] lines = File.ReadAllLines(BooksFile);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length >= 4)
                        {
                            bool isAvailable = parts[4].Trim() != "взята";
                            books.Add(new Book(
                                parts[0].Trim(),
                                parts[1].Trim(),
                                parts[2].Trim(),
                                parts[3].Trim(),
                                isAvailable
                            ));
                        }
                    }
                    RefreshBookList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке книг: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshBookList()
        {
            listViewBooks.Items.Clear();
            foreach (var book in books)
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
                foreach (var book in books)
                {
                    lines.Add($"{book.Id},{book.Title},{book.Author},{book.Year},{(!book.IsAvailable ? "взята" : "доступна")}");
                }
                File.WriteAllLines(BooksFile, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении книг: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            if (listViewBooks.SelectedItems.Count > 0)
            {
                int selectedIndex = listViewBooks.SelectedIndices[0];
                if (!books[selectedIndex].IsAvailable)
                {
                    books[selectedIndex].IsAvailable = true;
                    SaveBooks();
                    RefreshBookList();
                }
                else
                {
                    MessageBox.Show("Эта книга уже доступна!", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnTakeBook_Click(object sender, EventArgs e)
        {
            if (listViewBooks.SelectedItems.Count > 0)
            {
                int selectedIndex = listViewBooks.SelectedIndices[0];
                if (books[selectedIndex].IsAvailable)
                {
                    books[selectedIndex].IsAvailable = false;
                    SaveBooks();
                    RefreshBookList();
                }
                else
                {
                    MessageBox.Show("Эта книга уже взята!", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    public class Book
    { 
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Year { get; set; }
        public bool IsAvailable { get; set; }

        public Book(string id, string title, string author, string year, bool isAvailable = true)
        {
            Id = id;
            Title = title;
            Author = author;
            Year = year;
            IsAvailable = isAvailable;
        }
    }
}
