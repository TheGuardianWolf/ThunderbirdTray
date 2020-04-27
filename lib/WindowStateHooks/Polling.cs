using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ThunderbirdTray.Win32;

namespace ThunderbirdTray.WindowStateHooks
{
    public class Polling : IWindowStateHook
    {
        public event EventHandler<WindowStateChangeEventArgs> WindowStateChange;

        private IntPtr windowHandle = IntPtr.Zero;
        private WindowState? windowState = null;
        private Timer pollingTimer = new Timer(100);

        public bool Hook(IntPtr windowHandle)
        {
            if (this.windowHandle == IntPtr.Zero)
            {
                this.windowHandle = windowHandle;
                pollingTimer.AutoReset = true;
                pollingTimer.Elapsed += PollingTimer_Elapsed;
                pollingTimer.Start();
                return true;
            }
            return false;
        }

        private void PollingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var windowPlacement = User32.GetWindowPlacement(windowHandle);

            WindowState newWindowState;
            switch (windowPlacement.showCmd)
            {
                case User32.ShowWindowType.SW_MAXIMIZE:
                    newWindowState = WindowState.Maximized;
                    break;
                case User32.ShowWindowType.SW_HIDE:
                case User32.ShowWindowType.SW_SHOWMINNOACTIVE:
                case User32.ShowWindowType.SW_MINIMIZE:
                case User32.ShowWindowType.SW_SHOWMINIMIZED:
                    newWindowState = WindowState.Minimized;
                    break;
                case User32.ShowWindowType.SW_NORMAL:
                default:
                    newWindowState = WindowState.Normal;
                    break;
            }

            if (newWindowState != windowState)
            {
                windowState = newWindowState;
                if (windowState != null)
                {
                    WindowStateChange?.Invoke(this, new WindowStateChangeEventArgs(windowState ?? WindowState.Normal));
                }
            }
        }

        public bool Unhook()
        {
            if (windowHandle != IntPtr.Zero)
            {
                pollingTimer.Stop();
                pollingTimer.Elapsed -= PollingTimer_Elapsed;
                return true;
            }
            return false;
        }
    }
}
