namespace Crankery.Emulate.Apple1
{
    using System;
    using System.IO;
    using System.Threading;
    using Crankery.Emulate.Core;
    using Crankery.Emulate.Core.Mos6502;
    using NLog;

    /// <summary>
    /// The Apple 1 computer.
    /// </summary>
    public class Computer: IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 1 MHz CPU 
        /// </summary>
        public const ulong CpuFrequency = 1000000;

        private ManualResetEvent attention;
        private Thread bgThread;

        public Computer(Ports ports)
        {
            Ports = ports;
            Memory = new MemoryMap(Ports);
            Cpu = new Mos6502Cpu(Memory);

            //Cpu.WriteLogMessage += (s, e) => System.Diagnostics.Trace.WriteLine(e.Message.ToString());
        }

        private Ports Ports { get; set; }

        private Mos6502Cpu Cpu { get; set; }

        private IMemory Memory { get; set; }

        public void Start()
        {
            attention = new ManualResetEvent(false);

            bgThread = new Thread(Run);
            bgThread.Start();
        }

        public void Stop()
        {
            attention.Set();
            bgThread.Join();
        }

        public void Reset()
        {
            Stop();
            Start();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Run()
        {
            Cpu.Reset();
            ExecuteUntilHalt();
        }

        private void SimpleExecuteUntilHalt()
        {
            while (!Cpu.IsHalted)
            {
                Cpu.Step();
            }
        }

        private void ExecuteUntilHalt()
        {
            // try to service 60ms worth of instructions per timer tick.
            var interval = 60;
            var cyclesPerMs = (int)(CpuFrequency / 1000);
            var cyclesPerInterval = interval * cyclesPerMs;
            var extra = 0;

            var timer = new Timer(
                (o) =>
                {
                    // if we can't process enough instructions fast enough, just block
                    lock (this)
                    {
                        if (Cpu.IsHalted)
                        {
                            attention.Set();
                        }
                        else
                        {
                            var cycles = extra;
                            while (cycles <= cyclesPerInterval)
                            {
                                cycles += Cpu.Step();
                            }

                            extra = cycles - cyclesPerInterval;
                        }
                    }
                },
                null,
                0,
                interval);

            attention.WaitOne();

            timer.Change(Timeout.Infinite, Timeout.Infinite);
            timer.Dispose();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (attention != null)
                {
                    Stop();
                    attention.Dispose();
                    attention = null;
                }
            }
        }
    }
}
