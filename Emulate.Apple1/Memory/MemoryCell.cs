namespace Crankery.Emulate.Apple1
{
    public class MemoryCell
    {
        private byte value;

        public MemoryCell()
        {
        }

        public virtual byte Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }
    }
}
