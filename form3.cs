using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp11
{
    public partial class Form3 : Form
    {
        private const string UsersFile = "users.txt";
        private List<User> users = new List<User>();
        private int selectedUserIndex = -1; 
        private string currentUser;

        public Form3(string username)
        {
            InitializeComponent();
            currentUser = username;
            cmbRole.Items.AddRange(new string[] { "admin", "worker", "client" });
            SetupListView();
            LoadUsers();
        }

        private void SetupListView()
        {
            listViewUsers.View = View.Details;
            listViewUsers.Columns.Add("Логин", 150);
            listViewUsers.Columns.Add("Пароль", 150);
            listViewUsers.Columns.Add("Роль", 100);

            listViewUsers.MouseClick += ListViewUsers_MouseClick;
        }

        private void ListViewUsers_MouseClick(object sender, MouseEventArgs e)
        {
            var hitTest = listViewUsers.HitTest(e.Location);
            if (hitTest.Item != null)
            {
                selectedUserIndex = hitTest.Item.Index;
            }
        }

        private void LoadUsers()
        {
            try
            {
                users.Clear();
                listViewUsers.Items.Clear();
                selectedUserIndex = -1; 

                if (File.Exists(UsersFile))
                {
                    foreach (string line in File.ReadAllLines(UsersFile))
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length >= 3)
                        {
                            users.Add(new User(
                                parts[0].Trim(),
                                parts[1].Trim(),
                                parts[2].Trim()));
                        }
                    }
                    ShowUsers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }

        private void ShowUsers()
        {
            listViewUsers.Items.Clear();
            foreach (User user in users)
            {
                ListViewItem item = new ListViewItem(user.Username);
                item.SubItems.Add(user.Password);
                item.SubItems.Add(user.Role);

                if (user.Role == "admin")
                {
                    item.BackColor = Color.LightYellow;
                }

                listViewUsers.Items.Add(item);
            }
        }

        private void SaveUsers()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (User user in users)
                {
                    lines.Add($"{user.Username},{user.Password},{user.Role}");
                }
                File.WriteAllLines(UsersFile, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void ClearFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            selectedUserIndex = -1;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
            ClearFields();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (users.Exists(u => u.Username == txtUsername.Text))
            {
                MessageBox.Show("Пользователь с таким логином уже есть!");
                return;
            }

            User newUser = new User(
                txtUsername.Text,
                txtPassword.Text,
                cmbRole.SelectedItem.ToString());

            users.Add(newUser);
            SaveUsers();
            ShowUsers();
            ClearFields();
            MessageBox.Show("Пользователь добавлен!");
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedUserIndex == -1)
            {
                MessageBox.Show("Выберите пользователя для редактирования!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }


            if (users.Exists(u => u.Username == txtUsername.Text && users.IndexOf(u) != selectedUserIndex))
            {
                MessageBox.Show("Пользователь с таким логином уже существует!");
                return;
            }

            users[selectedUserIndex].Username = txtUsername.Text;
            users[selectedUserIndex].Password = txtPassword.Text;
            users[selectedUserIndex].Role = cmbRole.SelectedItem.ToString();

            SaveUsers();
            ShowUsers();
            MessageBox.Show("Изменения сохранены!");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedUserIndex == -1)
            {
                MessageBox.Show("Выберите пользователя для удаления!");
                return;
            }

            if (users[selectedUserIndex].Role == "admin")
            {
                MessageBox.Show("Нельзя удалить администратора!");
                return;
            }

            if (MessageBox.Show("Удалить этого пользователя?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                users.RemoveAt(selectedUserIndex);
                SaveUsers();
                ShowUsers();
                ClearFields();
                MessageBox.Show("Пользователь удален!");
            }
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public User(string username, string password, string role)
        {
            Username = username;
            Password = password;
            Role = role;
        }
    }
}
