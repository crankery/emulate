// <copyright file="Intel8080Cpu.Immediate.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Intel8080Cpu
    {
        [Opcode(Instruction = 0x01, Mnemonic = "LXI  B,[a16]H", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0x11, Mnemonic = "LXI  D,[a16]H", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0x21, Mnemonic = "LXI  H,[a16]H", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0x31, Mnemonic = "LXI  SP,[a16]H", Length = 3, Duration = 10)]
        internal int LoadRegisterPair(OpcodeAttribute opcode, byte[] instruction)
        {
            var pair = (RegisterPair)((instruction[0] >> 4) & 3);

            registers[pair] = Utility.MakeWord(instruction[2], instruction[1]);

            return 0;
        }
        [Opcode(Instruction = 0x06, Mnemonic = "MVI  B,[d8]H", Length = 2, Duration = 4)]
        [Opcode(Instruction = 0x0e, Mnemonic = "MVI  C,[d8]H", Length = 2, Duration = 4)]
        [Opcode(Instruction = 0x16, Mnemonic = "MVI  D,[d8]H", Length = 2, Duration = 4)]
        [Opcode(Instruction = 0x1e, Mnemonic = "MVI  E,[d8]H", Length = 2, Duration = 4)]
        [Opcode(Instruction = 0x26, Mnemonic = "MVI  H,[d8]H", Length = 2, Duration = 4)]
        [Opcode(Instruction = 0x2e, Mnemonic = "MVI  L,[d8]H", Length = 2, Duration = 4)]
        [Opcode(Instruction = 0x36, Mnemonic = "MVI  M,[d8]H", Length = 2, Duration = 7)]
        [Opcode(Instruction = 0x3e, Mnemonic = "MVI  A,[d8]H", Length = 2, Duration = 4)]
        internal int MoveImmediate(OpcodeAttribute opcode, byte[] instruction)
        {
            var target = (Register)((instruction[0] >> 3) & 7);
            var value = instruction[1];

            registers[target] = value;

            return 0;
        }

        [Opcode(Instruction = 0xc6, Mnemonic = "ADI  [d8]H", Length = 2, Duration = 7)]
        internal int AddByteImmediate(OpcodeAttribute opcode, byte[] instruction)
        {
            registers.A = AddBytes(registers.A, instruction[1], false);

            return 0;
        }

        [Opcode(Instruction = 0xce, Mnemonic = "ACI  [d8]H", Length = 2, Duration = 7)]
        internal int AddByteImmediateWithCarry(OpcodeAttribute opcode, byte[] instruction)
        {
            registers.A = AddBytes(registers.A, instruction[1], true);

            return 0;
        }

        [Opcode(Instruction = 0xd6, Mnemonic = "SUI  [d8]H", Length = 2, Duration = 7)]
        internal int SubtractByteImmediate(OpcodeAttribute opcode, byte[] instruction)
        {
            registers.A = SubtractBytes(registers.A, instruction[1], false);

            return 0;
        }

        [Opcode(Instruction = 0xde, Mnemonic = "SBI [d8]H", Length = 2, Duration = 7)]
        internal int SubtractByteImmediateWithBorrow(OpcodeAttribute opcode, byte[] instruction)
        {
            registers.A = SubtractBytes(registers.A, instruction[1], true);

            return 0;
        }

        [Opcode(Instruction = 0xe6, Mnemonic = "ANI [d8]H", Length = 2, Duration = 7)]
        internal int AndByteImmediate(OpcodeAttribute opcode, byte[] instruction)
        {
            registers.A = AndBytes(registers.A, instruction[1]);

            return 0;
        }

        [Opcode(Instruction = 0xee, Mnemonic = "XRI [d8]H", Length = 2, Duration = 7)]
        internal int ExclusiveOrByteImmediate(OpcodeAttribute opcode, byte[] instruction)
        {
            registers.A = ExclusiveOrBytes(registers.A, instruction[1]);

            return 0;
        }

        [Opcode(Instruction = 0xf6, Mnemonic = "ORI [d8]H", Length = 2, Duration = 7)]
        internal int LogicalOrByteImmediate(OpcodeAttribute opcode, byte[] instruction)
        {
            registers.A = OrBytes(registers.A, instruction[1]);

            return 0;
        }

        [Opcode(Instruction = 0xfe, Mnemonic = "CPI [d8]H", Length = 2, Duration = 7)]
        internal int CompareByteImmediate(OpcodeAttribute opcode, byte[] instruction)
        {
            // ignore result for comparison.
            SubtractBytes(registers.A, instruction[1], false);

            return 0;
        }
    }
}
