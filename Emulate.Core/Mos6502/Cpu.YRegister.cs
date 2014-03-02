// <copyright file="Cpu.YRegister.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    public partial class Cpu
    {
        [OpcodeEx(Instruction = 0xa0, Mnemonic = "LDY #$[d8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Immediate)]
        [OpcodeEx(Instruction = 0xa5, Mnemonic = "LDY $[d8]", Length = 2, Duration = 3, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0xb4, Mnemonic = "LDY $[d8],X", Length = 2, Duration = 4, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0xac, Mnemonic = "LDY $[d16]", Length = 3, Duration = 4, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0xbc, Mnemonic = "LDY $[d16],X", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteX)]
        internal int LoadYRegister(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.Y = GetMemory(opcode.AddressingMode, instruction);
            registers.Flags.N = (registers.Y & 0x80) != 0;
            registers.Flags.Z = registers.Y == 0;

            return AddressCrossesPageBoundary(opcode.AddressingMode, instruction) ? 1 : 0;
        }

        [OpcodeEx(Instruction = 0x84, Mnemonic = "STY $[d8]", Length = 2, Duration = 3, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0x94, Mnemonic = "STY $[d8],X", Length = 2, Duration = 4, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0x8c, Mnemonic = "STY $[d16]", Length = 3, Duration = 4, AddressingMode = AddressingMode.Absolute)]
        internal int StoreYRegister(OpcodeExAttribute opcode, byte[] instruction)
        {
            SetMemory(opcode.AddressingMode, instruction, registers.Y);

            return AddressCrossesPageBoundary(opcode.AddressingMode, instruction) ? 1 : 0;
        }

        [OpcodeEx(Instruction = 0xc8, Mnemonic = "INY", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int IncrementYRegister(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.Y++;
            registers.Flags.N = (registers.Y & 0x80) != 0;
            registers.Flags.Z = registers.Y == 0;

            return 0;
        }

        [OpcodeEx(Instruction = 0x88, Mnemonic = "DEY", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int DecrementYRegister(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.Y--;
            registers.Flags.N = (registers.Y & 0x80) != 0;
            registers.Flags.Z = registers.Y == 0;

            return 0;
        }
    }
}
