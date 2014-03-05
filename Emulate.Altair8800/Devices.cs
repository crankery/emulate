namespace Crankery.Emulate.Altair8800
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Crankery.Emulate.Core.Intel8080;

    /// <summary>
    /// The 8080 devices/ports.
    /// </summary>
    public class Devices : IDevices
    {
        private readonly Dictionary<byte, Func<byte>> inputPorts = new Dictionary<byte, Func<byte>>();
        private readonly Dictionary<byte, Action<byte>> outputPorts = new Dictionary<byte, Action<byte>>();

        /// <summary>
        /// Reads the specified port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Writes the specified port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="value">The value.</param>
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

        /// <summary>
        /// Registers the input port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="read">The read.</param>
        public void RegisterInputPort(byte port, Func<byte> read)
        {
            inputPorts[port] = read;
        }

        /// <summary>
        /// Registers the output port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="write">The write.</param>
        public void RegisterOutputPort(byte port, Action<byte> write)
        {
            outputPorts[port] = write;
        }
    }
}
