# Some bots I made for UserNumber2's Bullet Force

<img width="1317" height="770" alt="image" src="https://github.com/user-attachments/assets/a30a9e99-fea8-4547-b29f-e5f3abeb36a3" />

# WinForms Suck

https://github.com/user-attachments/assets/9de005ce-c8e9-489a-9f7d-1d25eb99378c


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

