using System;
using System.IO;

namespace SomberQueen.Utilities
{
    public static class Logger
    {
        static string LogFilePath = @"C:\Users\metin\OneDrive\Desktop\application_logs.txt";

        public static void WriteLog(string message)
        {
            try
            {
                using (var writer = new StreamWriter(LogFilePath, append: true))
                {
                    writer.WriteLine($"{DateTime.UtcNow}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Loglama sırasında bir hata oluştu: {ex.Message}");
            }
        }
    }
}
