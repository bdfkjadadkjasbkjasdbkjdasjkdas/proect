using Microsoft.VisualBasic.ApplicationServices;
using System.Text;
using System.Text.Json;
using WinFormsApp5;

namespace WinFormsApp5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cmbRole.Items.AddRange(new string[] { "admin", "worker", "client" });
            cmbRole.SelectedIndex = 0;
        }

        private void ClearRegistrationFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            cmbRole.SelectedIndex = 0;
        }

        private bool UserExists(string username)
        {
            string filePath = "users.txt";

            if (!File.Exists(filePath))
            {
                return false;
            }

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

        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string role = cmbRole.SelectedItem?.ToString();


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


            if (role != "admin" && role != "client" && role != "worker")
            {
                MessageBox.Show("Роль может быть только 'admin',  'client' или 'worker'",
                                "Недопустимая роль",
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

        private void btnLogins_Click(object sender, EventArgs e)
        {
            Form2 Forms = new Form2();
            Forms.Show();
            this.Hide();
        }
    }
}
