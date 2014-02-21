namespace Crankery.Emulate.Core
{
    public partial class Intel8080
    {
        [Opcode(Instruction = 0xf3, Mnemonic = "DI", Length = 1, Duration = 4)]
        internal int DisableInterurrupts(byte[] instruction)
        {
            interruptEnable = false;

            return 0;
        }

        [Opcode(Instruction = 0xfb, Mnemonic = "EI", Length = 1, Duration = 4)]
        internal int EnableInterrupts(byte[] instruction)
        {
            interruptEnable = true;

            return 0;
        }
    }
}
