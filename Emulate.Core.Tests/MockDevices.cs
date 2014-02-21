using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crankery.Emulate.Core.Tests
{
    public class MockDevices : IDevices
    {
        public MockDevices()
        {
            Output = string.Empty;
        }

        public string Output { get; set; }

        public byte Read(byte port)
        {
            return 0;
        }

        public void Write(byte port, byte value)
        {
            if (port == 1)
            {
                Output += Convert.ToChar(value);
            }
        }
    }

}
