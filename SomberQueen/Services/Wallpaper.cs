using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SomberQueen.Services
{
    public class Wallpaper
    {
        private const int SPI_SETDESKWALLPAPER = 0x0014;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDCHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]

        private static extern int SystemParametersInfo(
            int user_Action, 
            int user_Param, 
            string lvpParam, 
            int fuWinIni);

        public static void SetWallpaper(string wPath)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, wPath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }

    }
}
