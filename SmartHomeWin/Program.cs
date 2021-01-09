using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartHomeWin
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var home = new Home();
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                home.WindowState = FormWindowState.Minimized;
            }
            Application.Run(home);

        }
    }
}
