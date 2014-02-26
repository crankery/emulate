namespace Crankery.Emulate.Console
{
    using System;

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var root = new MainView();
            root.ShowDialog();
        }
    }
}
