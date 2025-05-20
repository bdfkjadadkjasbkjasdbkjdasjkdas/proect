using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp11
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            UpdateUsersList();
        }

        private void UpdateUsersList()
        {
            dgvUsers.DataSource = DataService.LoadUsers();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            new Form5().ShowDialog();
            UpdateUsersList();
        }
        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                var users = DataService.LoadUsers();
                users.RemoveAt(dgvUsers.SelectedRows[0].Index);
                DataService.SaveUsers(users);
                UpdateUsersList();
            }
        }
    }
}
