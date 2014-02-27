
namespace Crankery.Emulate.Console
{
    using System;
    using System.Windows.Input;
    using System.Windows.Threading;

    public class MainViewModel : ViewModel
    {
        private readonly Computer computer;
        private readonly Terminal terminal;
        private readonly Dispatcher dispatcher;
        private readonly ICommand reset;

        public MainViewModel(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;

            computer = new Computer();
            terminal = new Terminal();
            
            // show bytes sent to the serial i/o to the display 
            computer.SerialInputOutput.Send +=
                (s, e) =>
                {
                    dispatcher.InvokeAsync(
                        (Action)(() => terminal.DisplayCharacter(e)));
                };

            // send keypresses to the serial i/o
            terminal.KeyPressed +=
                (s, e) =>
                {
                    dispatcher.InvokeAsync(
                        (Action)(() => computer.SerialInputOutput.Receive(e)));
                };

            reset = new DelegateCommand(() => computer.Reset());

            computer.Start();
        }

        public Terminal Terminal
        {
            get
            {
                return terminal;
            }
        }

        public ICommand Reset
        {
            get
            {
                return reset;
            }
        }

        public void Shutdown()
        {
            computer.Stop();
        }
    }
}
