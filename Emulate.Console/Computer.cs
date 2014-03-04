
namespace Crankery.Emulate.Console
{
    using Crankery.Emulate.Core;
    using Crankery.Emulate.Core.Intel8080;
    using NLog;
    using System.IO;
    using System.Threading;

    public class Computer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // 2 MHz CPU
        public const ulong CpuFrequency = 2000000;

        private ManualResetEvent attention;
        private Thread bgThread;

        public Computer()
        {
            Memory = new Memory();
            Devices = new Devices();
            Cpu = new Intel8080Cpu(Memory, Devices);
            SerialInputOutput = new SerialInputOutput(Devices);
            Disk = new Disk(Devices);
            FrontPanel = new FrontPanel(Devices);
        }

        public SerialInputOutput SerialInputOutput { get; private set; }

        public Disk Disk { get; private set; }

        public FrontPanel FrontPanel { get; private set; }

        private Intel8080Cpu Cpu { get; set; }

        private Memory Memory { get; set; }

        private Devices Devices { get; set; }

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

            Disk.Load(0, null);
        }

        public void Reset()
        {
            Stop();
            Start();
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
    }
}
