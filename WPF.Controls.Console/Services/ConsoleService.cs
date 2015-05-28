using System;
using System.Collections.Generic;

using WPF.Controls.Console.Internal;

namespace WPF.Controls.Console.Services
{
    public class ConsoleService
    {
        private string _consoleName;

        public ConsoleService(string consoleName)
        {
            if (consoleName == null)
                throw new ArgumentNullException("consoleName");
            if (string.IsNullOrWhiteSpace(consoleName))
                throw new ArgumentException("Argument 'consoleName' should be a valid console name.");

            _consoleName = consoleName;
        }

        public void Clear()
        {
            foreach (var console in Consoles)
                console.Clear();
        }

        public void WriteLine(string text)
        {
            foreach (var console in Consoles)
                console.WriteLine(text);
        }

        public void Write(string text)
        {
            foreach (var console in Consoles)
                console.Write(text);
        }

        private IEnumerable<ConsoleControl> Consoles
        {
            get
            {
                return ConsoleRegister.Retrieve(_consoleName);
            }
        }
    }
}
