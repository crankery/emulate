namespace Crankery.Emulate.Core
{
    public class InputPort : Port
    {
        public InputPort(byte address)
            : base(address)
        {
        }

        public virtual byte Read()
        {
            return 0;
        }
    }
}
