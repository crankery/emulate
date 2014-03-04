namespace Crankery.Emulate.Altair8800
{
    using Crankery.Emulate.Core.Intel8080;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class Devices : IDevices
    {
        private readonly Dictionary<byte, Func<byte>> inputPorts = new Dictionary<byte, Func<byte>>();
        private readonly Dictionary<byte, Action<byte>> outputPorts = new Dictionary<byte, Action<byte>>();

        public byte Read(byte port)
        {
            Func<byte> read;
            if (inputPorts.TryGetValue(port, out read))
            {
                return read();
            }

            Trace.WriteLine(string.Format("Read from unmapped port: {0:x2}", port));

            return 0x00;
        }

        public void Write(byte port, byte value)
        {
            Action<byte> write;
            if (outputPorts.TryGetValue(port, out write))
            {
                write(value);
            }
            else
            {
                Trace.WriteLine(string.Format("Write to unmapped port: {0:x2}", port));
            }
        }

        public void RegisterInputPort(byte port, Func<byte> read)
        {
            inputPorts[port] = read;
        }

        public void RegisterOutputPort(byte port, Action<byte> write)
        {
            outputPorts[port] = write;
        }
    }
}
