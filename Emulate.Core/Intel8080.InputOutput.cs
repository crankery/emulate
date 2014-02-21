namespace Crankery.Emulate.Core
{
    public partial class Intel8080
    {
        [Opcode(Instruction = 0xdb, Mnemonic = "IN [d8]", Length = 2, Duration = 10)]
        internal int In(byte[] instruction)
        {
            registers.A = Devices.Read(instruction[1]);

            return 0;
        }

        [Opcode(Instruction = 0xd3, Mnemonic = "OUT [d8]", Length = 2, Duration = 10)]
        internal int Out(byte[] instruction)
        {
            Devices.Write(instruction[1], registers.A);

            return 0;
        }
    }
}
