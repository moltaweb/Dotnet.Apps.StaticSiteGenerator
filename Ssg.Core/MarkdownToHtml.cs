using Markdig;
using System;
using System.IO;

namespace Ssg.Core
{
    public class MarkdownToHtml
    {
        public static void ConvertDirectoryFiles(string inputDirectory, string outputDirectory, bool generateWebFiles=true)
        {
            DeleteFiles(outputDirectory);            

            string[] inputDirectoryFiles = Directory.GetFiles(inputDirectory);

            Array.Sort(inputDirectoryFiles);

            foreach (string inputFilePath in inputDirectoryFiles)
            {
                string fileName = Path.GetFileName(inputFilePath);

                if (ApplyFilters(fileName))
                {
                    GenerateHtmlWpfAndWebFiles(inputFilePath, outputDirectory, generateWebFiles);
                }
            }          

        }

        public static (string outputFilePathWpf, string outputFilePathWeb) ConvertSingleFile(string inputFilePath, string outputDirectory)
        {
            (string outputFilePathWpf, string outputFilePathWeb)  = GenerateHtmlWpfAndWebFiles(inputFilePath, outputDirectory);

            return (outputFilePathWpf, outputFilePathWeb);
        }

        private static void DeleteFiles(string outputDirectory)
        {
            DirectoryInfo di = new DirectoryInfo(outputDirectory);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        private static bool ApplyFilters(string fileName)
        {
            bool output = true;

            if (!fileName.EndsWith(".md") || fileName.StartsWith("_"))
                output = false;

            return output;
        }

        public static void CopyImagesFolder(string inputDirectory, string outputDirectory)
        {
            //string localParentFolder = Directory.GetParent(inputDirectory).FullName;

            if (Directory.Exists(Path.Combine(inputDirectory, "img")))
            {
                string[] inputDirectoryFiles = Directory.GetFiles(Path.Combine(inputDirectory, "img"));

                if (inputDirectoryFiles.Length > 0)
                {
                    //string remoteParentFolder = Directory.GetParent(outputDirectory).FullName;

                    if (!Directory.Exists(Path.Combine(outputDirectory, "img")))
                    {
                        Directory.CreateDirectory(Path.Combine(outputDirectory, "img"));
                    }

                    foreach (string inputFilePath in inputDirectoryFiles)
                    {
                        string fileName = Path.GetFileName(inputFilePath);
                        string targetFilePath = Path.Combine(outputDirectory, "img", fileName);
                        
                        if (!File.Exists(targetFilePath))
                            File.Copy(inputFilePath, targetFilePath);
                    }

                }
            }
        }

        private static (string outputFilePathWpf, string outputFilePathWeb) GenerateHtmlWpfAndWebFiles(string inputFile, string outputDirectory, bool generateWeb=true)
        {            
            string[] lines = File.ReadAllLines(inputFile);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].TrimStart() == string.Empty)
                    lines[i] = lines[i].TrimStart();
            }

            string inputContentMd = String.Join("\n", lines);

            // var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            string postContentHtml = Markdown.ToHtml(inputContentMd, pipeline);            

            // Modify HTML
            // Correct links href
            postContentHtml = postContentHtml.Replace(@".md"">", @".html"">");

            // Generate WPF files
            string outputFilePathWpf = Path.Combine(outputDirectory, "wpf", Path.GetFileName(inputFile).Replace(".md", ".html"));            
            Html.GenerateHtmlFileWpf(outputFilePathWpf, postContentHtml);

            // Generate WEB files
            string outputFilePathWeb = "";
            if (generateWeb)
            {
                outputFilePathWeb = Path.Combine(outputDirectory, "web", Path.GetFileName(inputFile).Replace(".md", ".html"));
                Html.GenerateHtmlFileWeb(outputFilePathWeb, postContentHtml);
            }

            return (outputFilePathWpf, outputFilePathWeb);

        }

    }
}
