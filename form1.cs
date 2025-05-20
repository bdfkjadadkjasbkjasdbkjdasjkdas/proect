using Microsoft.VisualBasic.ApplicationServices;
using System.Text.Json;

namespace WinFormsApp11
{
    public partial class Form1 : Form
    {

        public static User CurrentUser;


        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var users = DataService.LoadUsers();
            var user = users.FirstOrDefault(u => u.Username == txtUsername.Text && u.Password == txtPassword.Text);

            if (user != null)
            {
                CurrentUser = user;
                this.Hide();
                new Form2().Show();
            }
            else
            {
                MessageBox.Show("Неверные данные!");
            }
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } 
    }

    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public bool IsAvailable { get; set; }
    }

    public static class DataService
    {
        private static string usersFile = "users.json";
        private static string booksFile = "books.json";

        public static List<User> LoadUsers()
        {
            if (File.Exists(usersFile))
            {
                return JsonSerializer.Deserialize<List<User>>(File.ReadAllText(usersFile));
            }
            return new List<User>();
        }

        public static List<Book> LoadBooks()
        {
            if (File.Exists(booksFile))
            {
                return JsonSerializer.Deserialize<List<Book>>(File.ReadAllText(booksFile));
            }
            return new List<Book>();
        }

        public static void SaveUsers(List<User> users)
        {
            File.WriteAllText(usersFile, JsonSerializer.Serialize(users));
        }

        public static void SaveBooks(List<Book> books)
        {
            File.WriteAllText(booksFile, JsonSerializer.Serialize(books)); ;
        }
    }
}
