namespace Crankery.Emulate.Altair8800
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
