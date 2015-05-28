using System;
using System.Linq;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using WPF.Controls.Console.Services;

namespace WPF.Controls.Client
{
    class MainWindowViewModel : ViewModelBase
    {
        readonly ConsoleService _consoleService;

        public MainWindowViewModel(ConsoleService consoleService)
        {
            _consoleService = consoleService;

            ExecuteCommand = new RelayCommand<string>(Execute);
        }

        public ICommand ExecuteCommand
        {
            get;
            private set;
        }

        private void Execute(string commandLine)
        {
            var parts = commandLine.Split(' ');
            var command = parts[0];

            switch (command)
            {
                case "echo":
                    _consoleService.WriteLine(parts.Skip(1).Aggregate((s1, s2) => s1 + " " + s2));
                    break;

                case "time":
                    _consoleService.WriteLine(DateTime.Now.ToLongTimeString());
                    break;

                case "date":
                    _consoleService.WriteLine(DateTime.Now.ToLongDateString());
                    break;

                case "cls":
                case "clear":
                    _consoleService.Clear();
                    break;

                case "help":
                    _consoleService.WriteLine("Supported commands:");
                    _consoleService.WriteLine("\techo [text] - display text on next line");
                    _consoleService.WriteLine("\ttime        - display current time");
                    _consoleService.WriteLine("\tdate        - display current date");
                    _consoleService.WriteLine("\tclear       - clear command line");
                    break;

                default:
                    _consoleService.WriteLine("Try typing 'help' into the console and pressing enter.");
                    break;
            }
        }
    }
}
