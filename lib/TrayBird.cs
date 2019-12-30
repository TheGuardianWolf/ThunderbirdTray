using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Automation;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ThunderbirdTray
{
    public partial class TrayBird : ApplicationContext
    {
        public static readonly string Guid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;

        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenu;

        private Task initTask;
        private Process thunderbirdProcess;
        private IntPtr thunderbirdMainWindowHandle;
        private bool thunderbirdShown = true;
        private WindowVisualState lastVisualState = WindowVisualState.Normal;

        public TrayBird()
        {
            ToolStripMenuItem showMenuItem = new ToolStripMenuItem("Show / Hide Thunderbird", null, new EventHandler(ToggleShowThunderbird));
            showMenuItem.Font = new Font(showMenuItem.Font, showMenuItem.Font.Style | FontStyle.Bold);
            //ToolStripMenuItem configMenuItem = new ToolStripMenuItem("Configuration", null, new EventHandler(ShowConfig));
            //configMenuItem.Enabled = false;
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Exit", null, new EventHandler(Exit));
            contextMenu = new ContextMenuStrip();
            contextMenu.Items.AddRange(new ToolStripMenuItem[] { showMenuItem, /*configMenuItem,*/ exitMenuItem });


            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            notifyIcon.ContextMenuStrip = contextMenu;
            notifyIcon.Click += NotifyIcon_Click;
            notifyIcon.Visible = true;

            Initialise();
        }

        private void Initialise()
        {
            if (initTask == null)
            {
                initTask = Task.Run(async () =>
                {
                    using (var process = Process.GetProcessesByName("thunderbird").FirstOrDefault())
                    {
                        if (process == null || process.MainWindowHandle == IntPtr.Zero)
                        {
                            StartThunderbird();
                        }
                    }

                    int retries = 0;
                    while (!HookThunderbird())
                    {
                        if (retries >= 30)
                        {
                            throw new TimeoutException("Cannot hook on to the thunderbird process.");
                        }
                        await Task.Delay(100);
                        retries += 1;
                    }
                });
            }
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            var mouseEvent = e as MouseEventArgs;
            if (mouseEvent != null)
            {
                if (mouseEvent.Button == MouseButtons.Left)
                {
                    ToggleShowThunderbird();
                }
            }
        }

        private void ToggleShowThunderbird(object sender, EventArgs e)
        {
            ToggleShowThunderbird();
        }

        private void ToggleShowThunderbird()
        {
            if (thunderbirdProcess != null)
            {
                if (thunderbirdShown)
                {
                    HideThunderbird();
                }
                else
                {
                    ShowThunderbird();
                }
            }
        }

        public void StartThunderbird()
        {
            string filename = null;
            var filename64 = Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramW6432%"), "Mozilla Thunderbird", "thunderbird.exe");
            var filename86 = Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%"), "Mozilla Thunderbird", "thunderbird.exe");

            if (File.Exists(filename64))
            {
                filename = filename64;
            }
            else if (File.Exists(filename86))
            {
                filename = filename86;
            }

            using (var process = new Process 
                {
                    StartInfo =
                    {
                        FileName = filename,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Minimized,
                        UseShellExecute = true
                    }
                }
            )
            {
                process.Start();
            }
        }

        private void ShowThunderbird()
        {
            ShowWindow(thunderbirdMainWindowHandle, lastVisualState == WindowVisualState.Maximized ? 3 : 1);
            SetForegroundWindow(thunderbirdMainWindowHandle);
            thunderbirdMainWindowHandle = thunderbirdProcess.MainWindowHandle;
            thunderbirdShown = true;
        }

        private void HideThunderbird()
        {
            ShowWindow(thunderbirdMainWindowHandle, 0);
            thunderbirdShown = false;
        }

        private void ShowConfig(object sender, EventArgs e)
        {
            //if (configWindow.Visible)
            //{
            //    configWindow.Activate();
            //}
            //else
            //{
            //    configWindow.ShowDialog();
            //}
        }

        private bool HookThunderbird()
        {
            if (thunderbirdProcess != null && thunderbirdMainWindowHandle != IntPtr.Zero)
            {
                // Already hooked
                return true;
            }

            thunderbirdProcess = Process.GetProcessesByName("thunderbird").FirstOrDefault();

            if (thunderbirdProcess == null)
            {
                // Hook failure, process does not exist
                return false;
            }

            if (thunderbirdProcess.MainWindowHandle == IntPtr.Zero)
            {
                // Main window is lost (hidden)
                return false;
            }

            thunderbirdMainWindowHandle = thunderbirdProcess.MainWindowHandle;

            thunderbirdProcess.EnableRaisingEvents = true;
            thunderbirdProcess.Exited += Thunderbird_Exited;
            var thunderbirdElement = AutomationElement.FromHandle(thunderbirdMainWindowHandle);
            lastVisualState = (WindowVisualState)thunderbirdElement.GetCurrentPropertyValue(WindowPattern.WindowVisualStateProperty);
            Automation.AddAutomationPropertyChangedEventHandler(
                thunderbirdElement,
                TreeScope.Element,
                Thunderbird_VisualStateChanged,
                new AutomationProperty[] { WindowPattern.WindowVisualStateProperty });

            // If not already hidden and is currently minimised, hide immediately
            if (thunderbirdShown && IsIconic(thunderbirdMainWindowHandle))
            {
                HideThunderbird();
            }

            return true;
        }

        private void Thunderbird_VisualStateChanged(object sender, AutomationPropertyChangedEventArgs e)
        {
            WindowVisualState visualState = WindowVisualState.Normal;
            try
            {
                visualState = (WindowVisualState)e.NewValue;
            }
            catch (InvalidCastException)
            {
                // ignore
            }

            if (visualState == WindowVisualState.Minimized)
            {
                HideThunderbird();
            }
            else
            {
                // Update window handle in case something odd happens
                thunderbirdMainWindowHandle = thunderbirdProcess.MainWindowHandle;
                thunderbirdShown = true;
                lastVisualState = visualState;
            }
        }

        private void Thunderbird_Exited(object sender, EventArgs e)
        {
            Exit(this, new EventArgs());
        }

        private void UnhookThunderbird()
        {
            Automation.RemoveAllEventHandlers();

            if (thunderbirdProcess != null)
            {
                if (!thunderbirdShown)
                {
                    if (thunderbirdMainWindowHandle != IntPtr.Zero)
                    {
                        // If previously hidden, show minimised
                        ShowWindow(thunderbirdMainWindowHandle, 2);
                        thunderbirdMainWindowHandle = IntPtr.Zero;
                    }
                }

                thunderbirdProcess.Dispose();
                thunderbirdProcess = null;
            }
        }

        private void Exit(object sender, EventArgs e)
        {
            // We must manually tidy up and remove the icon before we exit.
            // Otherwise it will be left behind until the user mouses over.
            UnhookThunderbird();
            notifyIcon.Visible = false;
            Application.Exit();
        }

        protected override void Dispose(bool disposing)
        {
            // Clean up any components being used.
            if (disposing)
            {
                UnhookThunderbird();
            }

            base.Dispose(disposing);
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
    }
}
