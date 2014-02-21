namespace Crankery.Emulate.Core
{
    public abstract class Port
    {
        public Port(byte address)
        {
            Address = address;
        }

        public byte Address { get; private set; }
    }
}
