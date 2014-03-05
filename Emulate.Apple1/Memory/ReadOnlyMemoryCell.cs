namespace Crankery.Emulate.Apple1
{
    public class ReadOnlyMemoryCell : MemoryCell
    {
        public ReadOnlyMemoryCell(byte initialValue)
        {
            base.Value = initialValue;
        }

        public override byte Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                // ignore the write operation.
            }
        }
    }
}
