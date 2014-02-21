namespace Crankery.Emulate.Core
{
    public interface IMemory
    {
        byte Read(ushort address);
        ushort ReadWord(ushort address);
        void Write(ushort address, byte value);
        void Write(ushort address, ushort value);
    }
}
