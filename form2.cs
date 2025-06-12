using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using static System.Windows.Forms.DataFormats;

namespace WinFormsApp11
{
    public partial class Form2 : Form
    {
        private const string UsersFile = "users.txt";

        public Form2()
        {
            InitializeComponent();
            labelRole.Visible = false;
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

        private string ValidateUser(string username, string password)
        {
            try
            {
                if (!File.Exists(UsersFile))
                {
                    return null;
                }

                string hashedPassword = GetHashSHA256(password);
                string[] lines = File.ReadAllLines(UsersFile);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 3 && parts[0] == username && parts[1] == hashedPassword)
                    {
                        return parts[2];
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Введите логин и пароль", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string userRole = ValidateUser(username, password);
                if (userRole != null)
                {
                    labelRole.Text = $"Ваша роль: {userRole}";
                    labelRole.Visible = true;
                    MessageBox.Show($"Добро пожаловать, {username}!", "Успешный вход",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (userRole == "admin")
                    {
                        Form3 adminForm = new Form3(username);
                        adminForm.Show();
                    }
                    else if (userRole == "client")
                    {
                        Form4 clientForm = new Form4(username);
                        clientForm.Show();
                    }
                    else if (userRole == "worker")
                    {
                        Form5 workerForm = new Form5(username);
                        workerForm.Show();
                    }

                    this.Hide();
                }
                else
                {
                    labelRole.Visible = false;
                    MessageBox.Show("Неверный логин или пароль", "Ошибка входа",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }
    }
}
