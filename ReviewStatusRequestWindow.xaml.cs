using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using static TaskManager.NotificationsWindow;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for ReviewStatusRequestWindow.xaml
    /// </summary>
    public partial class ReviewStatusRequestWindow : Window
    {
        private readonly TaskManagerDb1Context _context;
        private readonly NotificationViewModel _notification;
        private readonly string _taskTitle; 
        public event Action RequestProcessed;

        public string TaskTitle => _taskTitle;
        public string RequestMessage => _notification.Message;
        public string RequestedStatus => _notification.RequestedStatus;
        public ReviewStatusRequestWindow(NotificationViewModel notification, string taskTitle)
        {
            InitializeComponent();
            _context = new TaskManagerDb1Context();
            _notification = notification;
            _taskTitle = taskTitle;
            DataContext = this;
        }

        private void BtnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var task = _context.Tasks.Find(_notification.TaskId.Value);
                    if (task == null)
                    {
                        MessageBox.Show("Task not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    int currentUserId = GetCurrentUserId();
                    var isProjectManager = _context.ProjectUserRoles
                        .Any(pur => pur.ProjectId == task.ProjectId && pur.UserId == currentUserId && pur.Role == "ProjectManager");
                    if (!isProjectManager)
                    {
                        MessageBox.Show("You do not have permission to approve this request.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    task.Status = _notification.RequestedStatus;
                    task.CompletedAt = _notification.RequestedStatus == "Done" ? (DateTime?)DateTime.Now : null;

                    _context.ActivityLogs.Add(new ActivityLog
                    {
                        UserId = currentUserId,
                        TaskId = task.TaskId,
                        Action = "ApproveStatusUpdate",
                        Description = $"Approved status change to {_notification.RequestedStatus} for task '{task.Title}'",
                        CreatedAt = DateTime.Now
                    });

                    _context.Notifications.Add(new Notification
                    {
                        UserId = task.AssignedTo.Value,
                        TaskId = task.TaskId,
                        Message = $"Your request to update task '{task.Title}' to {_notification.RequestedStatus} has been approved.",
                        IsRead = false,
                        CreatedAt = DateTime.Now
                    });

                    _context.SaveChanges();
                    transaction.Commit();

                    RequestProcessed?.Invoke();
                    MessageBox.Show("Status update approved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error approving request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ResponseCommentTextBox.Text))
            {
                MessageBox.Show("Please provide a comment for rejection.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var task = _context.Tasks.Find(_notification.TaskId.Value);
                    if (task == null)
                    {
                        MessageBox.Show("Task not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    int currentUserId = GetCurrentUserId();
                    var isProjectManager = _context.ProjectUserRoles
                        .Any(pur => pur.ProjectId == task.ProjectId && pur.UserId == currentUserId && pur.Role == "ProjectManager");
                    if (!isProjectManager)
                    {
                        MessageBox.Show("You do not have permission to reject this request.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    _context.ActivityLogs.Add(new ActivityLog
                    {
                        UserId = currentUserId,
                        TaskId = task.TaskId,
                        Action = "RejectStatusUpdate",
                        Description = $"Rejected status change to {_notification.RequestedStatus} for task '{task.Title}'",
                        CreatedAt = DateTime.Now
                    });

                    _context.Notifications.Add(new Notification
                    {
                        UserId = task.AssignedTo.Value,
                        TaskId = task.TaskId,
                        Message = $"Your request to update task '{task.Title}' to {_notification.RequestedStatus} was rejected. Reason: {ResponseCommentTextBox.Text}",
                        IsRead = false,
                        CreatedAt = DateTime.Now
                    });

                    _context.SaveChanges();
                    transaction.Commit();

                    RequestProcessed?.Invoke();
                    MessageBox.Show("Status update rejected successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error rejecting request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private int GetCurrentUserId()
        {
            var currentUser = CurrentUser.Instance.Current;
            return currentUser?.UserId ?? 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
