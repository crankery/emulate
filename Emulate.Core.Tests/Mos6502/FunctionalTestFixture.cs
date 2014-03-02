namespace Crankery.Emulate.Core.Tests.Mos6502
{
    using Crankery.Emulate.Core.Mos6502;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Reflection;

    [TestClass]
    public class FunctionalTestFixture
    {
        [TestMethod]
        [Ignore]
        public void FunctionalTestPasses()
        {
            // https://raw.github.com/redline6561/cl-6502/b0087903428ec2a3794ba4219494005174d1b09f/tests/6502_functional_test.a65
            var cpu = new Cpu(new Memory());
            var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", string.Empty);
            cpu.Memory.Load(File.ReadAllBytes(@"Mos6502\6502_functional_test.bin"), 0xa);
            cpu.ProgramCounter = 0x1000;

            while (!cpu.IsHalted)
            {
                cpu.Step();
            }
        }
    }
}
