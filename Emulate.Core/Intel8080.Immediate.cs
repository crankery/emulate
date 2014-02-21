// <copyright file="Intel8080.Immediate.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    public partial  class Intel8080
    {
        [Opcode(Instruction = 0x01, Mnemonic = "LD BC,[a16]", Length = 3, Duration = 10)]
        internal int LoadBC(byte[] instruction)
        {
            registers.BC = Utility.GetWord(instruction[2], instruction[1]);

            return 0;
        }

        [Opcode(Instruction = 0x11, Mnemonic = "LD DE,[a16]", Length = 3, Duration = 10)]
        internal int LoadDE(byte[] instruction)
        {
            registers.DE = Utility.GetWord(instruction[2], instruction[1]);

            return 0;
        }

        [Opcode(Instruction = 0x21, Mnemonic = "LD HL,[a16]", Length = 3, Duration = 10)]
        internal int LoadHL(byte[] instruction)
        {
            registers.HL = Utility.GetWord(instruction[2], instruction[1]);

            return 0;
        }

        [Opcode(Instruction = 0x31, Mnemonic = "LD SP,[a16]", Length = 3, Duration = 10)]
        internal int LoadSP(byte[] instruction)
        {
            registers.StackPointer = Utility.GetWord(instruction[2], instruction[1]);

            return 0;
        }

        [Opcode(Instruction = 0x06, Mnemonic = "MVI B,[d8]", Length = 2, Duration = 4)]
        [Opcode(Instruction = 0x0e, Mnemonic = "MVI C,[d8]", Length = 2, Duration = 4)]
        [Opcode(Instruction = 0x16, Mnemonic = "MVI D,[d8]", Length = 2, Duration = 4)]
        [Opcode(Instruction = 0x1e, Mnemonic = "MVI E,[d8]", Length = 2, Duration = 4)]
        [Opcode(Instruction = 0x26, Mnemonic = "MVI H,[d8]", Length = 2, Duration = 4)]
        [Opcode(Instruction = 0x2e, Mnemonic = "MVI L,[d8]", Length = 2, Duration = 4)]
        [Opcode(Instruction = 0x36, Mnemonic = "MVI M,[d8]", Length = 2, Duration = 7)]
        [Opcode(Instruction = 0x3e, Mnemonic = "MVI A,[d8]", Length = 2, Duration = 4)]
        internal int MoveImmediate(byte[] instruction)
        {
            var target = (Register)((instruction[0] >> 3) & 7);
            var value = instruction[1];

            registers[target] = value;

            return 0;
        }

        [Opcode(Instruction = 0xc6, Mnemonic = "ADI [d8]", Length = 2, Duration = 7)]
        internal int AddByteImmediate(byte[] instruction)
        {
            registers.A = AddBytes(registers.A, instruction[1], false);

            return 0;
        }

        [Opcode(Instruction = 0xce, Mnemonic = "ACI [d8]", Length = 2, Duration = 7)]
        internal int AddByteImmediateWithCarry(byte[] instruction)
        {
            registers.A = AddBytes(registers.A, instruction[1], true);

            return 0;
        }

        [Opcode(Instruction = 0xd6, Mnemonic = "SUI [d8]", Length = 2, Duration = 7)]
        internal int SubtractByteImmediate(byte[] instruction)
        {
            registers.A = SubtractBytes(registers.A, instruction[1], false);

            return 0;
        }

        [Opcode(Instruction = 0xde, Mnemonic = "SBI [d8]", Length = 2, Duration = 7)]
        internal int SubtractByteImmediateWithBorrow(byte[] instruction)
        {
            registers.A = SubtractBytes(registers.A, instruction[1], true);

            return 0;
        }

        [Opcode(Instruction = 0xe6, Mnemonic = "ANI [d8]", Length = 2, Duration = 7)]
        internal int AndByteImmediate(byte[] instruction)
        {
            registers.A = AndBytes(registers.A, instruction[1]);

            return 0;
        }

        [Opcode(Instruction = 0xee, Mnemonic = "XRI [d8]", Length = 2, Duration = 7)]
        internal int ExclusiveOrByteImmediate(byte[] instruction)
        {
            registers.A = ExclusiveOrBytes(registers.A, instruction[1]);

            return 0;
        }

        [Opcode(Instruction = 0xf6, Mnemonic = "ORI [d8]", Length = 2, Duration = 7)]
        internal int LogicalOrByteImmediate(byte[] instruction)
        {
            registers.A = OrBytes(registers.A, instruction[1]);

            return 0;
        }

        [Opcode(Instruction = 0xfe, Mnemonic = "CPI [d8]", Length = 2, Duration = 7)]
        internal int CompareByteImmediate(byte[] instruction)
        {
            // ignore result for comparison.
            SubtractBytes(registers.A, instruction[1], false);

            return 0;
        }
    }
}
