using Microsoft.Extensions.Configuration;
using Ssg.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace Ssg.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for MarkdownToHtmlControl.xaml
    /// </summary>
    public partial class MarkdownToHtmlControl : UserControl
    {
        public List<string> Filenames { get; set; } = new List<string>();

        private string inputDirectory;
        private string outputDirectory;
        private readonly IConfiguration _config;

        public MarkdownToHtmlControl(IConfiguration config)
        {
            InitializeComponent();

            _config = config;

            //GetFilesDirectories();
            inputDirectory = _config.GetSection("folders").GetSection("inputMarkdownFiles").Value;
            outputDirectory = _config.GetSection("folders").GetSection("outputHtmlFiles").Value;

            //MessageBox.Show(inputDirectory);

            ListFiles();
            
        }

        private void ListFiles()
        {            
            string[] inputDirectoryFiles = Directory.GetFiles(inputDirectory);

            foreach (string inputFilePath in inputDirectoryFiles)
            {
                string ext = Path.GetExtension(inputFilePath);

                if (ext == ".md" && inputFilePath[0] != '_')
                {                    
                    Filenames.Add(inputFilePath);
                }
            }

            //txtFileNames.Text = Filenames.ToString();
            fileNames.ItemsSource = Filenames;

        }

        private void btnConvertToHtml_Click(object sender, RoutedEventArgs e)
        {
            bool answerYes = MessageBox.Show($"Convertifiles in directory {inputDirectory}?", "Please confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes;

            if (answerYes)
            {                
                MarkdownToHtml.ConvertDirectoryFiles(inputDirectory, outputDirectory);

                MarkdownToHtml.CopyImagesFolder(inputDirectory, Path.Combine(outputDirectory, "wpf"));
                MarkdownToHtml.CopyImagesFolder(inputDirectory, Path.Combine(outputDirectory, "web"));
            }

            MessageBox.Show("OK - Files converted");

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            ((Window)this.Parent).Close();
        }
    }
}
