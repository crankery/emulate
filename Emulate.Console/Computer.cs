
namespace Crankery.Emulate.Console
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using Crankery.Emulate.Core;

    public class Computer
    {
        // 2 MHz CPU
        public const ulong CpuFrequency = 2000000;

        private ManualResetEvent stop;
        private Thread bgThread;

        public Computer()
        {
            var devices = new Devices();
            Memory = new Memory();
            Devices = devices;
            Cpu = new Intel8080(Memory, Devices);

            SerialInputOutput = new SerialInputOutput(devices, 0x10, 0x11);
        }

        public Intel8080 Cpu { get; private set; }

        public IMemory Memory { get; private set; }

        public IDevices Devices { get; private set; }

        public SerialInputOutput SerialInputOutput { get; private set; }

        public void Start()
        {
            stop = new ManualResetEvent(false);

            bgThread = new Thread(Execute);
            bgThread.Start();
        }

        public void Stop()
        {
            stop.Set();
            bgThread.Join();
        }

        private void Diskboot()
        {
            var bootrom = new[]
            { 
                0041, 0000, 0114, 0021, 0030, 0377, 0016, 0346,
                0032, 0167, 0023, 0043, 0015, 0302, 0010, 0377,
                0303, 0000, 0114, 0000, 0000, 0000, 0000, 0000,
                0363, 0061, 0142, 0115, 0257, 0323, 0010, 0076,  
                0004, 0323, 0011, 0303, 0031, 0114, 0333, 0010,  
                0346, 0002, 0302, 0016, 0114, 0076, 0002, 0323,
                0011, 0333, 0010, 0346, 0100, 0302, 0016, 0114,
                0021, 0000, 0000, 0006, 0000, 0333, 0010, 0346,
                0004, 0302, 0045, 0114, 0076, 0020, 0365, 0325,
                0305, 0325, 0021, 0206, 0200, 0041, 0324, 0114,
                0333, 0011, 0037, 0332, 0070, 0114, 0346, 0037,
                0270, 0302, 0070, 0114, 0333, 0010, 0267, 0372,
                0104, 0114, 0333, 0012, 0167, 0043, 0035, 0312,
                0132, 0114, 0035, 0333, 0012, 0167, 0043, 0302,
                0104, 0114, 0341, 0021, 0327, 0114, 0001, 0200,
                0000, 0032, 0167, 0276, 0302, 0301, 0114, 0200,
                0107, 0023, 0043, 0015, 0302, 0141, 0114, 0032,
                0376, 0377, 0302, 0170, 0114, 0023, 0032, 0270,
                0301, 0353, 0302, 0265, 0114, 0361, 0361, 0052,
                0325, 0114, 0325, 0021, 0000, 0377, 0315, 0316,
                0114, 0321, 0332, 0276, 0114, 0315, 0316, 0114,
                0322, 0256, 0114, 0004, 0004, 0170, 0376, 0040,
                0332, 0054, 0114, 0006, 0001, 0312, 0054, 0114,
                0333, 0010, 0346, 0002, 0302, 0240, 0114, 0076,
                0001, 0323, 0011, 0303, 0043, 0114, 0076, 0200,
                0323, 0010, 0303, 0000, 0000, 0321, 0361, 0075,
                0302, 0056, 0114, 0076, 0103, 0001, 0076, 0117,
                0001, 0076, 0115, 0107, 0076, 0200, 0323, 0010,
                0170, 0323, 0001, 0303, 0311, 0114, 0172, 0274,
                0300, 0173, 0275, 0311, 0204, 0000, 0114, 0044,
                0026, 0126, 0026, 0000, 0000, 0000, 0000, 0000
            };

            for (int i = 0; i < bootrom.Length; i++)
            {
                // these are octal values stored in integers.
                // octal? why?
                Memory.Write((ushort)(0xff00 + i), Convert.ToByte(bootrom[i].ToString(), 8));
            }

            Cpu.ProgramCounter = 0xff00;
        }

        private void AltairBasic(string hexfile)
        {
            Memory.Load(File.ReadLines(hexfile));
        }

        private void TurnkeyMon()
        {
            Memory.Load(File.ReadLines("turnmon.hex"));
            Cpu.ProgramCounter = 0xfc00;
        }

        private void Execute()
        {
            Cpu.Reset();
            //Cpu.Debug = true;
            var throttle = !Cpu.Debug;

            AltairBasic("basicex.hex");

            SerialInputOutput.Startup();

            ulong pauseAfter = CpuFrequency / 1000; // we are pausing 1 ms every 'pauseAfter' cpu cycles
            var sw = new Stopwatch();
            var startCycle = Cpu.Cycle;
            var pauseCycle = startCycle;
            sw.Start();

            int i = 0;
            while (!Cpu.IsHalted)
            {
                Cpu.Step();

                // check the stop flag every 1000 iterations regardless of throttling
                // don't wait for this check.
                if ((i++ % 1000) == 0 && stop.WaitOne(0))
                {
                    break;
                }

                // try to throttle to the desired frequency
                if (throttle)
                {
                    var cycles = Cpu.Cycle - startCycle;
                    if (cycles >= (CpuFrequency / 10))
                    {
                        sw.Stop();

                        var s = (sw.ElapsedTicks * 1.0) / Stopwatch.Frequency;
                        var cps = cycles / s;

                        if (cps > CpuFrequency)
                        {
                            // too fast, pause more
                            pauseAfter = Math.Max(pauseAfter - 50, 0);
                        }
                        else
                        {
                            // too slow, pause less
                            // TODO: we need a sensible minimum poll internal
                            pauseAfter = Math.Min(pauseAfter + 50, ulong.MaxValue);
                        }

                        startCycle = Cpu.Cycle;
                        sw.Restart();
                    }

                    if (Cpu.Cycle - pauseCycle > pauseAfter)
                    {
                        pauseCycle = Cpu.Cycle;

                        if (stop.WaitOne(1))
                        {
                            break;
                        }
                    }
                }
            }

            SerialInputOutput.Shutdown();
        }
    }
}
