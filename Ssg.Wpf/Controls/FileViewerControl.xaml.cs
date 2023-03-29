using Microsoft.Extensions.Configuration;
using Ssg.Wpf.Data;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace Ssg.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for FileViewerControl.xaml
    /// </summary>
    public partial class FileViewerControl : UserControl
    {
        private readonly IConfiguration _config;

        //private string inputDirectory;
        //private string outputDirectory;
        

        public FileViewerControl(IConfiguration config, string fileName)
        {
            _config = config;

            InitializeComponent();

            LoadHtmlFile(fileName);
        }


        private void LoadHtmlFile(string fileName)
        {
            string outputDirectory = _config.GetSection("folders").GetSection("outputHtmlFiles").Value;
            string filePath = Path.Combine(outputDirectory, "wpf", fileName);
            string content = File.ReadAllText(filePath);

            htmlContent.NavigateToString(content);

        }

    }
}
