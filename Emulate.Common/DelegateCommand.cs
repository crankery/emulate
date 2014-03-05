namespace Crankery.Emulate.Common
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// A delegate command.
    /// </summary>
    public sealed class DelegateCommand : ICommand
    {
        /// <summary>
        /// The delegate to wrap.
        /// </summary>
        public delegate void SimpleEventHandler();

        private SimpleEventHandler handler;

        private bool isEnabled = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public DelegateCommand(SimpleEventHandler handler)
        {
            this.handler = handler;
        }

        void ICommand.Execute(object arg)
        {
            this.handler();
        }

        bool ICommand.CanExecute(object arg)
        {
            return this.IsEnabled;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the command is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the command is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set
            {
                this.isEnabled = value;
                this.OnCanExecuteChanged();
            }
        }

        private void OnCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}