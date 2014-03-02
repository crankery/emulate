// <copyright file="Cpu.XRegister.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    public partial class Cpu
    {
        [OpcodeEx(Instruction = 0xa2, Mnemonic = "LDX #$[d8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Immediate)]
        [OpcodeEx(Instruction = 0xa6, Mnemonic = "LDX $[d8]", Length = 2, Duration = 3, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0xb6, Mnemonic = "LDX $[d8],Y", Length = 2, Duration = 4, AddressingMode = AddressingMode.ZeroPageY)]
        [OpcodeEx(Instruction = 0xae, Mnemonic = "LDX $[d16]", Length = 3, Duration = 4, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0xbe, Mnemonic = "LDX $[d16],Y", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteY)]
        internal int LoadXRegister(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.X = GetMemory(opcode.AddressingMode, instruction);
            registers.Flags.N = (registers.X & 0x80) != 0;
            registers.Flags.Z = registers.X == 0;

            return AddressCrossesPageBoundary(opcode.AddressingMode, instruction) ? 1 : 0;
        }

        [OpcodeEx(Instruction = 0x86, Mnemonic = "STX $[d8]", Length = 2, Duration = 3, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0x96, Mnemonic = "STX $[d8],Y", Length = 2, Duration = 4, AddressingMode = AddressingMode.ZeroPageY)]
        [OpcodeEx(Instruction = 0x8e, Mnemonic = "STX $[d16]", Length = 3, Duration = 4, AddressingMode = AddressingMode.Absolute)]
        internal int StoreXRegister(OpcodeExAttribute opcode, byte[] instruction)
        {
            SetMemory(opcode.AddressingMode, instruction, registers.X);

            return AddressCrossesPageBoundary(opcode.AddressingMode, instruction) ? 1 : 0;
        }
        [OpcodeEx(Instruction = 0xe8, Mnemonic = "INX", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int IncrementXRegister(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.X++;
            registers.Flags.N = (registers.X & 0x80) != 0;
            registers.Flags.Z = registers.X == 0;

            return 0;
        }

        [OpcodeEx(Instruction = 0xca, Mnemonic = "DEX", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int DecrementXRegister(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.X--;
            registers.Flags.N = (registers.X & 0x80) != 0;
            registers.Flags.Z = registers.X == 0;

            return 0;
        }
    }
}
