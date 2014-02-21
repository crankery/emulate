namespace Crankery.Emulate.Core
{
    public class OutputPort : Port
    {
        public OutputPort(byte address)
            : base(address)
        {
        }

        public virtual void Write(byte value)
        { 
        }
    }
}
