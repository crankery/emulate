using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crankery.Emulate.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var computer = new Computer();
            computer.Start();
        }
    }
}
