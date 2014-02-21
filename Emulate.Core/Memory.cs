namespace Crankery.Emulate.Core
{
    /// <summary>
    /// Basic block of 64k of ram.
    /// </summary>
    public class Memory : IMemory
    {
        private readonly byte[] ram = new byte[0x10000];

        public Memory()
        {
        }

        public byte Read(ushort address)
        {
            return ram[address];
        }

        public ushort ReadWord(ushort address)
        {
            return (ushort)((int)ram[address] + ((int)ram[address + 1] << 8));
        }

        public void Write(ushort address, byte value)
        {
            ram[address] = value;
        }

        public void Write(ushort address, ushort value)
        {
            ram[address] = (byte)(value & 0xff);
            ram[address + 1] = (byte)(value >> 8);
        }
    }
}
