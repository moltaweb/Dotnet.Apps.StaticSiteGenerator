using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ssg.Core
{
    public class Html
    {



        public static List<string> GetLayoutFileContent(string layoutType)
        {

            AppConfiguration config = new AppConfiguration();
            string templatePath = config.TemplatePath;

            // Get working Templates directories
            string layoutFilename = $"{layoutType.ToLower()}.html";
            string layoutFile = Path.Combine(templatePath, layoutFilename);

            // Get HTML Template content
            var layoutContent = File.ReadAllLines(layoutFile).ToList();

            return layoutContent;

        }

        public static (string sidebarHTML, string contentHtml) GenerateSidebarAndcontentHtml(string inputHtmlContent)
        {
            string[] lines = inputHtmlContent.Split("\n");

            char[] delimiters = new char[] { ' ', '\r', '\n' };
            string[] numberOfWords = inputHtmlContent.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            List<string> tocElements = new List<string>();

            int countH1 = 0;
            int countH2 = 0;

            //tocElements.Add("<ul>");
            tocElements.Add($"<h4>Table of Contents ({numberOfWords.Length} words)</h4>");

            // loop through every line of the input HTML to
            // 1- add the ID attribute in the <h1> or <h2> element
            // 2- add a new line in the sidebar TOC with the <a href> element
            for (int i = 0; i < lines.Length; i++)
            {

                if (lines[i].StartsWith("<h1>") || lines[i].StartsWith("<h2>"))
                {
                    string element = lines[i].Substring(1, 2);

                    // Extract title
                    string title = lines[i].Replace($"<{element}>", "").Replace($"</{element}>", "");

                    // add id to the HTML element
                    string id = "";
                    if (element == "h1")
                    {
                        countH1++;
                        id = $"{element}-{countH1}";
                    }
                    else
                    {
                        countH2++;
                        id = $"{element}-{countH2}";
                    }
                    lines[i] = lines[i].Replace($"<{element}>", $"<{element} id=\"{id}\">");

                    // add link in the sidebar TOC
                    tocElements.Add($"<a href=\"#{id}\" class=\"toc-{element} list-group-item list-group-item-action\">{title}</a>");
                }

            }

            //tocElements.Add("</ul>");

            // Create the TOC HTML from the tocElements list
            string tocHTML = "";

            foreach (string element in tocElements)
            {
                tocHTML += $"{element}\n";
            }       
            
            // Create the content HTML from the lines array
            string contentHtml = string.Join("\n", lines);

            return (tocHTML, contentHtml);

        }

        internal static void GenerateHtmlFileWeb(string outputFilePath, string htmlContent)
        {
            List<string> outputFileHTML = Html.GetLayoutFileContent("post-web");
            (string sidebarHTML, string contentHtml) = GenerateSidebarAndcontentHtml(htmlContent);

            outputFileHTML.InsertTitleForWeb(Path.GetFileName(outputFilePath));
            //outputFileHTML.InsertCss();
            outputFileHTML.InsertToc(sidebarHTML);
            outputFileHTML.InsertPostContent(contentHtml);            

            File.WriteAllLines(outputFilePath, outputFileHTML);
        }

        internal static void GenerateHtmlFileWpf(string outputFilePath, string htmlContent)
        {
            List<string> outputFileHTML = Html.GetLayoutFileContent("post-wpf");
            (string sidebarHTML, string contentHtml) = GenerateSidebarAndcontentHtml(htmlContent);

            outputFileHTML.InsertCss();
            outputFileHTML.InsertToc(sidebarHTML);
            outputFileHTML.InsertPostContent(contentHtml);

            File.WriteAllLines(outputFilePath, outputFileHTML);

        }
    }
}
