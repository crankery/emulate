using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Crankery.Emulate.Core.Tests
{
    [TestClass]
    public class CircularBufferFixture
    {
        [TestMethod]
        public void CircularBufferFixture_CanStoreLessThanCapacity()
        {
            var sut = new CircularBuffer<string>(10);

            sut.Submit("one");
            sut.Submit("two");

            Assert.IsTrue(Enumerable.SequenceEqual(new[] { "one", "two" }, sut.Values));
        }

        [TestMethod]
        public void CircularBufferFixture_CanStoreCapacityValues()
        {
            var count = 10;
            var items = 10;
            var sut = new CircularBuffer<int>(count);

            foreach (var v in Enumerable.Range(0, items))
            {
                sut.Submit(v);
            }

            var values = sut.Values.ToArray();
            var expected = Enumerable.Range(items - count, count).ToArray();

            Assert.IsTrue(Enumerable.SequenceEqual(expected, values));
        }

        [TestMethod]
        public void CircularBufferFixture_StoresLastNValues()
        {
            var count = 10;
            var items = 95;
            var sut = new CircularBuffer<int>(count);

            foreach (var v in Enumerable.Range(0, items))
            {
                sut.Submit(v);
            }

            var values = sut.Values.ToArray();
            var expected = Enumerable.Range(items - count, count).ToArray();

            Assert.IsTrue(Enumerable.SequenceEqual(expected, values));
        }
    }
}
