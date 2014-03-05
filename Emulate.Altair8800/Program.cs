namespace Crankery.Emulate.Altair8800
{
    using System;

    /// <summary>
    /// The entry point.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        [STAThread]
        static void Main(string[] args)
        {
            var root = new MainView();
            root.ShowDialog();
        }
    }
}
