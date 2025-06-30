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
        private TaskManagerDb1Context _context = new();
        public MainWindow()
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
                txtErrorMessage.Text = "Please enter both username and password.";
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
                    txtErrorMessage.Text = "Invalid username or password.";
                    txtErrorMessage.Visibility = Visibility.Visible;
                    return;
                }

                // Cập nhật thời gian đăng nhập cuối
                user.LastLogin = DateTime.Now;
                _context.SaveChanges();

                // Lưu thông tin người dùng
                CurrentUser.Instance.SetUser(user);

                // Chuyển hướng đến dashboard tương ứng
                LoadContentBasedOnRole();
            }
            catch (Exception ex)
            {
                txtErrorMessage.Text = $"Error: {ex.Message}";
                txtErrorMessage.Visibility = Visibility.Visible;
            }
        }

        private void LoadContentBasedOnRole()
        {
            var user = CurrentUser.Instance.Current;
            if (user == null)
            {
                MessageBox.Show("No user logged in.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Chọn UserControl dựa trên vai trò
            switch (user.Role)
            {
                case "Admin":
                    MainContent.Content = new AdminView();
                    break;
                case "ProjectManager":
                    MainContent.Content = new ProjectManagerView();
                    break;
                case "TeamMember":
                    MainContent.Content = new TeamMemberView();
                    break;
                case "Guest":
                    MainContent.Content = new GuestView();
                    break;
                default:
                    MessageBox.Show("Invalid user role.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private string ComputeSha256Hash(string rawData)
        {
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

    // Lớp singleton để lưu thông tin người dùng
    public class CurrentUser
    {
        private static CurrentUser _instance;
        public static CurrentUser Instance => _instance ??= new CurrentUser();

        public User Current { get; private set; }

        public void SetUser(User user)
        {
            Current = user;
        }

        public void Clear()
        {
            Current = null;
        }
    }
}
