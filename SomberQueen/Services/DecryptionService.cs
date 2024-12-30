using System;
using System.IO;
using System.Security.Cryptography;
using SomberQueen.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SomberQueen.Services
{ public class DecryptionService
    {
        private readonly DBHelper _db;

        public DecryptionService(DBHelper db)
        {
            _db = db;
        }

        public byte[] GetDecryptionKey(string username)
        {
            try
            {
                string query = @"
                    SELECT decryption_key 
                    FROM users 
                    WHERE username = @username";

                var decryptionKey = _db.QueryFirstOrDefault<byte[]>(query, new { username });

                if (decryptionKey == null)
                {
                    Logger.WriteLog($"Decryption key bulunamadı: {username}");
                    throw new Exception("Decryption key bulunamadı");
                }

                Logger.WriteLog($"Decryption key başarıyla alındı: {username}");
                return decryptionKey;
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"Decryption key alınırken hata: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<string> GetEncryptedFiles(string username)
        {
            try
            {
                string query = @"
                    SELECT file_name 
                    FROM encrypted_files 
                    WHERE username = @username";

                return _db.Query<string>(query, new { username });
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"Şifrelenmiş dosya listesi alınırken hata: {ex.Message}");
                throw;
            }
        }
   }
}
