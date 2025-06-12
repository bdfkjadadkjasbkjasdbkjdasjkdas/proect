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
        private string currentUser;

        public Form5(string username)
        {
            InitializeComponent();
            currentUser = username;
            SetupListView();
            LoadBooks();
        }

        private void SetupListView()
        {
            listViewBooks.View = View.Details;
            listViewBooks.Columns.Clear();
            listViewBooks.Columns.Add("Номер", 100);
            listViewBooks.Columns.Add("Название", 200);
            listViewBooks.Columns.Add("Автор", 150);
            listViewBooks.Columns.Add("Год", 80);
            listViewBooks.Columns.Add("Статус", 100);
            listViewBooks.Columns.Add("Взята кем", 150);
            listViewBooks.Columns.Add("Дата взятия", 100);
            listViewBooks.Columns.Add("Вернуть до", 100);
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
                            bool available = parts.Length > 4 ? parts[4].Trim() != "взята" : true;
                            string takenBy = parts.Length > 5 ? parts[5].Trim() : "";

                            DateTime? takenDate = null;
                            if (parts.Length > 6 && !string.IsNullOrEmpty(parts[6]))
                                takenDate = DateTime.Parse(parts[6]);

                            DateTime? dueDate = null;
                            if (parts.Length > 7 && !string.IsNullOrEmpty(parts[7]))
                                dueDate = DateTime.Parse(parts[7]);

                            books.Add(new Book(
                                parts[0].Trim(),
                                parts[1].Trim(),
                                parts[2].Trim(),
                                parts[3].Trim(),
                                available,
                                takenBy,
                                takenDate,
                                dueDate));
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
                item.SubItems.Add(book.IsAvailable ? "" : book.TakenBy);
                item.SubItems.Add(book.IsAvailable ? "" : book.TakenDate?.ToString("dd.MM.yyyy"));
                item.SubItems.Add(book.IsAvailable ? "" : book.DueDate?.ToString("dd.MM.yyyy"));
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
                    string takenDateStr = book.TakenDate?.ToString("yyyy-MM-dd") ?? "";
                    string dueDateStr = book.DueDate?.ToString("yyyy-MM-dd") ?? "";
                    lines.Add($"{book.Id},{book.Title},{book.Author},{book.Year},{status},{book.TakenBy},{takenDateStr},{dueDateStr}");
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
                MessageBox.Show("Заполните все поля!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtId.Text, out int id))
            {
                MessageBox.Show("Номер книги должен быть числом!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtId.Focus();
                return;
            }

            if (!int.TryParse(txtYear.Text, out int year) || year < 1000 || year > DateTime.Now.Year + 1)
            {
                MessageBox.Show($"Год должен быть числом между 1000 и {DateTime.Now.Year + 1}!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtYear.Focus();
                return;
            }

            if (books.Exists(b => b.Id == txtId.Text))
            {
                MessageBox.Show("Книга с таким номером уже есть!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtId.Focus();
                return;
            }

            Book newBook = new Book(txtId.Text, txtTitle.Text, txtAuthor.Text, txtYear.Text, true);
            books.Add(newBook);
            SaveBooks();
            ShowBooks();
            ClearFields();
            MessageBox.Show("Книга добавлена!", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listViewBooks.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите книгу для редактирования!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtId.Text) ||
                string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtAuthor.Text) ||
                string.IsNullOrWhiteSpace(txtYear.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtId.Text, out int id))
            {
                MessageBox.Show("Номер книги должен быть числом!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtId.Focus();
                return;
            }

            if (!int.TryParse(txtYear.Text, out int year) || year < 1000 || year > DateTime.Now.Year + 1)
            {
                MessageBox.Show($"Год должен быть числом между 1000 и {DateTime.Now.Year + 1}!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtYear.Focus();
                return;
            }

            string originalId = books[selectedBookIndex].Id;
            if (originalId != txtId.Text && books.Exists(b => b.Id == txtId.Text))
            {
                MessageBox.Show("Книга с таким номером уже есть!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtId.Focus();
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
            MessageBox.Show("Изменения сохранены!", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
