using System;
using System.Collections.Generic;

namespace WPF.Controls.Console.Internal
{
    static class ConsoleRegister
    {
        static readonly Dictionary<string, List<ConsoleControl>> _register;
        static ConsoleRegister()
        {
            _register = new Dictionary<string, List<ConsoleControl>>();
        }

        public static void Register(ConsoleControl control)
        {
            List<ConsoleControl> list;
            if (_register.ContainsKey(control.Name))
            {
                list = _register[control.Name];
            }
            else
            {
                list = new List<ConsoleControl>();
                _register[control.Name] = list;
            }
            list.Add(control);
        }

        public static IEnumerable<ConsoleControl> Retrieve(string consoleName)
        {
            if (!_register.ContainsKey(consoleName))
                throw new InvalidOperationException("Console should be loaded first.");

            return _register[consoleName];
        }

        public static void Unregister(ConsoleControl control)
        {
            if (_register.ContainsKey(control.Name))
            {
                var list = _register[control.Name];
                list.Remove(control);
            }
        }
    }
}
