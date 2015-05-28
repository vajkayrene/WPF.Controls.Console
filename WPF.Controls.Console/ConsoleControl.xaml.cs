using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPF.Controls.Console.Helpers;
using WPF.Controls.Console.Internal;

namespace WPF.Controls.Console
{
    /// <summary>
    /// Interaction logic for ConsoleControl.xaml
    /// </summary>
    public partial class ConsoleControl : UserControl
    {
        int _lastRemovableCharacter = 0;

        public static readonly DependencyProperty ExecuteCommandProperty =
            DependencyProperty.Register("ExecuteCommand", typeof(ICommand), typeof(ConsoleControl), 
                new PropertyMetadata(null));

        public static new readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(ConsoleControl),
                new PropertyMetadata(Brushes.Black, BackgroundPropertyChanged));

        public static new readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(ConsoleControl),
                new PropertyMetadata(Brushes.White, ForegroundPropertyChanged));

        public static new readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(ConsoleControl),
                new PropertyMetadata(SystemFonts.MessageFontFamily, FontFamilyPropertyChanged));

        public static new readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(ConsoleControl),
                new PropertyMetadata(SystemFonts.MessageFontSize, FontSizePropertyChanged));

        public static new readonly DependencyProperty FontStyleProperty =
            DependencyProperty.Register("FontStyle", typeof(FontStyle), typeof(ConsoleControl),
                new PropertyMetadata(SystemFonts.MessageFontStyle, FontStylePropertyChanged));

        public static new readonly DependencyProperty FontWeightProperty =
            DependencyProperty.Register("FontWeight", typeof(FontWeight), typeof(ConsoleControl),
                new PropertyMetadata(SystemFonts.MessageFontWeight, FontWeightPropertyChanged));

        public static readonly DependencyProperty CommandLinePrefixProperty =
            DependencyProperty.Register("CommandLinePrefix", typeof(string), typeof(ConsoleControl),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty WelcomeMessageProperty =
            DependencyProperty.Register("WelcomeMessage", typeof(string), typeof(ConsoleControl),
                new PropertyMetadata(string.Empty));

        public ConsoleControl()
        {
            InitializeComponent();
        }

        public ICommand ExecuteCommand
        {
            get { return (ICommand)GetValue(ExecuteCommandProperty); }
            set { SetValue(ExecuteCommandProperty, value); }
        }

        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public new Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public new FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public new double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public new FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public new FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public string CommandLinePrefix
        {
            get { return (string)GetValue(CommandLinePrefixProperty); }
            set { SetValue(CommandLinePrefixProperty, value); }
        }

        public string WelcomeMessage
        {
            get { return (string)GetValue(WelcomeMessageProperty); }
            set { SetValue(WelcomeMessageProperty, value); }
        }

        public void Clear()
        {
            CmdLine.Text = string.Empty;
        }

        public void WriteLine(string text)
        {
            CmdLine.Text += text + Environment.NewLine;
        }

        public void Write(string text)
        {
            CmdLine.Text += text;
        }

        static void BackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ControlHelpers.PropertyChanged(d, e, (ctrl, val) => ctrl.CmdLine.Background = (Brush)val.NewValue);
        }

        static void ForegroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ControlHelpers.PropertyChanged(d, e, (ctrl, val) => ctrl.CmdLine.Foreground = (Brush)val.NewValue);
        }

        static void FontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ControlHelpers.PropertyChanged(d, e, (ctrl, val) => ctrl.CmdLine.FontFamily = (FontFamily)val.NewValue);
        }

        static void FontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ControlHelpers.PropertyChanged(d, e, (ctrl, val) => ctrl.CmdLine.FontSize = (double)val.NewValue);
        }

        static void FontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ControlHelpers.PropertyChanged(d, e, (ctrl, val) => ctrl.CmdLine.FontStyle = (FontStyle)val.NewValue);
        }

        static void FontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ControlHelpers.PropertyChanged(d, e, (ctrl, val) => ctrl.CmdLine.FontWeight = (FontWeight)val.NewValue);
        }

        string CmdLinePrefix
        {
            get { return string.Format("{0}>", CommandLinePrefix); }
        }

        void InitializeConsole()
        {
            if (!string.IsNullOrEmpty(WelcomeMessage))
                CmdLine.Text += WelcomeMessage + Environment.NewLine;

            CmdLine.Text += CmdLinePrefix;

            UpdateConsoleState();
        }

        void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ConsoleRegister.Register(this);

            InitializeConsole();

            CmdLine.Focus();
        }

        void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ConsoleRegister.Unregister(this);
        }

        void CmdLine_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((CmdLine.CaretIndex < _lastRemovableCharacter && !IsNavigationKey(e.Key)) ||
                (CmdLine.CaretIndex == _lastRemovableCharacter && IsDeleteKey(e.Key)))
            {
                e.Handled = true;
            }
        }

        void CmdLine_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var consoleLine = CmdLine.Text.Substring(_lastRemovableCharacter);

                CmdLine.Text += Environment.NewLine;

                RaiseExecuteCommand(consoleLine);

                CmdLine.Text += CmdLinePrefix;

                UpdateConsoleState();

                e.Handled = true;
            }
        }

        void RaiseExecuteCommand(string command)
        {
            if (ExecuteCommand != null)
                ExecuteCommand.Execute(command);
        }

        void UpdateConsoleState()
        {
            CmdLine.CaretIndex = CmdLine.Text.Length;
            _lastRemovableCharacter = CmdLine.Text.Length;
        }

        bool IsNavigationKey(Key key)
        {
            return key == Key.Up || key == Key.Down || key == Key.Right || key == Key.Left;
        }

        bool IsDeleteKey(Key key)
        {
            return key == Key.Back || key == Key.Delete;
        }
    }
}
