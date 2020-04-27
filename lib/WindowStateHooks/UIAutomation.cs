using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace ThunderbirdTray.WindowStateHooks
{
    public class UIAutomation : IWindowStateHook 
    { 

        public event EventHandler<WindowStateChangeEventArgs> WindowStateChange;

        private AutomationElement automationElement;

        public bool Hook(IntPtr windowHandle)
        {
            if (automationElement == null)
            {
                automationElement = AutomationElement.FromHandle(windowHandle);
                Automation.AddAutomationPropertyChangedEventHandler(
                    automationElement,
                    TreeScope.Element,
                    OnVisualStateChange,
                    new AutomationProperty[] { WindowPattern.WindowVisualStateProperty });
                return true;
            }
            return false;
        }

        private void OnVisualStateChange(object sender, AutomationPropertyChangedEventArgs e)
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

            WindowStateChange?.Invoke(this, new WindowStateChangeEventArgs((WindowState)visualState));
        }

        public bool Unhook()
        {
            if (automationElement != null)
            {
                Automation.RemoveAutomationPropertyChangedEventHandler(automationElement, OnVisualStateChange);
                automationElement = null;
                return true;
            }
            return false;
        }
    }
}
