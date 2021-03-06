﻿namespace Crankery.Emulate.Core.Tests.Intel8080
{
    using System;
    using Crankery.Emulate.Core.Intel8080;

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
