namespace Crankery.Emulate.Altair8800
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
       }

        public MainViewModel ViewModel { get; private set; }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ViewModel.Shutdown();
        }
   }
}
