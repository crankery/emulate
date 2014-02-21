namespace Crankery.Emulate.Core
{
    public partial class Intel8080
    {
        [Opcode(Instruction = 0x00, Mnemonic = "NOP", Length = 1, Duration = 4)]
        ////[Opcode(Instruction = 0x10, Mnemonic = "NOP*", Length = 1, Duration = 4)]
        ////[Opcode(Instruction = 0x20, Mnemonic = "NOP*", Length = 1, Duration = 4)]
        ////[Opcode(Instruction = 0x30, Mnemonic = "NOP*", Length = 1, Duration = 4)]
        ////[Opcode(Instruction = 0x08, Mnemonic = "NOP*", Length = 1, Duration = 4)]
        ////[Opcode(Instruction = 0x18, Mnemonic = "NOP*", Length = 1, Duration = 4)]
        ////[Opcode(Instruction = 0x28, Mnemonic = "NOP*", Length = 1, Duration = 4)]
        ////[Opcode(Instruction = 0x38, Mnemonic = "NOP*", Length = 1, Duration = 4)]
        internal int NoOp(byte[] instruction)
        {
            return 0;
        }
    }
}
