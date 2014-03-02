// <copyright file="Cpu.BitwiseLogic.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    public partial class Cpu
    {
        [OpcodeEx(Instruction = 0x38, Mnemonic = "SEC", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int SetCarry(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.Flags.C = true;

            return 0;
        }

        [OpcodeEx(Instruction = 0x18, Mnemonic = "CLC", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int ClearCarry(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.Flags.C = false;

            return 0;
        }

        [OpcodeEx(Instruction = 0x78, Mnemonic = "SEI", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int SetInterrupt(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.Flags.I = true;

            return 0;
        }

        [OpcodeEx(Instruction = 0x58, Mnemonic = "CLI", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int ClearInterrupt(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.Flags.I = false;

            return 0;
        }

        [OpcodeEx(Instruction = 0xf8, Mnemonic = "SED", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int SetDecimal(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.Flags.D = true;

            return 0;
        }

        [OpcodeEx(Instruction = 0xd8, Mnemonic = "CLD", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int ClearDecimal(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.Flags.D = false;

            return 0;
        }

        [OpcodeEx(Instruction = 0xb8, Mnemonic = "CLV", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int ClearOverflow(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.Flags.I = false;

            return 0;
        }
    }
}
