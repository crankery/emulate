// <copyright file="Mos6502Cpu.Arithmetic.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    using System;

    public partial class Mos6502Cpu
    {
        [OpcodeEx(Instruction = 0x69, Mnemonic = "ADC #[d8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Immediate)]
        [OpcodeEx(Instruction = 0x65, Mnemonic = "ADC [d8]", Length = 2, Duration = 4, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0x75, Mnemonic = "ADC [d8],X", Length = 2, Duration = 4, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0x6d, Mnemonic = "ADC [a16]", Length = 3, Duration = 4, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0x7d, Mnemonic = "ADC [a16],X", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteX)]
        [OpcodeEx(Instruction = 0x79, Mnemonic = "ADC [a16],Y", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteY)]
        [OpcodeEx(Instruction = 0x61, Mnemonic = "ADC ([d8],X)", Length = 2, Duration = 6, AddressingMode = AddressingMode.IndexedIndirectX)]
        [OpcodeEx(Instruction = 0x71, Mnemonic = "ADC ([d8]),Y", Length = 2, Duration = 2, AddressingMode = AddressingMode.IndexedIndirectY)]
        internal int AddMemoryToAccumulatorWithCarry(OpcodeExAttribute opcode, byte[] instruction)
        {
            var m = GetMemory(opcode.AddressingMode, instruction);
            var t = registers.A + m + (registers.Flags.C ? 1 : 0);

            if (registers.Flags.D)
            {
                if (((registers.A & 0xf) + (m & 0xf) + (registers.Flags.C ? 1 : 0)) > 9)
                {
                    // add 6 to temp to keep the low digit between 0 and 9.
                    t += 0x06;
                }

                if (t > 0x99)
                {
                    // increment the upper digit by 6 to keep he high digit between 0 and 9
                    t += 0x60;
                }

                // if the temporary value is bigger than "99", set the carry.
                registers.Flags.C = t > 0x99;
            }
            else
            {
                registers.Flags.C = t > 0xff;
            }

            registers.Flags.V = (((registers.A ^ t) & 0x80) != 0) && (((registers.A ^ m) & 0x80) == 0); // not valid in decimal
            registers.Flags.N = ((byte)t & 0x80) != 0; // not valid in decimal
            registers.Flags.Z = (byte)t == 0; // possibly not valid in decimal
            registers.A = (byte)t;

            return AddressCrossesPageBoundary(opcode.AddressingMode, instruction) ? 1 : 0;
        }

        [OpcodeEx(Instruction = 0xe9, Mnemonic = "SBC #[d8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Immediate)]
        [OpcodeEx(Instruction = 0xe5, Mnemonic = "SBC [d8]", Length = 2, Duration = 3, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0xf5, Mnemonic = "SBC [d8],X", Length = 2, Duration = 4, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0xed, Mnemonic = "SBC [a16]", Length = 3, Duration = 4, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0xfd, Mnemonic = "SBC [a16],X", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteX)]
        [OpcodeEx(Instruction = 0xf9, Mnemonic = "SBC [a16],Y", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteY)]
        [OpcodeEx(Instruction = 0xe1, Mnemonic = "SBC ([d8],X)", Length = 2, Duration = 6, AddressingMode = AddressingMode.IndexedIndirectX)]
        [OpcodeEx(Instruction = 0xf1, Mnemonic = "SBC ([d8]),Y", Length = 2, Duration = 5, AddressingMode = AddressingMode.IndexedIndirectY)]
        internal int SubtractMemoryFromAccumulatorWithBorrow(OpcodeExAttribute opcode, byte[] instruction)
        {
            var m = GetMemory(opcode.AddressingMode, instruction);
            var t = (uint)registers.A - (uint)m - (uint)(registers.Flags.C ? 0 : 1);

            if (registers.Flags.D)
            {
                if (((registers.A & 0xf) - (registers.Flags.C ? 0 : 1)) < (m & 0xf))
                {
                    t -= 0x06;
                }

                if (t > 0x99)
                {
                    t -= 0x60;
                }
            }

            registers.Flags.C = t <= 0xff; // works for decimal mode too
            registers.Flags.V = (((registers.A ^ t) & 0x80) != 0) && (((registers.A ^ m) & 0x80) != 0); // not valid in decimal
            registers.Flags.N = ((byte)t & 0x80) != 0; // not valid in decimal
            registers.Flags.Z = (byte)t == 0; // possibly not valid in decimal
            registers.A = (byte)t;

            return AddressCrossesPageBoundary(opcode.AddressingMode, instruction) ? 1 : 0;
        }

        [OpcodeEx(Instruction = 0xe6, Mnemonic = "INC [d8]", Length = 2, Duration = 5, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0xf6, Mnemonic = "INC [d8]", Length = 2, Duration = 6, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0xee, Mnemonic = "INC [a16]", Length = 3, Duration = 6, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0xfe, Mnemonic = "INC [a16],X", Length = 3, Duration = 7, AddressingMode = AddressingMode.AbsoluteX)]
        internal int IncrementMemory(OpcodeExAttribute opcode, byte[] instruction)
        {
            var m = (byte)(GetMemory(opcode.AddressingMode, instruction) + 1);
            registers.Flags.N = (m & 0x80) != 0;
            registers.Flags.Z = m == 0;

            SetMemory(opcode.AddressingMode, instruction, m);

            return 0;
        }

        [OpcodeEx(Instruction = 0xc6, Mnemonic = "DEC [d8]", Length = 2, Duration = 5, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0xd6, Mnemonic = "DEC [d8]", Length = 2, Duration = 6, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0xce, Mnemonic = "DEC [a16]", Length = 3, Duration = 6, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0xde, Mnemonic = "DEC [a16],X", Length = 3, Duration = 7, AddressingMode = AddressingMode.AbsoluteX)]
        internal int DecrementMemory(OpcodeExAttribute opcode, byte[] instruction)
        {
            var m = (byte)(GetMemory(opcode.AddressingMode, instruction) - 1);
            registers.Flags.N = (m & 0x80) != 0;
            registers.Flags.Z = m == 0;

            SetMemory(opcode.AddressingMode, instruction, m);

            return 0;
        }
    }
}
