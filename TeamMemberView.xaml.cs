using Microsoft.EntityFrameworkCore;
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
using TaskManager.Models;


namespace TaskManager
{
    /// <summary>
    /// Interaction logic for TeamMemberView.xaml
    /// </summary>
    public partial class TeamMemberView : UserControl
    {
        private readonly TaskManagerDb1Context _context;
        private int _currentUserId; // Giả định UserId của thành viên hiện tại (cần lấy từ session hoặc login)

        public TeamMemberView()
        {
            InitializeComponent();
            _context = new TaskManagerDb1Context();
            _currentUserId = 3; // Giả lập UserId, cần thay bằng logic thực tế (ví dụ: từ đăng nhập)
            LoadAssignedTasks();
            LoadNotifications();
        }

        private void LoadAssignedTasks()
        {
            try
            {
                var tasks = _context.Tasks
                    .Include(t => t.AssignedToNavigation)
                    .Where(t => t.AssignedTo == _currentUserId)
                    .Select(t => new AssignedTaskViewModel
                    {
                        TaskId = t.TaskId,
                        Title = t.Title,
                        Status = t.Status,
                        DueDate = t.DueDate.HasValue ? t.DueDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null
                    }).ToList();

                AssignedTasksDataGrid.ItemsSource = tasks;
                if (tasks.Count == 0)
                {
                    MessageBox.Show("No tasks assigned to the current user.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}");
            }
        }

        private void LoadNotifications()
        {
            var notifications = _context.Notifications
                .Where(n => n.UserId == _currentUserId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new
                {
                    n.Message,
                    n.CreatedAt
                }).ToList();

            NotificationsListBox.ItemsSource = notifications.Select(n => $"{n.Message} (at {n.CreatedAt})");
        }

        private void UpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var stackPanel = button.Parent as StackPanel;
                if (stackPanel != null)
                {
                    var comboBox = stackPanel.Children.OfType<ComboBox>().FirstOrDefault();
                    if (comboBox != null)
                    {
                        var task = button.DataContext as AssignedTaskViewModel;
                        if (task != null)
                        {
                            var newStatus = comboBox.SelectedItem as ComboBoxItem;
                            if (newStatus != null)
                            {
                                var dbTask = _context.Tasks.Find(task.TaskId);
                                if (dbTask != null)
                                {
                                    dbTask.Status = newStatus.Content.ToString();
                                    _context.SaveChanges();
                                    task.Status = newStatus.Content.ToString(); // Cập nhật mô hình cục bộ
                                    LoadAssignedTasks();
                                    MessageBox.Show("Task status updated successfully!");
                                }
                            }
                        }
                    }
                }
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