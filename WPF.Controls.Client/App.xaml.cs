using System.Windows;

using WPF.Controls.Console.Services;

namespace WPF.Controls.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var consoleService = new ConsoleService("MyConsoleControl");
            var mainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(consoleService)
            };
            mainWindow.Show();

            MainWindow = mainWindow;
        }
    }
}
