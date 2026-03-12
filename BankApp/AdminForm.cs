using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace BankApp
{
    public class AdminForm : Form
    {
        TextBox txtUser = new TextBox();
        TextBox txtPass = new TextBox();
        TextBox txtAmount = new TextBox();
        Button btnCreate = new Button();
        Button btnAddMoney = new Button();
        Button btnUsers = new Button();
        public AdminForm()
        {
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Admin Panel";
            Size = new Size(400, 300);

            txtUser.SetBounds(50, 30, 200, 30);
            txtPass.SetBounds(50, 70, 200, 30);
            txtAmount.SetBounds(50, 150, 200, 30);

            btnCreate.Text = "Create Customer";
            btnCreate.SetBounds(50, 110, 200, 30);
            btnCreate.Click += CreateCustomer;

            btnAddMoney.Text = "Add Money";
            btnAddMoney.SetBounds(50, 190, 200, 30);
            btnAddMoney.Click += AddMoney;

            btnUsers.Text = "Users List";
            btnUsers.SetBounds(50, 230, 200, 30);
            btnUsers.Click += OpenUsers;

            Controls.AddRange(new Control[]
            {
                txtUser,
                txtPass,
                txtAmount,
                btnCreate,
                btnAddMoney,
                btnUsers
            });
        }

        private void CreateCustomer(object sender, EventArgs e)
        {
            if (txtUser.Text.Trim() == "" || txtPass.Text.Trim() == "")
            {
                MessageBox.Show("Username ve Password boş olamaz");
                return;
            }

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText =
                "INSERT INTO Users(Username,Password,Role,Balance) VALUES(@u,@p,'Customer',0)";

                cmd.Parameters.AddWithValue("@u", txtUser.Text);
                cmd.Parameters.AddWithValue("@p", txtPass.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Customer Created");
            }
        }

        private void AddMoney(object sender, EventArgs e)
        {
            double amount;
            if (!double.TryParse(txtAmount.Text, out amount))
            {
                MessageBox.Show("Invalid Amount");
                return;
            }

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText =
                "UPDATE Users SET Balance = Balance + @a WHERE Username=@u";

                cmd.Parameters.AddWithValue("@a", amount);
                cmd.Parameters.AddWithValue("@u", txtUser.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Money Added");
            }
        }
        void OpenUsers(object sender, EventArgs e)
        {
            new UsersForm().ShowDialog();
        }
    }
}