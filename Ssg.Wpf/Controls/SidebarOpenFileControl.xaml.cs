using Microsoft.Extensions.Configuration;
using Ssg.Wpf.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Ssg.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for SidebarOpenFileControl.xaml
    /// </summary>
    public partial class SidebarOpenFileControl : UserControl
    {
        private readonly IConfiguration _config;

        public event EventHandler<string> FileChanged;

        public List<string> Filenames { get; set; } = new List<string>();
        private string _htmlDirectory;

        public SidebarOpenFileControl(IConfiguration config, string filesDirectory)
        {
            InitializeComponent();

            _config = config;
            _htmlDirectory = filesDirectory;

            string[] inputDirectoryFiles = Directory.GetFiles(filesDirectory);

            if (inputDirectoryFiles.Length > 0)
            {

                foreach (string inputFilePath in inputDirectoryFiles)
                {
                    Filenames.Add(Path.GetFileName(inputFilePath));
                }

                //txtFileNames.Text = Filenames.ToString();
                fileNames.ItemsSource = Filenames;
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
                MessageBox.Show("No files found");
            }

        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = fileNames.SelectedItem;
            
            if (selectedItem is not null)
            {
                string selectedString = selectedItem.ToString();
                string fileName = Path.Combine(_htmlDirectory, selectedString);
                if (fileName != null)
                {
                    SetSelectedFilename(selectedString);                    
                    HideSidebar();
                }

            }
        }

        private void SetSelectedFilename(string newFileName)
        {            
            DbFile.SaveSelectedFile(newFileName);
            FileChanged?.Invoke(this, newFileName);
        }

        private void HideSidebar()
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void btnCloseSidebar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            HideSidebar();
        }
    }
}
