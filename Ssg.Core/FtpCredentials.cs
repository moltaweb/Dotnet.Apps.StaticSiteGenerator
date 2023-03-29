using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssg.Core
{
    public class FtpCredentials
    {
        public FtpCredentials(string ftpUserName, string ftpPassword, string ftpAddress)
        {
            FtpUserName = ftpUserName;
            FtpPassword = ftpPassword;
            FtpAddress = ftpAddress;
        }

        public string FtpUserName { get; }
        public string FtpPassword { get; }
        public string FtpAddress { get; }
    }
}
