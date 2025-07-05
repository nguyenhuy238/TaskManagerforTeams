using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Security.Cryptography;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        private readonly TaskManagerDb1Context _context;

        public LoginView()
        {
            InitializeComponent();
            _context = new TaskManagerDb1Context();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                txtErrorMessage.Text = "Vui lòng nhập tên đăng nhập và mật khẩu.";
                txtErrorMessage.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                // Mã hóa mật khẩu nhập vào (SHA256)
                string passwordHash = ComputeSha256Hash(password);

                // Truy vấn người dùng
                var user = _context.Users
                    .FirstOrDefault(u => u.Username == username && u.PasswordHash == passwordHash);

                if (user == null)
                {
                    txtErrorMessage.Text = "Tên đăng nhập hoặc mật khẩu không hợp lệ.";
                    txtErrorMessage.Visibility = Visibility.Visible;
                    return;
                }

                // Cập nhật thời gian đăng nhập cuối
                user.LastLogin = DateTime.Now;
                _context.SaveChanges();

                // Lưu thông tin người dùng
                CurrentUser.Instance.SetUser(user);

                // Tìm MainWindow và chuyển hướng đến dashboard tương ứng
                Window mainWindow = Window.GetWindow(this);
                if (mainWindow is MainWindow mw)
                {
                    mw.LoadContentBasedOnRole();
                }
            }
            catch (Exception ex)
            {
                txtErrorMessage.Text = $"Lỗi: {ex.Message}";
                txtErrorMessage.Visibility = Visibility.Visible;
            }
        }

        private string ComputeSha256Hash(string rawData)
        {
            // TODO: Thay thế bằng mã hóa SHA256 thực tế
            return rawData;
            //using (SHA256 sha256 = SHA256.Create())
            //{
            //    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            //    StringBuilder builder = new StringBuilder();
            //    for (int i = 0; i < bytes.Length; i++)
            //    {
            //        builder.Append(bytes[i].ToString("x2"));
            //    }
            //    return builder.ToString();
            //}
        }
    }
}
