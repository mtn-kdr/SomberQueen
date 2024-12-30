using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SomberQueen.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class FileHelper
    {
        public static List<string> GetFiles(string directoryPath, string searchPattern = "*.*")
        {
            return new List<string>(Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories));
        }

        public static string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public static void WriteFile(string filePath, string content)
        {

            File.WriteAllText(filePath, content);
        }


        public static FileInfo GetFileInfo(string filePath)
        {
            return new FileInfo(filePath);
        }


        public static long GetFileSize(string filePath)
        {
            var fileInfo = GetFileInfo(filePath);
            return fileInfo.Length;
        }


        public static string GetFileExtension(string filePath)
        {
            var fileInfo = GetFileInfo(filePath);
            return fileInfo.Extension;
        }

    }
}