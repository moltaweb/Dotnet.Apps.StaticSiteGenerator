using System;
using System.IO;

namespace Ssg.Wpf.Data
{
    public class DbFile
    {
        public static void SaveSelectedFile(string fileName)
        {
            string dbFile = Path.Combine("Data", "dbfile.txt");

            File.WriteAllText(dbFile, fileName);
        }

        internal static string LoadSelectedFile()
        {
            string dbFile = Path.Combine("Data", "dbfile.txt");

            return File.ReadAllText(dbFile);
        }
    }
}
