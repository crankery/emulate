using Crankery.Emulate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Crankery.Emulate.Console
{
    public class SerialInputOutput
    {
        private Queue<byte> data = new Queue<byte>();
        private ManualResetEvent stop;
        private Thread bgThread;

        public SerialInputOutput(Devices devices)
            : this(devices, 0x10, 0x11)
        {
        }

        public SerialInputOutput(Devices devices, byte statusPort, byte dataPort)
        {
            devices.RegisterInputPort(
                statusPort,
                () =>
                {
                    // the lowest bit (bit 0) means 'yes, there's input'
                    // bit 1 should be set to indicate that the serial io is present
                    return (byte)(data.Count == 0 ? 0xfe : 0xff);
                });

            devices.RegisterOutputPort(
                statusPort,
                (b) =>
                {
                    if (b == 0x3)
                    {
                        data.Clear();
                    }
                });

            devices.RegisterInputPort(
                dataPort,
                () =>
                {
                    return data.Count == 0 ? (byte)0x00 : data.Dequeue();
                });

            devices.RegisterOutputPort(
                dataPort, 
                (b) => 
                {
                    System.Console.Write(Convert.ToChar(b & 0x7f));
                });
        }

        public void Startup()
        {
            System.Console.TreatControlCAsInput = true;

            stop = new ManualResetEvent(false);
            bgThread = new Thread(
                () =>
                {
                    while (!stop.WaitOne(50))
                    {
                        var key = System.Console.ReadKey(true);
                        data.Enqueue(Convert.ToByte(key.KeyChar));
                    }
                });

            bgThread.Start();
        }

        public void Shutdown()
        {
            stop.Set();
            bgThread.Join();
        }
    }
}
