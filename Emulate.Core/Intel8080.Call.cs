using System;
namespace Crankery.Emulate.Core
{
    public partial class Intel8080
    {
        [Opcode(Instruction = 0xcd, Mnemonic = "CALL [a16]", Length = 3, Duration = 17)]
        //[Opcode(Instruction = 0xdd, Mnemonic = "*CALL [a16]", Length = 3, Duration = 17)]
        //[Opcode(Instruction = 0xed, Mnemonic = "*CALL [a16]", Length = 3, Duration = 17)]
        //[Opcode(Instruction = 0xfd, Mnemonic = "*CALL [a16]", Length = 3, Duration = 17)]
        internal int CallUnconditional(byte[] instruction)
        {
            var address = (ushort)(instruction[2] << 8 | instruction[1]);

            Push(registers.ProgramCounter);
            registers.ProgramCounter = address;

            return 0;
        }

        [Opcode(Instruction = 0xc4, Mnemonic = "CNZ [a16]", Length = 3, Duration = 11)]
        [Opcode(Instruction = 0xcc, Mnemonic = "CZ [a16]", Length = 3, Duration = 11)]
        [Opcode(Instruction = 0xd4, Mnemonic = "CNC [a16]", Length = 3, Duration = 11)]
        [Opcode(Instruction = 0xdc, Mnemonic = "CC [a16]", Length = 3, Duration = 11)]
        [Opcode(Instruction = 0xe4, Mnemonic = "CPO [a16]", Length = 3, Duration = 11)]
        [Opcode(Instruction = 0xec, Mnemonic = "CPE [a16]", Length = 3, Duration = 11)]
        [Opcode(Instruction = 0xf4, Mnemonic = "CP [a16]", Length = 3, Duration = 11)]
        [Opcode(Instruction = 0xfc, Mnemonic = "CM [a16]", Length = 3, Duration = 11)]
        internal int CallConditional(byte[] instruction)
        {
            if (instruction[0] == 0xc4 && !flags.Z ||
                instruction[0] == 0xcc && flags.Z ||
                instruction[0] == 0xd4 && !flags.C ||
                instruction[0] == 0xdc && flags.C ||
                instruction[0] == 0xe4 && !flags.P ||
                instruction[0] == 0xec && flags.P ||
                instruction[0] == 0xf4 && !flags.S ||
                instruction[0] == 0xfc && flags.S)
            {
                Push(registers.ProgramCounter);
                
                var address = (ushort)(instruction[2] << 8 | instruction[1]);
                registers.ProgramCounter = address;

                // if we successfully evaluated a condition, it took 7 more cycles.
                return 7;
            }

            return 0;
        }

        [Opcode(Instruction = 0xc7, Mnemonic = "RST 0", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0xcf, Mnemonic = "RST 1", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0xd7, Mnemonic = "RST 2", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0xdf, Mnemonic = "RST 3", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0xe7, Mnemonic = "RST 4", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0xef, Mnemonic = "RST 5", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0xf7, Mnemonic = "RST 6", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0xff, Mnemonic = "RST 7", Length = 1, Duration = 11)]
        internal int CallSubroutine(byte[] instruction)
        {
            Push(registers.ProgramCounter);

            // the address of the sub is in bits 4-6. (0b00111000 mask)
            // 0=0x0000, 1=0x0008, ..., 7=0x0038
            registers.ProgramCounter = (ushort)(instruction[0] & 0x38);

            return 0;
        }
    }
}
