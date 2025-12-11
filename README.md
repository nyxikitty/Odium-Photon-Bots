# Some bots I made for UserNumber2's Bullet Force

# WinForms Suck

<img width="1584" height="862" alt="image" src="https://github.com/user-attachments/assets/2b7669e5-7605-4c18-9105-c7c1796b30ef" />

If you want to allocate a console, slap this in Program.cs

```cs
using System.Runtime.InteropServices;

namespace BLF_Odium_Network_Bots
{
    internal static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AllocConsole();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "BF Photon Bot Console";

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
```

