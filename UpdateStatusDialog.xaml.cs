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

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for UpdateStatusDialog.xaml
    /// </summary>
    public partial class UpdateStatusDialog : Window
    {
        public string NewStatus { get; private set; }
        public string Comment { get; private set; }

        public UpdateStatusDialog(string currentStatus)
        {
            InitializeComponent();
            NewStatus = currentStatus; // Gán trạng thái hiện tại làm mặc định
            StatusTextBlock.Text = $"Current Status: {NewStatus}";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CommentTextBox.Text))
            {
                MessageBox.Show("Comment is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Comment = CommentTextBox.Text;
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
