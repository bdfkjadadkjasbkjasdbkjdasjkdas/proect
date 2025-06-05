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
        private string currentUser;

        public Form4(string username)
        {
            InitializeComponent();
            currentUser = username;
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
            listViewBooks.Columns.Add("Взята кем", 150); 
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
                            string takenBy = parts.Length > 5 ? parts[5].Trim() : "";
                            books.Add(new Book(
                                parts[0].Trim(),
                                parts[1].Trim(),
                                parts[2].Trim(),
                                parts[3].Trim(),
                                isAvailable,
                                takenBy
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
                item.SubItems.Add(book.IsAvailable ? "" : book.TakenBy); 
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
                    string status = !book.IsAvailable ? "взята" : "доступна";
                    lines.Add($"{book.Id},{book.Title},{book.Author},{book.Year},{status},{book.TakenBy}");
                }
                File.WriteAllLines(BooksFile, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении книг: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    books[selectedIndex].TakenBy = currentUser; 
                    SaveBooks();
                    RefreshBookList();
                    MessageBox.Show($"Книга успешно взята пользователем {currentUser}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            if (listViewBooks.SelectedItems.Count > 0)
            {
                int selectedIndex = listViewBooks.SelectedIndices[0];
                if (!books[selectedIndex].IsAvailable)
                {
                    if (books[selectedIndex].TakenBy == currentUser || currentUser == "admin")
                    {
                        books[selectedIndex].IsAvailable = true;
                        books[selectedIndex].TakenBy = "";
                        SaveBooks();
                        RefreshBookList();
                        MessageBox.Show("Книга успешно возвращена", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Вы не можете вернуть эту книгу, так как ее взял пользователь {books[selectedIndex].TakenBy}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
    }

    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Year { get; set; }
        public bool IsAvailable { get; set; }
        public string TakenBy { get; set; }

        public Book(string id, string title, string author, string year, bool isAvailable = true, string takenBy = "")
        {
            Id = id;
            Title = title;
            Author = author;
            Year = year;
            IsAvailable = isAvailable;
            TakenBy = takenBy;
        }
    }
}
