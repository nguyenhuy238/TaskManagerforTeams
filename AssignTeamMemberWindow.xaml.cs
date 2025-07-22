using Microsoft.EntityFrameworkCore;
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

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for AssignTeamMemberWindow.xaml
    /// </summary>
    public partial class AssignTeamMemberWindow : Window, INotifyPropertyChanged
    {
        private readonly TaskManagerDb1Context _context;
        private readonly int _projectId;
        private int? _selectedUserId;
        private List<TeamMemberViewModel> _teamMembers;

        public event Action UserAssigned;

        public int? SelectedUserId
        {
            get => _selectedUserId;
            set
            {
                _selectedUserId = value;
                OnPropertyChanged(nameof(SelectedUserId));
            }
        }

        public List<TeamMemberViewModel> TeamMembers
        {
            get => _teamMembers;
            set
            {
                _teamMembers = value;
                OnPropertyChanged(nameof(TeamMembers));
            }
        }

        public AssignTeamMemberWindow(int projectId)
        {
            InitializeComponent();
            _context = new TaskManagerDb1Context();
            _projectId = projectId;
            DataContext = this;
            LoadAvailableTeamMembers();
        }

        private void LoadAvailableTeamMembers()
        {
            try
            {
                // Lấy danh sách tất cả người dùng có vai trò TeamMember
                var assignedUserIds = _context.ProjectUserRoles
                    .Where(pur => pur.ProjectId == _projectId)
                    .Select(pur => pur.UserId)
                    .ToList();

                var availableUsers = _context.Users
                    .AsNoTracking()
                    .Where(u => u.Role == "TeamMember")
                    .Select(u => new TeamMemberViewModel
                    {
                        UserId = u.UserId,
                        FullName = u.FullName,
                        Skills = u.Skills ?? "None",
                        CurrentTasks = _context.Tasks
                            .Count(t => t.AssignedTo == u.UserId && t.Status != "Done"),
                        OnTimeRate = (decimal)_context.PerformanceMetrics
                            .Where(pm => pm.UserId == u.UserId)
                            .OrderByDescending(pm => pm.PayPeriod)
                            .Select(pm => pm.OnTimeRate)
                            .FirstOrDefault(),
                        QualityScore = (decimal)_context.PerformanceMetrics
                            .Where(pm => pm.UserId == u.UserId)
                            .OrderByDescending(pm => pm.PayPeriod)
                            .Select(pm => pm.QualityScore)
                            .FirstOrDefault(),
                        ProjectCount = _context.ProjectUserRoles
                            .Count(pur => pur.UserId == u.UserId && pur.Role == "TeamMember"),
                        IsAssigned = assignedUserIds.Contains(u.UserId),
                        AssignedStatus = assignedUserIds.Contains(u.UserId) ? "Assigned" : "Unassigned"
                    })
                    .ToList();

                TeamMembers = availableUsers;
                TeamMembersDataGrid.ItemsSource = TeamMembers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading team members: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAssign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!SelectedUserId.HasValue)
                {
                    MessageBox.Show("Please select a team member.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int currentUserId = GetCurrentUserId();
                // Kiểm tra quyền ProjectManager
                var isProjectManager = _context.ProjectUserRoles
                    .Any(pur => pur.ProjectId == _projectId && pur.UserId == currentUserId && pur.Role == "ProjectManager");
                if (!isProjectManager)
                {
                    MessageBox.Show("You do not have permission to assign team members.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Kiểm tra xem người dùng đã được gán chưa
                var selectedUser = TeamMembers.FirstOrDefault(tm => tm.UserId == SelectedUserId.Value);
                if (selectedUser?.IsAssigned == true)
                {
                    MessageBox.Show("This user is already assigned to the project.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var transaction = _context.Database.BeginTransaction())
                {
                    // Thêm bản ghi vào ProjectUserRoles
                    var projectUserRole = new ProjectUserRole
                    {
                        ProjectId = _projectId,
                        UserId = SelectedUserId.Value,
                        Role = "TeamMember",
                        AssignedAt = DateTime.Now
                    };
                    _context.ProjectUserRoles.Add(projectUserRole);

                    // Tạo thông báo
                    var project = _context.Projects.Find(_projectId);
                    var notification = new Notification
                    {
                        UserId = SelectedUserId.Value,
                        Message = $"You have been assigned to project '{project.Name}'.",
                        IsRead = false,
                        CreatedAt = DateTime.Now
                    };
                    _context.Notifications.Add(notification);

                    _context.SaveChanges();
                    transaction.Commit();

                    MessageBox.Show("Team member assigned successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    UserAssigned?.Invoke();
                    LoadAvailableTeamMembers(); // Làm mới DataGrid để cập nhật trạng thái
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error assigning team member: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
    }

    public class TeamMemberViewModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Skills { get; set; }
        public int CurrentTasks { get; set; }
        public decimal OnTimeRate { get; set; }
        public decimal QualityScore { get; set; }
        public int ProjectCount { get; set; }
        public bool IsAssigned { get; set; }
        public string AssignedStatus { get; set; }
    }
}
