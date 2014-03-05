namespace Crankery.Emulate.Core.Tests.Intel8080
{
    using System.IO;
    using System.Reflection;
    using Crankery.Emulate.Common;
    using Crankery.Emulate.Core.Intel8080;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HelloWorldFixture
    {
        [TestMethod]
        public void Intel8080_HelloWorldFixture_HelloWorldSaysHi()
        {
            var devices = new MockDevices();
            var machine = new Intel8080Cpu(new Memory(), devices);
            var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", string.Empty);
            var lines = File.ReadAllLines(Path.Combine(root, @"Intel8080\HelloWorld.hex"));
            machine.Memory.Load(lines);

            while (!machine.IsHalted)
            {
                machine.Step();
            }

            Assert.AreEqual("Hello World!", devices.Output);
        }
    }
}
