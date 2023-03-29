using System.Collections.Generic;
using System.IO;

namespace Ssg.Core
{
    public static class ExtensionMethods
    {
        public static void InsertCss(this List<string> content)
        {
            // Read CSS code from file
            AppConfiguration config = new AppConfiguration();
            string templatePath = config.TemplatePath;
            string cssFile = Path.Combine(templatePath, "assets", "wpf.css");
            string cssContent = File.ReadAllText(cssFile);

            // Insert CSS in the output HTML
            int index = content.IndexOf("{{css}}");
            content.RemoveAt(index);
            content.Insert(index, cssContent);
        }

        public static void InsertToc(this List<string> content, string htmlContent)
        {
            int indexCss = content.IndexOf("{{sidebar-toc}}");
            content.RemoveAt(indexCss);
            content.Insert(indexCss, htmlContent);
        }

        public static void InsertPostContent(this List<string> content, string htmlContent)
        {
            int index = content.IndexOf("{{content}}");
            content.RemoveAt(index);
            content.Insert(index, htmlContent);
        }

        public static void InsertTitleForWeb(this List<string> content, string title)
        {
            int index = content.IndexOf("{{title}}");
            content.RemoveAt(index);
            content.Insert(index, $"<title>{title}</title>");
        }

    }
}
