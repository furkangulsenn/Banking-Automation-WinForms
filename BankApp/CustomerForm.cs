using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace BankApp
{
    public class CustomerForm : Form
    {
        int userId;
        Label lblBalance = new Label();
        TextBox txtAmount = new TextBox();
        Button btnSend = new Button();

        public CustomerForm(int id)
        {
            userId = id;

            Text = "Customer Panel";
            Size = new Size(400, 250);

            lblBalance.SetBounds(50, 30, 200, 30);
            txtAmount.SetBounds(50, 80, 200, 30);

            btnSend.Text = "Send Money";
            btnSend.SetBounds(50, 120, 200, 35);
            btnSend.Click += SendMoney;

            Controls.AddRange(new Control[]
            { lblBalance, txtAmount, btnSend });

            LoadBalance();
        }

        private void LoadBalance()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText =
                "SELECT Balance FROM Users WHERE Id=@id";

                cmd.Parameters.AddWithValue("@id", userId);

                lblBalance.Text =
                    "Balance: " + cmd.ExecuteScalar();
            }
        }

        private void SendMoney(object sender, EventArgs e)
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

                var check = conn.CreateCommand();
                check.CommandText =
                "SELECT Balance FROM Users WHERE Id=@id";
                check.Parameters.AddWithValue("@id", userId);

                double balance =
                    Convert.ToDouble(check.ExecuteScalar());

                if (amount > balance)
                {
                    MessageBox.Show("Insufficient Balance");
                    return;
                }

                var update = conn.CreateCommand();
                update.CommandText =
                "UPDATE Users SET Balance = Balance - @a WHERE Id=@id";

                update.Parameters.AddWithValue("@a", amount);
                update.Parameters.AddWithValue("@id", userId);

                update.ExecuteNonQuery();
                LoadBalance();
            }
        }
    }
}