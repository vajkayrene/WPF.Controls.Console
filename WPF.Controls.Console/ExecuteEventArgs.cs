using System;

namespace WPF.Controls.Console
{
    public class ExecuteEventArgs : EventArgs
    {
        internal ExecuteEventArgs(string command)
        {
            Command = command;
        }

        public string Command { get; private set; }
    }
}
