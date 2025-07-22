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
    public partial class TeamMemberView : UserControl
    {
        private readonly TaskManagerDb1Context _context;
        private int _currentUserId;

        public TeamMemberView()
        {
            InitializeComponent();
            _context = new TaskManagerDb1Context();
            try
            {
                _currentUserId = GetCurrentUserId();
                if (_currentUserId == 0)
                {
                    MessageBox.Show("No user is logged in. Please log in again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                LoadAssignedTasks();
                LoadNotifications();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadAssignedTasks()
        {
            try
            {
                var tasks = _context.Tasks
                    .Where(t => t.AssignedTo == _currentUserId)
                    .Select(t => new AssignedTaskViewModel
                    {
                        TaskId = t.TaskId,
                        Title = t.Title,
                        Status = t.Status ?? "ToDo",
                        Priority = t.Priority ?? "Low",
                        DueDate = t.DueDate.HasValue ? t.DueDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null
                    }).ToList();

                AssignedTasksDataGrid.ItemsSource = tasks;
                if (tasks.Count == 0)
                {
                    MessageBox.Show("No tasks assigned to the current user.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadNotifications()
        {
            try
            {
                var notifications = _context.Notifications
                    .Where(n => n.UserId == _currentUserId)
                    .OrderByDescending(n => n.CreatedAt)
                    .Select(n => new
                    {
                        n.NotificationId,
                        DisplayText = $"{n.Message} (at {n.CreatedAt:yyyy-MM-dd HH:mm:ss})",
                        n.IsRead,
                        n.TaskId
                    }).ToList();

                NotificationsListBox.ItemsSource = notifications;
                if (!notifications.Any())
                {
                    NotificationsListBox.ItemsSource = new[] { new { DisplayText = "No notifications available.", IsRead = true, NotificationId = 0, TaskId = (int?)null } };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading notifications: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RequestStatusUpdate_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button?.DataContext as AssignedTaskViewModel;
            if (task != null)
            {
                var existingRequest = _context.Notifications
                    .Any(n => n.TaskId == task.TaskId && n.UserId == _currentUserId && n.IsRead == false && !string.IsNullOrEmpty(n.RequestedStatus));
                if (existingRequest)
                {
                    MessageBox.Show("A status update request for this task is already pending.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var requestStatusWindow = new RequestStatusWindow(task.TaskId);
                requestStatusWindow.Owner = Window.GetWindow(this);
                requestStatusWindow.RequestSubmitted += LoadNotifications;
                requestStatusWindow.ShowDialog();
            }
        }

        private void ViewTaskDetails_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button?.DataContext as AssignedTaskViewModel;
            if (task != null)
            {
                try
                {
                    var taskDetailsWindow = new TaskDetailsWindow(task.TaskId);
                    taskDetailsWindow.Owner = Window.GetWindow(this);
                    taskDetailsWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening task details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void MarkNotificationAsRead_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var notification = button?.DataContext as dynamic;
            if (notification != null)
            {
                try
                {
                    var dbNotification = _context.Notifications.Find(notification.NotificationId);
                    if (dbNotification != null)
                    {
                        dbNotification.IsRead = true;
                        _context.SaveChanges();
                        LoadNotifications();
                        MessageBox.Show("Notification marked as read.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error marking notification as read: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                CurrentUser.Instance.Clear();
                Window mainWindow = Window.GetWindow(this);
                if (mainWindow is MainWindow mw)
                {
                    mw.ShowLoginForm();
                }
            }
        }

        private int GetCurrentUserId()
        {
            var currentUser = CurrentUser.Instance.Current;
            return currentUser?.UserId ?? 0;
        }
    }

    public class AssignedTaskViewModel
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class BooleanToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is bool && (bool)value ? Brushes.Gray : Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is bool && (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
