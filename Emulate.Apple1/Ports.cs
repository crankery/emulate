namespace Crankery.Emulate.Apple1
{
    using System.Collections.Generic;
    using Crankery.Emulate.Common;

    public class Ports
    {
        private Queue<byte> keys = new Queue<byte>();

        public event SendByteEventArgs.SendByteEvent DisplayCharacter = delegate { };

        public Ports()
        {
            KeyboardData = new Port(
                () =>
                {
                    return keys.Count == 0 ? (byte)0x00 : (byte)(keys.Dequeue() | 0x80);
                },
                (x) =>
                {
                });

            KeyboardControl = new Port(
                () =>
                {
                    return keys.Count == 0 ? (byte)0x00 : (byte)0x80;
                },
                (x) =>
                {
                });

            DisplayData = new Port(
                () =>
                {
                    return (byte)0x00;
                },
                (x) =>
                {
                    DisplayCharacter(this, new SendByteEventArgs { Value = (byte)(x & 0x7f) });
                });

            DisplayControl = new Port(
                () =>
                {
                    return (byte)0x00;
                },
                (x) =>
                {
                });
        }

        public Port KeyboardData { get; private set; }

        public Port KeyboardControl { get; private set; }

        public Port DisplayData { get; private set; }

        public Port DisplayControl { get; private set; }

        public void KeyPressed(byte keyCode)
        {
            keys.Enqueue(keyCode);
        }
    }
}
