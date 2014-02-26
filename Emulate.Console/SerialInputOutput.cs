﻿namespace Crankery.Emulate.Console
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// A MITS SIO2 card with two serial ports.
    /// The first serial port is the local terminal device (keyboard and display)
    /// The second isn't connected to anything (would normally be a printer).
    /// This occupies ports 0x10-0x13 by default.
    /// </summary>
    public class SerialInputOutput
    {
        private readonly Queue<byte> data = new Queue<byte>();
        
        public delegate void SendByte(object sender, byte b);

        /// <summary>
        /// Send a character to the terminal's display.
        /// </summary>
        public event SendByte Send = delegate { };

        public SerialInputOutput(Devices devices)
            : this(devices, 0x10, 0x11, 0x12, 0x13)
        {
        }

        public SerialInputOutput(Devices devices, byte statusPort0, byte dataPort0, byte statusPort1, byte dataPort1)
        {
            devices.RegisterInputPort(
                statusPort0,
                () =>
                {
                    // the lowest bit (bit 0) means 'yes, there's input'
                    // bit 1 should be set to indicate that the serial io is present
                    return (byte)(data.Count == 0 ? 0xfe : 0xff);
                });

            devices.RegisterOutputPort(
                statusPort0,
                (b) =>
                {
                    if (b == 0x3)
                    {
                        data.Clear();
                    }
                });

            devices.RegisterInputPort(
                dataPort0,
                () =>
                {
                    return data.Count == 0 ? (byte)0x00 : data.Dequeue();
                });

            devices.RegisterOutputPort(
                dataPort0, 
                (b) => 
                {
                    Send(this, (byte)(b & 0x7f));
                });

            devices.RegisterInputPort(
                statusPort1,
                () =>
                {
                    // no data ready
                    return (byte)0xfe;
                });

            devices.RegisterOutputPort(
                statusPort1,
                (b) =>
                {
                });

            devices.RegisterInputPort(
                dataPort1,
                () =>
                {
                    // no data available on 2nd port
                    return (byte)0x00;
                });

            devices.RegisterOutputPort(
                dataPort1,
                (b) =>
                {
                });
        }

        /// <summary>
        /// Recieve a character from the keyboard
        /// </summary>
        /// <param name="b">The character (as a byte).</param>
        public void Receive(byte b)
        {
            data.Enqueue(b);
        }
    }
}
