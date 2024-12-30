using SomberQueen.Utilities;
using System;
using System.IO;

namespace SomberQueen.Services
{
    public class DBService
    {
        private readonly DBHelper _dbHelper;

        public DBService(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }



        public void AddUser(string username, string role, byte[] decryptionKey)
        {
            _dbHelper.Execute(
                "INSERT INTO users (username, role, decryption_key, created_at) VALUES (@username, @role, @decryption_key, @created_at)",
                new
                {
                    username,
                    role,
                    decryption_key = decryptionKey,
                    created_at = DateTime.UtcNow
                }
            );
        }


        public void UpdateDecryptionKey(string username, byte[] newDecryptionKey)
        {
            _dbHelper.Execute(
                "UPDATE users SET decryption_key = @newDecryptionKey WHERE username = @username",
                new
                {
                    username,
                    newDecryptionKey
                }
            );
        }

        public void SaveDecryptionKey(string username, byte[] decryptionKey)
        {
            string query = "SELECT id FROM users WHERE username = @username";
            int? userId = _dbHelper.QueryFirstOrDefault<int?>(query, new { username });

            if (userId == null)
            {
                throw new Exception($"Kullanıcı bulunamadı: {username}");
            }

            string updateQuery = "UPDATE users SET decryption_key = @decryptionKey WHERE id = @userId";
            _dbHelper.Execute(updateQuery, new { decryptionKey = Convert.ToBase64String(decryptionKey), userId });
        }


        public void SaveFileNamesToDatabase(string folderPath, string username)
        {
            try
            {
                string[] files = Directory.GetFiles(folderPath);

                if (files.Length == 0)
                {
                    Logger.WriteLog("Kaydedilecek dosya bulunamadı.");
                    return;
                }

                string concatenatedFileNames = string.Join(", ", files.Select(Path.GetFileName));

                string query = @"
                    INSERT INTO encrypted_files (file_name, created_at, username) 
                    VALUES (@fileName, @createdAt, @username)";

                _dbHelper.Execute(query, new
                {
                    fileName = concatenatedFileNames,
                    createdAt = DateTime.UtcNow,
                    username = username
                });

                Logger.WriteLog($"Dosya adları veritabanına kaydedildi: {concatenatedFileNames}");
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"Dosya adları kaydedilirken hata oluştu: {ex.Message}");
                throw;
            }
        }




        /*
        public string GetUserId(string username)
        {
            string query = "SELECT user_id FROM users WHERE username = @username";
            return _dbHelper.QueryFirstOrDefault<string>(query, new { username });
        }
    
         */


    }
}
