using Microsoft.VisualBasic.ApplicationServices;
using System.Text;
using System.Text.Json;
using WinFormsApp11;

namespace WinFormsApp11
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
            txtRole.Clear();
        }

        private bool UserExists(string username)
        {
            string filePath = "users.txt";

            if (!File.Exists(filePath))
                return false;

            string[] existingUsers = File.ReadAllLines(filePath);
            foreach (string user in existingUsers)
            {
                string[] parts = user.Split(',');
                if (parts.Length > 0 && parts[0].Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private void RegisterUser(string username, string password, string role)
        {
            string filePath = "users.txt";
            string userRecord = $"{username},{password},{role}";

            File.AppendAllText(filePath, userRecord + Environment.NewLine);
        }

        private void btnRegister_Click(object sender, EventArgs e)
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
                    ClearRegistrationFields(); 
                    return; 
                }

                RegisterUser(username, password, role);

                MessageBox.Show("Регистрация прошла успешно! Теперь вы можете войти.",
                                "Успех",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                Form2 Forms = new Form2();
                Forms.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}",
                                "Ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
    }
}
