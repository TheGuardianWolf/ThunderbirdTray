using System;
using System.Threading;
using System.Windows.Forms;

namespace ThunderbirdTray
{
    static class Program
    {
        static string appGuid = "299462ce-7579-45d0-afcc-8ea50db9a11b";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("Instance already running");
                    return;
                }

                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new TrayBird());
            }
        }
    }
}
