using Ssg.Wpf.Controls;
using Ssg.Core;
using System.Windows;
using Microsoft.Extensions.Configuration;
using System.IO;
using Ssg.Wpf.Data;
using Ssg.Wpf.Windows;
using System;
using System.Diagnostics;

namespace Ssg.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IConfiguration _config;

        //private readonly string HtmlDirectoryWpf;

        private string _selectedFilePath;
        private string _htmlDirectory;
        private string _htmlDirectoryWpf;
        private string _htmlDirectoryWeb;
        private string _markdownDirectory;
        private string _templatesDirectory;


        public MainWindow(IConfiguration config)
        {
            InitializeComponent();

            _config = config;

            _htmlDirectory = _config.GetSection("folders").GetSection("outputHtmlFiles").Value;
            _htmlDirectoryWpf = Path.Combine(_htmlDirectory, "wpf");
            _htmlDirectoryWeb = Path.Combine(_htmlDirectory, "web");
            _markdownDirectory = _config.GetSection("folders").GetSection("inputMarkdownFiles").Value;
            _templatesDirectory = _config.GetSection("folders").GetSection("templates").Value;

            string selectedFilename = DbFile.LoadSelectedFile();            

            if (!string.IsNullOrEmpty(selectedFilename))
            {
                
                string selectedFilePath = Path.Combine(_htmlDirectoryWpf, selectedFilename);
                if (File.Exists(selectedFilePath))
                    DisplayFileContent(selectedFilePath);
            }

        }

        private void DisplayFileContent(string filePath)
        {
            _selectedFilePath = filePath;
            this.Title = filePath;
            content.Content = new FileViewerControl(_config, filePath);            
        }

        private void menuMarkdownnToHtml_Click(object sender, RoutedEventArgs e)
        {
            Window conversionWindow = new MainWindow(_config);
            conversionWindow.Title = "Convert Markdown to HTML";
            conversionWindow.Content = new MarkdownToHtmlControl(_config);
            conversionWindow.Show();
            
        }

        private void menuOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(_htmlDirectoryWpf))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = _htmlDirectoryWpf,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
        }

        private void menuOpenFile_Click(object sender, RoutedEventArgs e)
        {
            //sidebar.Content = new SidebarOpenFileControl(_config, HtmlDirectory);

            var sidebarControl = new SidebarOpenFileControl(_config, _htmlDirectoryWpf);
            sidebarControl.FileChanged += LoadHtml_FileChanged;

            sidebar.Content = sidebarControl;
        }

        private void LoadHtml_FileChanged(object? sender, string newFile)
        {
            DisplayFileContent(newFile);
        }

        private void menuFtpUpload_Click(object sender, RoutedEventArgs e)
        {
            Website.GenerateIndexFile(_htmlDirectoryWeb);

            FtpCredentials connection = GetFtpCredentials();

            string result = Website.UploadDirectoryFiles(_htmlDirectoryWeb, connection);
            result += Website.UploadDirectoryFiles(_htmlDirectoryWeb, connection, "_img");

            MessageBox.Show(result);

        }

        private void menuEditCss_Click(object sender, RoutedEventArgs e)
        {         
            string cssFilePath = Path.Combine(_templatesDirectory, "assets", "wpf.css");

            var editWindow = new EditFileWindow(cssFilePath);
            //editWindow.FileSaved += Edit;
            editWindow.FileSaved += EditWindow_CssFileSaved;

            editWindow.Show();
        }

        private void EditWindow_CssFileSaved(object? sender, string e)
        {
            MarkdownToHtml.ConvertDirectoryFiles(_markdownDirectory, _htmlDirectory, false);

            // Refresh selected WPF file
            DisplayFileContent(_selectedFilePath);
        }

        private void menuEditFile_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_selectedFilePath))
            {

                string fileName = Path.GetFileName(_selectedFilePath);                
                string markdownFilePath = Path.Combine(_markdownDirectory, fileName.Replace(".html", ".md"));

                var editWindow = new EditFileWindow(markdownFilePath);
                editWindow.FileSaved += EditWindow_FileSaved;

                editWindow.Show();
            }
        }

        private void EditWindow_FileSaved(object? sender, string file)
        {
            //MessageBox.Show($"File saved: {file}");
            ReloadMarkdownFile(file);
        }

        private void ReloadMarkdownFile(string filePath)
        {
            // Generate new HTML files
            (string outputFilePathWpf, string outputFilePathWeb) = MarkdownToHtml.ConvertSingleFile(filePath, _htmlDirectory);

            // Refresh WPF file
            DisplayFileContent(outputFilePathWpf);

            // Upload WEB file
            FtpCredentials connection = GetFtpCredentials();
            bool uploadResult = Website.UploadSingleFile(outputFilePathWeb, connection);

            if(uploadResult == false)
            {
                MessageBox.Show("Problem uploading WEB file");
            }
        }

        private FtpCredentials GetFtpCredentials()
        {
            string ftpUserName = _config.GetSection("ftp").GetSection("userName").Value;
            string ftpPassword = _config.GetSection("ftp").GetSection("password").Value;
            string ftpAddress = _config.GetSection("ftp").GetSection("address").Value;
            FtpCredentials connection = new FtpCredentials(ftpUserName, ftpPassword, ftpAddress);

            return connection;
        }
    }
}
