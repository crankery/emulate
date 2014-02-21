namespace Crankery.Emulate.Core
{
    public interface IDevices
    {
        byte Read(byte port);
        void Write(byte port, byte value);
    }
}
