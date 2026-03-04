using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace BankApp
{
    public class LoginForm : Form
    {
        TextBox txtUser = new TextBox();
        TextBox txtPass = new TextBox();
        Button btnLogin = new Button();

        public LoginForm()
        {
            Text = "Bank Login";
            Size = new Size(300, 220);

            txtUser.SetBounds(50, 30, 200, 30);

            txtPass.SetBounds(50, 70, 200, 30);
            txtPass.PasswordChar = '*';

            btnLogin.Text = "Login";
            btnLogin.SetBounds(100, 120, 100, 35);
            btnLogin.Click += Login;

            Controls.AddRange(
                new Control[] { txtUser, txtPass, btnLogin });
        }

        private void Login(object sender, EventArgs e)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText =
                "SELECT Id,Role FROM Users WHERE Username=@u AND Password=@p";

                cmd.Parameters.AddWithValue("@u", txtUser.Text);
                cmd.Parameters.AddWithValue("@p", txtPass.Text);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string role = reader.GetString(1);

                        if (role == "Admin")
                            new AdminForm().Show();
                        else
                            new CustomerForm(id).Show();

                        Hide();
                    }
                    else
                        MessageBox.Show("Login Failed");
                }
            }
        }
    }
}