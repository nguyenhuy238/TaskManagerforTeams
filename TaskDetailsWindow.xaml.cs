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
    public partial class TaskDetailsWindow : Window
    {
        private readonly TaskManagerDb1Context _context;
        private readonly int _taskId;

        public TaskDetailsWindow(int taskId)
        {
            InitializeComponent();
            _context = new TaskManagerDb1Context();
            _taskId = taskId;
            LoadTaskDetails();
        }

        private void LoadTaskDetails()
        {
            try
            {
                var task = _context.Tasks
                    .Where(t => t.TaskId == _taskId)
                    .Select(t => new
                    {
                        t.Title,
                        t.Description
                    })
                    .FirstOrDefault();

                if (task != null)
                {
                    TaskTitle.Text = task.Title;
                    TaskDescription.Text = task.Description ?? "No description available.";
                }
                else
                {
                    TaskTitle.Text = "Task not found";
                    TaskDescription.Text = "";
                }

                var comments = _context.TaskComments
                    .Where(c => c.TaskId == _taskId)
                    .Join(_context.Users,
                        c => c.UserId,
                        u => u.UserId,
                        (c, u) => new
                        {
                            c.CommentText,
                            CreatedByName = u.FullName,
                            c.CreatedAt
                        })
                    .OrderBy(c => c.CreatedAt)
                    .ToList();

                CommentsListBox.ItemsSource = comments;
                if (comments.Count == 0)
                {
                    CommentsListBox.ItemsSource = new[] { new { CommentText = "No comments available.", CreatedByName = "", CreatedAt = DateTime.Now } };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading task details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
