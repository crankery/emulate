namespace Crankery.Emulate.Altair8800
{
    using System;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Crankery.Emulate.Common;

    /// <summary>
    /// The main view/model.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class MainViewModel : ViewModel
    {
        private readonly Dispatcher dispatcher;
        private readonly ICommand reset;
        private Computer computer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        public MainViewModel(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;

            computer = new Computer();
            Terminal = new Terminal(80, 24, new Vt220Glyphs());
            
            // show bytes sent to the serial i/o to the display 
            computer.SerialInputOutput.Send +=
                (s, e) =>
                {
                    dispatcher.InvokeAsync(
                        () => Terminal.DisplayCharacter(e.Value));
                };

            // send keypresses to the serial i/o
            Terminal.KeyPressed +=
                (s, e) =>
                {
                    dispatcher.InvokeAsync(
                        () => computer.SerialInputOutput.Receive(e.Value));
                };

            reset = new DelegateCommand(() => computer.Reset());

            computer.Start();
        }

        /// <summary>
        /// Gets the terminal.
        /// </summary>
        /// <value>
        /// The terminal.
        /// </value>
        public Terminal Terminal
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the reset.
        /// </summary>
        /// <value>
        /// The reset.
        /// </value>
        public ICommand Reset
        {
            get
            {
                return reset;
            }
        }

        /// <summary>
        /// Shutdowns this instance.
        /// </summary>
        public void Shutdown()
        {
            computer.Stop();
        }
    }
}
