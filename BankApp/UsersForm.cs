using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace BankApp
{
    public class UsersForm : Form
    {
        ListBox listUsers = new ListBox();
        Button btnDelete = new Button();

        public UsersForm()
        {
            Text = "Users List";
            Size = new Size(300, 350);
            StartPosition = FormStartPosition.CenterScreen;

            listUsers.SetBounds(20, 20, 240, 200);

            btnDelete.Text = "Delete User";
            btnDelete.SetBounds(20, 240, 240, 35);
            btnDelete.Click += DeleteUser;

            Controls.AddRange(new Control[]
            {
                listUsers,
                btnDelete
            });

            LoadUsers();
        }

        void LoadUsers()
        {
            listUsers.Items.Clear();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText =
                "SELECT Username FROM Users WHERE Role='Customer'";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listUsers.Items.Add(reader.GetString(0));
                    }
                }
            }
        }

        void DeleteUser(object sender, EventArgs e)
        {
            if (listUsers.SelectedItem == null)
            {
                MessageBox.Show("Select a user");
                return;
            }

            string username = listUsers.SelectedItem.ToString();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText =
                "DELETE FROM Users WHERE Username=@u";

                cmd.Parameters.AddWithValue("@u", username);

                cmd.ExecuteNonQuery();
            }

            LoadUsers();
        }
    }
}