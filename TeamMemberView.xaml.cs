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

        private void UpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var stackPanel = button.Parent as StackPanel;
                var comboBox = stackPanel?.Children.OfType<ComboBox>().FirstOrDefault();
                var task = button.DataContext as AssignedTaskViewModel;
                if (comboBox != null && task != null)
                {
                    var newStatus = comboBox.SelectedValue as string;
                    if (!string.IsNullOrEmpty(newStatus) && new[] { "ToDo", "InProgress", "Done" }.Contains(newStatus))
                    {
                        try
                        {
                            var dialog = new UpdateStatusDialog(task.Status);
                            dialog.Owner = Window.GetWindow(this);
                            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.Comment))
                            {
                                var dbTask = _context.Tasks.Find(task.TaskId);
                                if (dbTask != null)
                                {
                                    dbTask.Status = newStatus;
                                    if (newStatus == "Done")
                                        dbTask.CompletedAt = DateTime.Now;

                                    _context.TaskComments.Add(new TaskComment
                                    {
                                        TaskId = task.TaskId,
                                        UserId = _currentUserId,
                                        CommentText = dialog.Comment,
                                        CreatedAt = DateTime.Now
                                    });

                                    _context.ActivityLogs.Add(new ActivityLog
                                    {
                                        UserId = _currentUserId,
                                        TaskId = task.TaskId,
                                        Action = "Update Status",
                                        Description = $"Task {task.TaskId} status changed to {newStatus} by {CurrentUser.Instance.Current.FullName}",
                                        CreatedAt = DateTime.Now
                                    });

                                    var projectManagerIds = _context.ProjectUserRoles
                                        .Where(pur => pur.ProjectId == dbTask.ProjectId && pur.Role == "ProjectManager")
                                        .Select(pur => pur.UserId)
                                        .ToList();
                                    foreach (var pmId in projectManagerIds)
                                    {
                                        _context.Notifications.Add(new Notification
                                        {
                                            UserId = pmId,
                                            TaskId = task.TaskId,
                                            Message = $"Task {task.Title} status updated to {newStatus} by {CurrentUser.Instance.Current.FullName}",
                                            IsRead = false,
                                            CreatedAt = DateTime.Now
                                        });
                                    }

                                    _context.SaveChanges();
                                    task.Status = newStatus;
                                    MessageBox.Show("Task status updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else if (dialog.ShowDialog() == true && string.IsNullOrWhiteSpace(dialog.Comment))
                            {
                                MessageBox.Show("Comment is required to update status.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error updating task status: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid status value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
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