// <copyright file="Computer.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Altair8800
{
    using System;
    using System.IO;
    using System.Threading;
    using Crankery.Emulate.Common;
    using Crankery.Emulate.Core;
    using Crankery.Emulate.Core.Intel8080;
    using NLog;

    /// <summary>
    /// The Altair 8800 computer.
    /// </summary>
    public class Computer : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 2 MHz CPU 
        /// </summary>
        public const ulong CpuFrequency = 2000000;

        private ManualResetEvent attention;
        private Thread bgThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="Computer"/> class.
        /// </summary>
        public Computer()
        {
            Memory = new Memory();
            Devices = new Devices();
            Cpu = new Intel8080Cpu(Memory, Devices);
            SerialInputOutput = new SerialInputOutput(Devices);
            Disk = new Disk(Devices);
            FrontPanel = new FrontPanel(Devices);
        }

        /// <summary>
        /// Gets the serial input output.
        /// </summary>
        /// <value>
        /// The serial input output.
        /// </value>
        public SerialInputOutput SerialInputOutput { get; private set; }

        /// <summary>
        /// Gets the disk.
        /// </summary>
        /// <value>
        /// The disk.
        /// </value>
        public Disk Disk { get; private set; }

        /// <summary>
        /// Gets the front panel.
        /// </summary>
        /// <value>
        /// The front panel.
        /// </value>
        public FrontPanel FrontPanel { get; private set; }

        private Intel8080Cpu Cpu { get; set; }

        private Memory Memory { get; set; }

        private Devices Devices { get; set; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            attention = new ManualResetEvent(false);

            bgThread = new Thread(Run);
            bgThread.Start();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            attention.Set();
            bgThread.Join();

            Disk.Load(0, null);
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Stop();
            Start();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Run()
        {
            Disk.Load(0, File.ReadAllBytes(@"Resources\Disks\cpm.dsk"));
            Disk.Load(1, File.ReadAllBytes(@"Resources\Disks\altdos2.dsk"));
            Memory.Load(File.ReadAllBytes(@"Resources\Roms\dbl.bin"), 0xff00);

            Cpu.Reset();
            Cpu.ProgramCounter = 0xff00;

            ExecuteUntilHalt();
        }

        private void ResetAndContinue()
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
