// <copyright file="Cpu.Arithmetic.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    using System;

    public partial class Cpu
    {
        [OpcodeEx(Instruction = 0x69, Mnemonic = "ADC #$[d8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Immediate)]
        [OpcodeEx(Instruction = 0x65, Mnemonic = "ADC $[d8]", Length = 2, Duration = 4, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0x75, Mnemonic = "ADC $[d8],X", Length = 2, Duration = 4, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0x6d, Mnemonic = "ADC $[d16]", Length = 3, Duration = 4, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0x7d, Mnemonic = "ADC $[d16],X", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteX)]
        [OpcodeEx(Instruction = 0x79, Mnemonic = "ADC $[d16],Y", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteY)]
        [OpcodeEx(Instruction = 0x61, Mnemonic = "ADC ($[d8],X)", Length = 2, Duration = 6, AddressingMode = AddressingMode.IndexedIndirectX)]
        [OpcodeEx(Instruction = 0x71, Mnemonic = "ADC ($[d8]),Y", Length = 2, Duration = 2, AddressingMode = AddressingMode.IndexedIndirectY)]
        internal int AddMemoryToAccumulatorWithCarry(OpcodeExAttribute opcode, byte[] instruction)
        {
            if (registers.Flags.D)
            {
                throw new NotImplementedException();
            }
            else
            {
                var t = registers.A + GetMemory(opcode.AddressingMode, instruction) + (registers.Flags.C ? 1 : 0);
                registers.Flags.V = (registers.A & 0x80) != (t & 0x80); // sign changed
                registers.Flags.C = t > 0xff;
                registers.A = (byte)t;
                registers.Flags.N = (registers.A & 0x80) != 0;
                registers.Flags.Z = registers.A == 0;
            }

            return AddressCrossesPageBoundary(opcode.AddressingMode, instruction) ? 1 : 0;
        }

        [OpcodeEx(Instruction = 0xe9, Mnemonic = "SBC #$[d8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Immediate)]
        [OpcodeEx(Instruction = 0xe5, Mnemonic = "SBC $[d8]", Length = 2, Duration = 3, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0xf5, Mnemonic = "SBC $[d8],X", Length = 2, Duration = 4, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0xed, Mnemonic = "SBC $[d16]", Length = 3, Duration = 4, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0xed, Mnemonic = "SBC $[d16],X", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteX)]
        [OpcodeEx(Instruction = 0xf9, Mnemonic = "SBC $[d16],Y", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteY)]
        [OpcodeEx(Instruction = 0xe1, Mnemonic = "SBC ($[d8],X)", Length = 2, Duration = 6, AddressingMode = AddressingMode.IndexedIndirectX)]
        [OpcodeEx(Instruction = 0xf1, Mnemonic = "SBC ($[d8]),Y", Length = 2, Duration = 5, AddressingMode = AddressingMode.IndexedIndirectY)]
        internal int SubtractMemoryFromAccumulatorWithBorrow(OpcodeExAttribute opcode, byte[] instruction)
        {
            if (registers.Flags.D)
            {
                throw new NotImplementedException();
            }
            else
            {
                var t = registers.A - GetMemory(opcode.AddressingMode, instruction) - (registers.Flags.C ? 0 : 1);
                registers.Flags.V = (registers.A & 0x80) != (t & 0x80); // sign changed
                registers.Flags.C = t > 0xff;
                registers.A = (byte)t;
                registers.Flags.N = (registers.A & 0x80) != 0;
                registers.Flags.Z = registers.A == 0;
            }

            return AddressCrossesPageBoundary(opcode.AddressingMode, instruction) ? 1 : 0;
        }

        [OpcodeEx(Instruction = 0xe6, Mnemonic = "INC [d8]", Length = 2, Duration = 5, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0xf6, Mnemonic = "INC [d8]", Length = 2, Duration = 6, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0xee, Mnemonic = "INC [d16]", Length = 3, Duration = 6, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0xfe, Mnemonic = "INC [d16],X", Length = 3, Duration = 7, AddressingMode = AddressingMode.AbsoluteX)]
        internal int IncrementMemory(OpcodeExAttribute opcode, byte[] instruction)
        {
            SetMemory(opcode.AddressingMode, instruction, (byte)(GetMemory(opcode.AddressingMode, instruction) + 1));

            return 0;
        }

        [OpcodeEx(Instruction = 0xc6, Mnemonic = "DEC [d8]", Length = 2, Duration = 5, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0xd6, Mnemonic = "DEC [d8]", Length = 2, Duration = 6, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0xce, Mnemonic = "DEC [d16]", Length = 3, Duration = 6, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0xde, Mnemonic = "DEC [d16],X", Length = 3, Duration = 7, AddressingMode = AddressingMode.AbsoluteX)]
        internal int DecrementMemory(OpcodeExAttribute opcode, byte[] instruction)
        {
            SetMemory(opcode.AddressingMode, instruction, (byte)(GetMemory(opcode.AddressingMode, instruction) - 1));

            return 0;
        }
    }
}
