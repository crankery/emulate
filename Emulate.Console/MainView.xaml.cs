namespace Crankery.Emulate.Console
{
    using System;
    using System.ComponentModel;
    using System.Timers;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            ViewModel = new MainViewModel(Dispatcher);
            DataContext = ViewModel;

            var timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 333),
                IsEnabled = true
            };

            timer.Tick += 
                (s, e) =>
                {
                    ViewModel.TerminalDisplay.ShowCursor ^= true;
                };
        }

        public MainViewModel ViewModel { get; private set; }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ViewModel.Shutdown();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var code = KeyTranslate.Translate(e.Key);

            if (code.HasValue)
            {
                e.Handled = true;
                ViewModel.TerminalKeyboard.OnKeyPressed(code.Value);
            }
        }
   }
}
