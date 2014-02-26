
namespace Crankery.Emulate.Console
{
    using System;
    using System.Windows.Input;
    using System.Windows.Threading;

    public class MainViewModel : ViewModel
    {
        private readonly Computer computer;
        private readonly TerminalDisplay terminalDisplay;
        private readonly TerminalKeyboard terminalKeyboard;
        private readonly Dispatcher dispatcher;

        private string title = "Altair 8800";

        public MainViewModel(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;

            computer = new Computer();
            terminalDisplay = new TerminalDisplay();
            terminalKeyboard = new TerminalKeyboard();

            // show bytes sent to the serial i/o to the display 
            computer.SerialInputOutput.Send +=
                (s, e) =>
                {
                    dispatcher.InvokeAsync(
                        (Action)(() => terminalDisplay.DisplayCharacter(e)));
                };

            // send keypresses to the serial i/o
            terminalKeyboard.KeyPressed +=
                (s, e) =>
                {
                    dispatcher.InvokeAsync(
                        (Action)(() => computer.SerialInputOutput.Receive(e)));
                };

            computer.Start();
        }

        public TerminalDisplay TerminalDisplay
        {
            get
            {
                return terminalDisplay;
            }
        }

        public TerminalKeyboard TerminalKeyboard
        {
            get
            {
                return terminalKeyboard;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        public void Shutdown()
        {
            computer.Stop();
        }
    }
}
