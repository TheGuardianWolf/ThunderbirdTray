using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThunderbirdTray.WindowStateHooks
{
    public interface IWindowStateHook
    {
        event EventHandler<WindowStateChangeEventArgs> WindowStateChange;

        bool Hook(IntPtr windowHandle);
        bool Unhook();
    }

    public enum WindowState: int
    {
        Normal = 0,
        Maximized = 1,
        Minimized = 2
    }

    public class WindowStateChangeEventArgs : EventArgs
    {
        public WindowState WindowState { get; }

        internal WindowStateChangeEventArgs(WindowState windowState)
        {
            WindowState = windowState;
        }
    }
}
