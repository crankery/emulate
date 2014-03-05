namespace Crankery.Emulate.Apple1
{
    public class EmptyMemoryCell : MemoryCell
    {
        public override byte Value
        {
            get
            {
                return 0xfe;
            }
            set
            {
            }
        }
    }
}
