using Crankery.Emulate.Core.Intel8080;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace Crankery.Emulate.Core.Tests.Intel8080
{
    [TestClass]
    public class HelloWorldFixture
    {
        [TestMethod]
        public void HelloWorldSaysHi()
        {
            var devices = new MockDevices();
            var machine = new Cpu(new Memory(), devices);
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
