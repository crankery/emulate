﻿// <copyright file="Mos6502Cpu.Accumulator.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    using System;

    public partial class Mos6502Cpu
    {
        [OpcodeEx(Instruction = 0xa9, Mnemonic = "LDA #[d8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Immediate)]
        [OpcodeEx(Instruction = 0xa5, Mnemonic = "LDA [d8]", Length = 2, Duration = 3, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0xb5, Mnemonic = "LDA [d8],X", Length = 2, Duration = 4, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0xad, Mnemonic = "LDA [a16]", Length = 3, Duration = 4, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0xbd, Mnemonic = "LDA [a16],X", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteX)]
        [OpcodeEx(Instruction = 0xb9, Mnemonic = "LDA [a16],Y", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteY)]
        [OpcodeEx(Instruction = 0xa1, Mnemonic = "LDA ([d8],X)", Length = 2, Duration = 6, AddressingMode = AddressingMode.IndexedIndirectX)]
        [OpcodeEx(Instruction = 0xb1, Mnemonic = "LDA ([d8]),Y", Length = 2, Duration = 5, AddressingMode = AddressingMode.IndexedIndirectY)]
        internal int LoadAccumulator(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.A = GetMemory(opcode.AddressingMode, instruction);
            registers.Flags.N = (registers.A & 0x80) != 0;
            registers.Flags.Z = registers.A == 0;

            return AddressCrossesPageBoundary(opcode.AddressingMode, instruction) ? 1 : 0;
        }

        [OpcodeEx(Instruction = 0x85, Mnemonic = "STA [d8]", Length = 2, Duration = 3, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0x95, Mnemonic = "STA [d8],X", Length = 2, Duration = 4, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0x8d, Mnemonic = "STA [a16]", Length = 3, Duration = 4, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0x9d, Mnemonic = "STA [a16],X", Length = 3, Duration = 5, AddressingMode = AddressingMode.AbsoluteX)]
        [OpcodeEx(Instruction = 0x99, Mnemonic = "STA [a16],Y", Length = 3, Duration = 5, AddressingMode = AddressingMode.AbsoluteY)]
        [OpcodeEx(Instruction = 0x81, Mnemonic = "STA ([d8],X)", Length = 2, Duration = 6, AddressingMode = AddressingMode.IndexedIndirectX)]
        [OpcodeEx(Instruction = 0x91, Mnemonic = "STA ([d8]),Y", Length = 2, Duration = 6, AddressingMode = AddressingMode.IndexedIndirectY)]
        internal int StoreAccumulator(OpcodeExAttribute opcode, byte[] instruction)
        {
            SetMemory(opcode.AddressingMode, instruction, registers.A);

            return AddressCrossesPageBoundary(opcode.AddressingMode, instruction) ? 1 : 0;
        }

        [OpcodeEx(Instruction = 0xc9, Mnemonic = "CMP #[d8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Immediate)]
        [OpcodeEx(Instruction = 0xc5, Mnemonic = "CMP [d8]", Length = 2, Duration = 3, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0xd5, Mnemonic = "CMP [d8],X", Length = 2, Duration = 4, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0xcd, Mnemonic = "CMP [a16]", Length = 3, Duration = 4, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0xdd, Mnemonic = "CMP [a16],X", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteX)]
        [OpcodeEx(Instruction = 0xd9, Mnemonic = "CMP [a16],Y", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteY)]
        [OpcodeEx(Instruction = 0xc1, Mnemonic = "CMP ([d8],X)", Length = 2, Duration = 6, AddressingMode = AddressingMode.IndexedIndirectX)]
        [OpcodeEx(Instruction = 0xd1, Mnemonic = "CMP ([d8]),Y", Length = 2, Duration = 5, AddressingMode = AddressingMode.IndexedIndirectY)]
        internal int CompareAccumulatorToMemory(OpcodeExAttribute opcode, byte[] instruction)
        {
            var m = GetMemory(opcode.AddressingMode, instruction);
            var t = (byte)(registers.A - m);
            registers.Flags.C = registers.A >= m;
            registers.Flags.N = (t & 0x80) != 0;
            registers.Flags.Z = t == 0;

            return AddressCrossesPageBoundary(opcode.AddressingMode, instruction) ? 1 : 0;
        }

        [OpcodeEx(Instruction = 0x8a, Mnemonic = "TXA", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int TransferXRegisterToAccumulator(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.A = registers.X;
            registers.Flags.N = (registers.A & 0x80) != 0;
            registers.Flags.Z = registers.A == 0;

            return 0;
        }

        [OpcodeEx(Instruction = 0xaa, Mnemonic = "TAX", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int TransferAccumulatorToXRegister(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.X = registers.A;
            registers.Flags.N = (registers.X & 0x80) != 0;
            registers.Flags.Z = registers.X == 0;

            return 0;
        }

        [OpcodeEx(Instruction = 0x98, Mnemonic = "TYA", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int TransferYRegisterToAccumulator(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.A = registers.Y;
            registers.Flags.N = (registers.A & 0x80) != 0;
            registers.Flags.Z = registers.A == 0;

            return 0;
        }

        [OpcodeEx(Instruction = 0xa8, Mnemonic = "TAY", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int TransferAccumulatorToYRegister(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.Y = registers.A;
            registers.Flags.N = (registers.Y & 0x80) != 0;
            registers.Flags.Z = registers.Y == 0;

            return 0;
        }
    }
}
