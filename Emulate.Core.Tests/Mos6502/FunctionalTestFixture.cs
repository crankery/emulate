namespace Crankery.Emulate.Core.Tests.Mos6502
{
    using Crankery.Emulate.Core.Mos6502;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Reflection;

    [TestClass]
    public class FunctionalTestFixture
    {
        /// <summary>
        /// This really isn't a unit test but it'd be pretty difficult to split it up.
        /// If the halt is reached with the PC at the right spot things are good.
        /// http://2m5.de/6502_Emu/index.htm
        /// </summary>
        [TestMethod]
        public void Mos6502_FunctionalTestFixture_Passes()
        {
            var cpu = new Mos6502Cpu(new Memory());
            var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", string.Empty);
            cpu.Memory.Load(File.ReadAllBytes(Path.Combine(root, @"Mos6502\6502_functional_test.bin")), 0xa);
            cpu.ProgramCounter = 0x1000;

            var buffer = new CircularBuffer(1000);

            // enable this to figure out what went wrong. it slows things down quite a bit so it's not enabled by default.
            ////cpu.WriteLogMessage += (s, e) => buffer.Submit(e);

            while (!cpu.IsHalted)
            {
                cpu.Step();
            }

            // a successful test will halt at 0x3bb5 by looping on a single jump statement
            Assert.AreEqual(0x3bb5, cpu.ProgramCounter);
        }
    }
}