using System;
using System.Windows;

namespace WPF.Controls.Console.Helpers
{
    static class ControlHelpers
    {
        public static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, Action<ConsoleControl, DependencyPropertyChangedEventArgs> action)
        {
            var control = (ConsoleControl)d;
            if (control != null)
            {
                action(control, e);
            }
        }
    }
}
