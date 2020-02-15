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
using Serilog;
using Serilog.Core;
using ThunderbirdTray.Win32;
using System.Text;

namespace ThunderbirdTray
{
    public partial class TrayBird : ApplicationContext
    {
        public static readonly string Guid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;
        private static readonly string thunderbirdMainWindowClassName = "MozillaWindowClass";
        private static readonly string thunderbirdMainWindowTextEndsWith = " Mozilla Thunderbird";

        private Logger log;

        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenu;

        private Task initTask;
        private Process thunderbirdProcess;
        private IntPtr thunderbirdMainWindowHandle;
        private bool thunderbirdShown = true;
        private WindowVisualState lastVisualState = WindowVisualState.Minimized;
        private AutomationElement thunderbirdAutomationElement;
        private User32.ShowWindowType restoreState
        {
            get
            {
                if (lastVisualState == WindowVisualState.Minimized)
                {
                    if (thunderbirdMainWindowHandle != null)
                    {
                        if ((User32.GetWindowPlacement(thunderbirdMainWindowHandle).flags & User32.WPF_RESTORETOMAXIMIZED) > 0)
                        {
                            return User32.ShowWindowType.SW_MAXIMIZE;
                        }
                        else
                        {
                            return User32.ShowWindowType.SW_NORMAL;
                        }
                    }
                    else
                    {
                        return User32.ShowWindowType.SW_NORMAL;
                    }
                }
                else
                {
                    return lastVisualState == WindowVisualState.Maximized ? User32.ShowWindowType.SW_MAXIMIZE : User32.ShowWindowType.SW_NORMAL;
                }
            }
        }

        public TrayBird(bool debugLog=true)
        {
            var logConfig = new LoggerConfiguration()
                .WriteTo.File("log.txt");

            if (debugLog)
            {
                logConfig = logConfig.MinimumLevel.Debug();
            }
            else
            {
                logConfig = logConfig.MinimumLevel.Warning();
            }

            log = logConfig.CreateLogger();

            log.Information("Started TrayBird.");
            ToolStripMenuItem showMenuItem = new ToolStripMenuItem("Show / Hide Thunderbird", null, new EventHandler(ToggleShowThunderbird));
            showMenuItem.Font = new Font(showMenuItem.Font, showMenuItem.Font.Style | FontStyle.Bold);
            showMenuItem.Enabled = false;
            //ToolStripMenuItem configMenuItem = new ToolStripMenuItem("Configuration", null, new EventHandler(ShowConfig));
            //configMenuItem.Enabled = false;
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Exit", null, new EventHandler(Exit));
            contextMenu = new ContextMenuStrip();
            contextMenu.Items.AddRange(new ToolStripMenuItem[] { showMenuItem, /*configMenuItem,*/ exitMenuItem });
            notifyIcon = new NotifyIcon();
            notifyIcon.Text = "ThunderbirdTray - Starting up...";
            notifyIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            notifyIcon.ContextMenuStrip = contextMenu;
            notifyIcon.Click += NotifyIcon_Click;
            notifyIcon.Visible = true;
            log.Debug("Context menu created.");

            Initialise();
            log.Information("Program started.");
        }

        private void Initialise()
        {

            if (initTask == null)
            {
                initTask = Task.Run(async () =>
                {
                    log.Debug("Starting initilisation task.");
                    using (var process = Process.GetProcessesByName("thunderbird").FirstOrDefault())
                    {
                        if (process == null)
                        {
                            log.Debug("Thunderbird process not detected. {@process}", process);
                            StartThunderbird();
                        }
                    }

                    int retries = 0;
                    while (!HookThunderbird())
                    {
                        await Task.Delay(100);
                        retries += 1;
                    }
                    log.Debug("Hook took {@retries} retries.", retries);
                });

                contextMenu.Items[0].Enabled = true;
                notifyIcon.Text = "ThunderbirdTray - Active";

                log.Information("Initialised.");
            }
            else
            {
                log.Information("Already initialised.");
            }
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            var mouseEvent = e as MouseEventArgs;
            if (mouseEvent != null)
            {
                var button = mouseEvent.Button;
                log.Debug("NotifyIcon clicked {@button}.", button);
                if (button == MouseButtons.Left)
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
                    log.Debug("Toggle hide.");
                    HideThunderbird();
                }
                else
                {
                    log.Debug("Toggle show.");
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
            log.Information("Thunderbird executable assumed at {@filename}", filename);

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
                log.Information("Starting Thunderbird...");
                process.Start();
            }
        }

        private void ShowThunderbird()
        {
            if (thunderbirdMainWindowHandle != IntPtr.Zero)
            {
                log.Debug("Showing Thunderbird with last state as {@lastVisualState}.", lastVisualState);
                User32.ShowWindow(thunderbirdMainWindowHandle, restoreState);
                User32.SetForegroundWindow(thunderbirdMainWindowHandle);
                thunderbirdShown = true;
            }
        }

        private void HideThunderbird()
        {
            if (thunderbirdMainWindowHandle != IntPtr.Zero)
            {
                log.Debug("Hiding Thunderbird with last state as {@lastVisualState}.", lastVisualState);
                User32.ShowWindow(thunderbirdMainWindowHandle, User32.ShowWindowType.SW_HIDE);
                thunderbirdShown = false;
            }
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
            log.Information("Hooking on to Thunderbird...");
            if (thunderbirdProcess != null && thunderbirdMainWindowHandle != IntPtr.Zero)
            {
                log.Information("Already hooked!");
                // Already hooked
                return true;
            }

            thunderbirdProcess = Process.GetProcessesByName("thunderbird").FirstOrDefault();

            if (thunderbirdProcess == null)
            {
                log.Error("Cannot find the Thunderbird process to hook to.");
                // Hook failure, process does not exist
                return false;
            }

            thunderbirdMainWindowHandle = FindMainThunderbirdWindow(thunderbirdProcess);

            if (thunderbirdMainWindowHandle == IntPtr.Zero)
            {
                log.Error("Cannot find Thunderbird's main window.");
                // Main window is lost (hidden)
                return false;
            }
            else
            {
                log.Debug("Hooked on to window handle {@thunderbirdMainWindowHandle}", thunderbirdMainWindowHandle);
            }

            thunderbirdProcess.EnableRaisingEvents = true;
            thunderbirdProcess.Exited += Thunderbird_Exited;
            thunderbirdAutomationElement = AutomationElement.FromHandle(thunderbirdMainWindowHandle);
            Automation.AddAutomationPropertyChangedEventHandler(
                thunderbirdAutomationElement,
                TreeScope.Element,
                Thunderbird_VisualStateChanged,
                new AutomationProperty[] { WindowPattern.WindowVisualStateProperty });

            log.Debug("Attached event handlers for window.");

            // If not already hidden and is currently minimised, hide immediately
            var isIconic = User32.IsIconic(thunderbirdMainWindowHandle);
            if (thunderbirdShown && isIconic)
            {
                log.Information("Thunderbird is already minimised, hiding now. {@thunderbirdShown}, {@isIconic}.", thunderbirdShown, isIconic);
                HideThunderbird();
            }
            else
            {
                lastVisualState = (WindowVisualState)thunderbirdAutomationElement.GetCurrentPropertyValue(WindowPattern.WindowVisualStateProperty);
            }
            log.Debug("Setting visual state as {@lastVisualState}.", lastVisualState);

            return true;
        }

        private IntPtr FindMainThunderbirdWindow(Process thunderbirdProcess)
        {
            IntPtr thunderbirdWindow = IntPtr.Zero;
            var thunderbirdProcessId = thunderbirdProcess.Id;

            User32.EnumWindows((IntPtr hWnd, IntPtr lParam) =>
            {
                int windowProcessId = 0;
                User32.GetWindowThreadProcessId(hWnd, out windowProcessId);

                // If process ids don't match or it is a child window, return true for a new window.
                if (windowProcessId != thunderbirdProcessId || User32.GetWindow(hWnd, User32.GetWindowType.GW_OWNER) != IntPtr.Zero)
                {
                    return true;
                }

                // Class names can only be a max length of 256 characters
                var classNameBuilder = new StringBuilder(256);
                var classNameLength = User32.GetClassName(hWnd, classNameBuilder, classNameBuilder.Capacity);
                var className = classNameBuilder.ToString();

                if (className != thunderbirdMainWindowClassName)
                {
                    return true;
                }

                int length = User32.GetWindowTextLength(hWnd);
                var windowTextBuilder = new StringBuilder(length + 1);
                var windowTextLength = User32.GetWindowText(hWnd, windowTextBuilder, windowTextBuilder.Capacity);
                var windowText = windowTextBuilder.ToString();

                if (!windowText.EndsWith(thunderbirdMainWindowTextEndsWith))
                {
                    return true;
                }

                thunderbirdWindow = hWnd;

                // Ends enumeration
                return false;
            }, IntPtr.Zero);

            return thunderbirdWindow;
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

            log.Debug("Detected visual state change to {@visualState}.", visualState);

            if (visualState == WindowVisualState.Minimized)
            {
                HideThunderbird();
            }
            else
            {
                thunderbirdShown = true;
                lastVisualState = visualState;
            }
        }

        private void Thunderbird_Exited(object sender, EventArgs e)
        {
            log.Information("Thunderbird has exited.");
            Exit(this, new EventArgs());
        }

        private void UnhookThunderbird()
        {
            log.Information("Unhooking Thunderbird...");

            if (thunderbirdProcess != null)
            {
                if (thunderbirdMainWindowHandle != IntPtr.Zero)
                {
                    if (!thunderbirdShown)
                    {
                        log.Debug("Previously hidden, showing.");
                        // If previously hidden, show to avoid bug
                        User32.ShowWindow(thunderbirdMainWindowHandle, User32.ShowWindowType.SW_NORMAL);
                    }

                    thunderbirdMainWindowHandle = IntPtr.Zero;
                }

                if (thunderbirdAutomationElement != null)
                {
                    Automation.RemoveAllEventHandlers();
                    thunderbirdAutomationElement = null;
                }
                thunderbirdProcess.Dispose();
                thunderbirdProcess = null;
                initTask = null;
            }
        }

        private void Exit(object sender, EventArgs e)
        {
            // We must manually tidy up and remove the icon before we exit.
            // Otherwise it will be left behind until the user mouses over.
            log.Information("Shutting down ThunderbirdTray");
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
    }
}
