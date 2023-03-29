using System;
using System.IO;
using System.Windows;

namespace Ssg.Wpf.Windows
{
    /// <summary>
    /// Interaction logic for EditFileWindow.xaml
    /// </summary>
    public partial class EditFileWindow : Window
    {

        private readonly string _fileName;
        private bool _isContentModified;

        public event EventHandler<string> FileSaved;

        public EditFileWindow(string filePath)
        {
            InitializeComponent();

            _fileName = filePath;
            _isContentModified = false;

            LoadFileContent();
        }

        private void LoadFileContent()
        {
            this.Title = $"EDIT FILE - {_fileName}";
            string fileContent = File.ReadAllText(_fileName);
            txtFileContent.Text = fileContent;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {            
            SaveFile();
        }

        private void SaveFile()
        {
            File.WriteAllText(_fileName, txtFileContent.Text);

            _isContentModified = false;
            // txtStatusBar.Text = $"File saved at {DateTime.Now}";
            txtStatusBar.Visibility = Visibility.Collapsed;

            FileSaved?.Invoke(this, _fileName);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (_isContentModified)
            {
                if (MessageBox.Show("Save before exiting?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
            }
            
            Close();
        }

        private void txtContentModified_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (_isContentModified == false)
            {
                _isContentModified = true;
                txtStatusBar.Visibility = Visibility.Visible;
            }
        }
    }
}
