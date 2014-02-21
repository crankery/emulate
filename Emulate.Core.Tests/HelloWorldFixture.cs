using Crankery.Emulate.Core.Intel8080;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace Crankery.Emulate.Core.Tests
{
    [TestClass]
    public class HelloWorldFixture
    {
        [TestMethod]
        public void Run()
        {
            var devices = new MockDevices();
            var machine = new Intel8080Cpu(new Memory(), devices);
            var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", string.Empty);
            var lines = File.ReadAllLines(Path.Combine(root, "HelloWorld.hex"));
            machine.Memory.Load(lines);

            while (!machine.IsHalted)
            {
                machine.Step();
            }

            Assert.AreEqual("Hello World!", devices.Output);
        }
    }
}
