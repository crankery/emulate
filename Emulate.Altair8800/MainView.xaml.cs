namespace Crankery.Emulate.Altair8800
{
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// The main view.
    /// </summary>
    public partial class MainView : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainView"/> class.
        /// </summary>
        public MainView()
        {
            InitializeComponent();

            ViewModel = new MainViewModel(Dispatcher);
            DataContext = ViewModel;
       }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public MainViewModel ViewModel { get; private set; }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ViewModel.Shutdown();
        }
   }
}
