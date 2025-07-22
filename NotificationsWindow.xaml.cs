using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for NotificationsWindow.xaml
    /// </summary>
    public partial class NotificationsWindow : Window, INotifyPropertyChanged
    {
        private readonly TaskManagerDb1Context _context;
        private ObservableCollection<NotificationViewModel> _notifications;

public ObservableCollection<NotificationViewModel> Notifications
{
    get => _notifications;
    set
    {
        _notifications = value;
        OnPropertyChanged(nameof(Notifications));
    }
}


        public NotificationsWindow()
        {
            InitializeComponent();
            _context = new TaskManagerDb1Context();
            LoadNotifications();
            DataContext = this;
        }

        private void LoadNotifications()
        {
            try
            {
                int currentUserId = GetCurrentUserId();
                var notifications = _context.Notifications
    .AsNoTracking()
    .Where(n => n.UserId == currentUserId)
    .OrderByDescending(n => n.CreatedAt)
    .Select(n => new NotificationViewModel
    {
        NotificationId = n.NotificationId,
        Message = n.Message,
        RequestedStatus = n.RequestedStatus,
        Comment = n.Comment ?? string.Empty,
        CreatedAt = n.CreatedAt ?? DateTime.MinValue,
        IsRead = n.IsRead ?? false,
        TaskId = n.TaskId,
        HasRequestedStatus = !string.IsNullOrEmpty(n.RequestedStatus)
    })
    .ToList();
                //Notifications = new ObservableCollection<NotificationViewModel>(notifications); // Correct type used here
                Notifications = new ObservableCollection<NotificationViewModel>(notifications); 
                NotificationsDataGrid.ItemsSource = Notifications;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading notifications: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReviewRequest_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var notification = button?.DataContext as NotificationViewModel;
            if (notification != null && notification.TaskId.HasValue)
            {
                var task = _context.Tasks.Find(notification.TaskId.Value);
                if (task == null)
                {
                    MessageBox.Show("Task not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var reviewWindow = new ReviewStatusRequestWindow(notification, task.Title);
                reviewWindow.Owner = this;
                reviewWindow.RequestProcessed += () =>
                {
                    var dbNotification = _context.Notifications.Find(notification.NotificationId);
                    if (dbNotification != null)
                    {
                        dbNotification.IsRead = true;
                        _context.SaveChanges();
                    }
                    LoadNotifications();
                };
                reviewWindow.ShowDialog();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void NotificationsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.DisplayIndex == 4) // Cột IsRead
            {
                var notification = e.Row.DataContext as NotificationViewModel;
                if (notification != null)
                {
                    try
                    {
                        var dbNotification = await _context.Notifications.FindAsync(notification.NotificationId);
                        if (dbNotification != null)
                        {
                            dbNotification.IsRead = notification.IsRead;
                            await _context.SaveChangesAsync();
                            MessageBox.Show("Notification status updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating notification: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private int GetCurrentUserId()
        {
            var currentUser = CurrentUser.Instance.Current;
            return currentUser.UserId;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public class NotificationViewModel
        {
            public int NotificationId { get; set; }
            public string Message { get; set; }
            public string RequestedStatus { get; set; }
            public string Comment { get; set; }
            public DateTime CreatedAt { get; set; }
            public bool? IsRead { get; set; }
            public int? TaskId { get; set; }
            public bool HasRequestedStatus { get; set; }
        }
    }
}
