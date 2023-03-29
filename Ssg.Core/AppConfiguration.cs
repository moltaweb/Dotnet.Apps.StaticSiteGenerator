using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssg.Core
{
    public class AppConfiguration
    {

        private readonly string _templatePath;

        public AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();

            var appSetting = root.GetSection("folders");
            _templatePath = appSetting["templates"];
        }

        public string TemplatePath
        {
            get => _templatePath;
        }
    }
}
