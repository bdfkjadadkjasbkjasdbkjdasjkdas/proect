using Microsoft.VisualBasic.ApplicationServices;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using WinFormsApp11;
using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinFormsApp11
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cmbRole.Items.AddRange(new string[] { "admin", "worker", "client" });
            cmbRole.SelectedIndex = 0;
        }

        private async Task FindCity()
        {
            try
            {
                string city = txtWeather.Text;

                string url = $"https://ipinfo.io/{city}/geo";

                using HttpClient client = new HttpClient();
                string data = await client.GetStringAsync(url);

                var jobject = JObject.Parse(data);

                lbl_name.Text = lbl_name.Text + jobject["city"]?.ToString();

                if (lbl_name.Text == "Krasnoyarsk")
                {
                    lbl_City.Text = "В твоем городе есть - Научная библиотека Сибирского федерального университета";
                }
                else if (lbl_name.Text == "Moscow")
                {
                    lbl_City.Text = "В твоем городе есть - Российская государственная библиотека";
                }
                else if (lbl_name.Text == "Yekaterinburg")
                {
                    lbl_City.Text = "В твоем городе есть - Свердловская областная универсальная научная библиотека им. В.Г. Белинского";
                }
                else if (lbl_name.Text == "Novosibirsk")
                {
                    lbl_City.Text = "В твоем городе есть - Государственная публичная научно-техническая библиотека СО РАН";
                }
                else if (lbl_name.Text == "Saint Petersburg")
                {
                    lbl_City.Text = "В твоем городе есть - Российская национальная библиотека";
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void btnCity_Click(object sender, EventArgs e)
        {
            FindCity();
        }


        private string GetHashSHA256(string plainText)
        {
            string hashText = "";
            Encoding enc = Encoding.UTF8;
            using (SHA256Managed hash = new SHA256Managed())
            {
                byte[] result = hash.ComputeHash(enc.GetBytes(plainText));
                foreach (byte item in result)
                {
                    hashText += item.ToString("X2");
                }
            }
            return hashText;
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
                if (parts.Length > 0 && parts[0] == username)
                {
                    return true;
                }
            }
            return false;
        }
        private void RegisterUser(string username, string password, string role)
        {
            string filePath = "users.txt";
            string hashedPassword = GetHashSHA256(password);
            string userRecord = $"{username},{hashedPassword},{role}";

            File.AppendAllText(filePath, userRecord + Environment.NewLine);
        }

        private void btnRegister_Click(object sender, EventArgs e)
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
                MessageBox.Show("Роль может быть только 'admin', 'client' или 'worker'",
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Form2 Forms = new Form2();
            Forms.Show();
            this.Hide();
        }
    }
}
