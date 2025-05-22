using Microsoft.VisualBasic.ApplicationServices;
using System.Text;
using System.Text.Json;

namespace WinFormsApp5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ClearRegistrationFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
        }

        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string role = txtRole.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Пожалуйста, заполните все поля",
                                "Ошибка ввода",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов",
                                "Ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (UserExists(username))
                {
                    MessageBox.Show("Пользователь с таким именем уже существует",
                                    "Ошибка",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                RegisterUser(username, password, role);

                MessageBox.Show("Регистрация прошла успешно! Теперь вы можете войти.",
                                "Успех",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                Form2 Forms = new Form2();
                Forms.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}",
                                "Ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
        private bool UserExists(string username)
        {
            List<string> existingUsers = new List<string> { };

            return existingUsers.Contains(username.ToLower());
        }
        private void RegisterUser(string username, string password, string role)
        {
            string filePath = "users.txt";

            string userRecord = $"{username},{password},{role}";

            if (File.Exists(filePath))
            {
                string[] existingUsers = File.ReadAllLines(filePath);
                foreach (string user in existingUsers)
                {
                    string[] parts = user.Split(',');
                    if (parts[0] == username)
                    {
                        MessageBox.Show("Пользователь с таким именем уже существует!");
                        return;
                    }
                }
            }
            File.AppendAllText(filePath, userRecord + Environment.NewLine);
            MessageBox.Show("Регистрация прошла успешно!");
        }
    }
}
