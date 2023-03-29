using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ssg.Core
{
    public class Website
    {
        public static void GenerateIndexFile(string filesDirectory)
        {
            string outputFile = Path.Combine(filesDirectory, "index.html");

            string outputContentHtml = "<ul>";
            string[] htmlFiles = Directory.GetFiles(filesDirectory);

            Array.Sort(htmlFiles);

            foreach (string htmlFile in htmlFiles)
            {
                string fileName = Path.GetFileName(htmlFile);
                outputContentHtml += $"<li><a href=\"{fileName}\">{fileName}</a></li>\n";
            }

            outputContentHtml += "</ul>";

            var outputHTML = Html.GetLayoutFileContent("index");

            int index = outputHTML.IndexOf("{{content}}");
            outputHTML.RemoveAt(index);
            outputHTML.Insert(index, outputContentHtml);
            
            File.WriteAllLines(outputFile, outputHTML.ToArray());

        }


        public static string UploadDirectoryFiles(string filesDirectory, FtpCredentials credentials, string subFolder=null)
        {
            string ftpUserName = credentials.FtpUserName;
            string ftpPassword = credentials.FtpPassword;
            string ftpAddress = credentials.FtpAddress;

            string localPath = (subFolder==null) ? filesDirectory : Path.Combine(filesDirectory, subFolder);
            string remotePath = (subFolder == null) ? $"{ftpAddress}/" : $"{ftpAddress}/{subFolder}/";

            string[] files = Directory.GetFiles(localPath);

            string errorMessage = "";

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string ftpUri = remotePath + fileName;

                try
                {
                    using (var client = new WebClient())
                    {
                        client.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                        client.UploadFile(ftpUri, WebRequestMethods.Ftp.UploadFile, file);
                    }
                }
                catch (Exception ex)
                {
                    errorMessage += $"\n- {fileName}";
                }

            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = "ERROR uploading the following files:" + errorMessage;
                return errorMessage;
            }
            else
            {
                string folderString = (subFolder == null) ? "OK" : $"{subFolder}";
                return $"{folderString} - Files uploaded correctly!\n";
            }
        }

        public static bool UploadSingleFile(string localFilePath, FtpCredentials credentials)
        {
            bool output = true;

            string ftpUserName = credentials.FtpUserName;
            string ftpPassword = credentials.FtpPassword;
            string ftpAddress = credentials.FtpAddress;

            string fileName = Path.GetFileName(localFilePath);
            string ftpUri = ftpAddress + "/" + fileName;            

            try
            {
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                    client.UploadFile(ftpUri, WebRequestMethods.Ftp.UploadFile, localFilePath);
                }
            }
            catch (Exception ex)
            {
                output = false;
            }

            return output;

        }
    }
}
