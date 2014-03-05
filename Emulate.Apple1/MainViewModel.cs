namespace Crankery.Emulate.Apple1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Crankery.Emulate.Common;

    public class MainViewModel : ViewModel
    {
        private readonly Dispatcher dispatcher;
        private readonly ICommand reset;

        public MainViewModel(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;

            Terminal = new Terminal(40, 24, new Apple1Glyphs());
            Ports = new Ports();
            Computer = new Computer(Ports);

            Terminal.KeyPressed +=
                (s, e) =>
                {
                    dispatcher.InvokeAsync(
                        () => Ports.KeyPressed(e.Value));
                };

            Ports.DisplayCharacter +=
                (s, e) =>
                {
                    dispatcher.InvokeAsync(
                        () =>
                        {
                            var x = e.Value;
                            if (x == '\r')
                            {
                                Terminal.DisplayCharacter((byte)'\r');
                                Terminal.DisplayCharacter((byte)'\n');
                            }
                            else if (x >= ' ' && x < 0x7f)
                            {
                                Terminal.DisplayCharacter(x);
                            }
                        });
                };

            reset = new DelegateCommand(() => Computer.Reset());

            Computer.Start();
        }

        public Computer Computer { get; private set; }

        public Ports Ports { get; private set; }

        public Terminal Terminal { get; private set; }

        public ICommand Reset
        {
            get
            {
                return reset;
            }
        }

        public void Shutdown()
        {
            Computer.Stop();
        }
    }
}
