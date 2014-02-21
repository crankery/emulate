namespace Crankery.Emulate.Core.Tests
{
    using System.IO;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [TestClass]
    public class CpuDiagFixture
    {
        [TestMethod]
        public void CpuDiagPasses()
        {
            var cpu = new Intel8080(new Memory(), new MockDevices());
            var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", string.Empty);
            var lines = File.ReadAllLines(Path.Combine(root, "cpudiag.bin"));
            cpu.Memory.Load(File.ReadAllBytes("cpudiag.bin"), 0x100);

            // this application's entry point is 0x100
            cpu.ProgramCounter = 0x100;
            cpu.Debug = true;

            // Halt (0x76) on the CPUER and CPUOK subroutines
            cpu.Memory.Write(0x0689, 0x76); // CPUER
            cpu.Memory.Write(0x069b, 0x76); // CPUOK

            while (!cpu.IsHalted)
            {
                cpu.Step();
            }

            // success if the program counter is one byte beyond CPUOK
            Assert.AreEqual(0x69c, cpu.ProgramCounter);
        }
    }
}
