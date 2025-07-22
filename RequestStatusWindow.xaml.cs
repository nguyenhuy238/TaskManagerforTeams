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
using System.Xml.Linq;
using TaskManager.Models;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for RequestStatusWindow.xaml
    /// </summary>
    public partial class RequestStatusWindow : Window
    {
        public string RequestedStatus { get; private set; }
        public string Comment { get; private set; }

        
        private readonly TaskManagerDb1Context _context;
        private readonly int _taskId;
        private readonly int _userId;
        public event Action RequestSubmitted;

        public RequestStatusWindow(int taskId)
        {
            InitializeComponent();
            _context = new TaskManagerDb1Context();
            _taskId = taskId;
            _userId = GetCurrentUserId();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (StatusComboBox.SelectedItem == null || string.IsNullOrWhiteSpace(CommentTextBox.Text))
            {
                MessageBox.Show("Please select a status and enter a comment.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string requestedStatus = ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString();
            string comment = CommentTextBox.Text;

            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var task = _context.Tasks.Find(_taskId);
                    if (task == null)
                    {
                        MessageBox.Show("Task not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var projectManagerId = _context.ProjectUserRoles
                        .Where(pur => pur.ProjectId == task.ProjectId && pur.Role == "ProjectManager")
                        .Select(pur => pur.UserId)
                        .FirstOrDefault();

                    if (projectManagerId == 0)
                    {
                        MessageBox.Show("No Project Manager found for this project.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var notification = new Notification
                    {
                        UserId = projectManagerId,
                        TaskId = _taskId,
                        Message = $"TeamMember {_context.Users.Find(_userId)?.FullName ?? "Unknown"} requested status update for task '{task.Title}' to {requestedStatus}",
                        IsRead = false,
                        CreatedAt = DateTime.Now,
                        RequestedStatus = requestedStatus,
                        Comment = comment
                    };
                    _context.Notifications.Add(notification);

                    _context.SaveChanges();
                    transaction.Commit();

                    MessageBox.Show("Request submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    RequestSubmitted?.Invoke();
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetCurrentUserId()
        {
            var currentUser = CurrentUser.Instance.Current;
            return currentUser.UserId;
        }
    }
}
