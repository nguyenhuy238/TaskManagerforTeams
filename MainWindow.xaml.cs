using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManager.Models;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskManagerDbContext  context = new();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            // Kiểm tra đầu vào
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                txtError.Text = "Please enter both username and password.";
                txtError.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                // Tìm người dùng trong cơ sở dữ liệu
                var user = context.Users
                    .FirstOrDefault(u => u.Username == username);

                if (user == null)
                {
                    txtError.Text = "Invalid username.";
                    txtError.Visibility = Visibility.Visible;
                    return;
                }

                // So sánh mật khẩu (giả định PasswordHash là SHA256)
                string passwordHash = ComputeSha256Hash(password);
                if (user.PasswordHash != passwordHash)
                {
                    txtError.Text = "Invalid password.";
                    txtError.Visibility = Visibility.Visible;
                    return;
                }

                // Cập nhật thời gian đăng nhập
                user.LastLogin = DateTime.Now;
                context.SaveChanges();

                // Chuyển sang giao diện chính
                LoginPanel.Visibility = Visibility.Collapsed;
                MainPanel.Visibility = Visibility.Visible;
                txtError.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                txtError.Text = $"An error occurred: {ex.Message}";
                txtError.Visibility = Visibility.Visible;
            }
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}