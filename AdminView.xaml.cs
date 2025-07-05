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
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : UserControl
    {
        private readonly TaskManagerDb1Context _context;

        public AdminView()
        {
            _context = new TaskManagerDb1Context();
            InitializeComponent();
            LoadUsers();
            LoadProjects();
            LoadPayroll();
            GenerateReport();
        }

        private void LoadUsers()
        {
            var users = _context.Users.ToList();
            UsersDataGrid.ItemsSource = users;
        }

        private void LoadProjects()
        {
            var projects = _context.Projects.ToList();
            ProjectsDataGrid.ItemsSource = projects;
        }

        private void LoadPayroll()
        {
            var payroll = _context.Payrolls.Where(p => (bool)!p.Approved).ToList();
            PayrollDataGrid.ItemsSource = payroll;
        }

        private void GenerateReport()
        {
            var totalUsers = _context.Users.Count();
            var totalTasks = _context.Tasks.Count();
            var completedTasks = _context.Tasks.Count(t => t.Status == "Done");
            var progress = totalTasks > 0 ? (double)completedTasks / totalTasks * 100 : 0;
            ReportTextBlock.Text = $"Report (as of {DateTime.Now}): {totalUsers} users, {totalTasks} tasks, {progress:F2}% completed";
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var newUser = new User
            {
                Username = "newuser",
                PasswordHash = "hashedpassword", // Cần mã hóa thực tế
                Email = "newuser@example.com",
                FullName = "New User",
                Role = "TeamMember",
                BaseSalary = 1000.00m
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            LoadUsers();
            MessageBox.Show("User added successfully!");
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            var user = UsersDataGrid.SelectedItem as User;
            if (user != null)
            {
                // Giả lập chỉnh sửa (thực tế cần dialog hoặc form)
                user.FullName = "Edited " + user.FullName;
                _context.SaveChanges();
                LoadUsers();
                MessageBox.Show("User edited successfully!");
            }
            else
            {
                MessageBox.Show("Please select a user to edit.");
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var user = UsersDataGrid.SelectedItem as User;
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                LoadUsers();
                MessageBox.Show("User deleted successfully!");
            }
            else
            {
                MessageBox.Show("Please select a user to delete.");
            }
        }

        private void ApprovePayroll_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var payroll = button?.DataContext as Payroll;
            if (payroll != null)
            {
                payroll.Approved = true;
                payroll.ApprovedBy = 1; // Giả lập Admin ID
                payroll.ApprovedAt = DateTime.Now;
                _context.SaveChanges();
                LoadPayroll();
                MessageBox.Show("Payroll approved successfully!");
            }
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Xóa thông tin người dùng hiện tại
            CurrentUser.Instance.Clear();

            // Tìm MainWindow và gọi phương thức hiển thị màn hình đăng nhập
            Window mainWindow = Window.GetWindow(this);
            if (mainWindow is MainWindow mw)
            {
                mw.ShowLoginForm();
            }
        }
    }
}