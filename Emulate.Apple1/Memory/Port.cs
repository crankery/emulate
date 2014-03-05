namespace Crankery.Emulate.Apple1
{
    using System;

    public class Port : MemoryCell
    {
        private readonly Func<byte> read;
        private readonly Action<byte> write;

        public Port(Func<byte> read, Action<byte> write)
        {
            this.read = read;
            this.write = write;
        }

        public override byte Value
        {
            get
            {
                return read();
            }

            set
            {
                write(value);
            }
        }
    }
}
