// <copyright file="Mos6502Cpu.BitwiseLogic.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    public partial class Mos6502Cpu
    {
        [OpcodeEx(Instruction = 0x29, Mnemonic = "AND #[d8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Immediate)]
        [OpcodeEx(Instruction = 0x25, Mnemonic = "AND [d8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0x35, Mnemonic = "AND [d8],X", Length = 2, Duration = 3, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0x2d, Mnemonic = "AND [a16]", Length = 3, Duration = 4, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0x3d, Mnemonic = "AND [a16],X", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteX)]
        [OpcodeEx(Instruction = 0x39, Mnemonic = "AND [a16],Y", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteY)]
        [OpcodeEx(Instruction = 0x21, Mnemonic = "AND ([d8],X)", Length = 2, Duration = 6, AddressingMode = AddressingMode.IndexedIndirectX)]
        [OpcodeEx(Instruction = 0x31, Mnemonic = "AND ([d8]),Y", Length = 2, Duration = 5, AddressingMode = AddressingMode.IndexedIndirectY)]
        internal int BitwiseAndAccumulatorWithMemory(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.A = (byte)(registers.A & GetMemory(opcode.AddressingMode, instruction));
            registers.Flags.N = (registers.A & 0x80) != 0;
            registers.Flags.Z = registers.A == 0;

            return AddressCrossesPageBoundary(opcode.AddressingMode, instruction) ? 1 : 0;
        }

        [OpcodeEx(Instruction = 0x49, Mnemonic = "EOR #[d8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Immediate)]
        [OpcodeEx(Instruction = 0x45, Mnemonic = "EOR [d8]", Length = 2, Duration = 3, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0x55, Mnemonic = "EOR [d8],X", Length = 2, Duration = 4, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0x4d, Mnemonic = "EOR [a16]", Length = 3, Duration = 4, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0x5d, Mnemonic = "EOR [a16],X", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteX)]
        [OpcodeEx(Instruction = 0x59, Mnemonic = "EOR [a16],Y", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteY)]
        [OpcodeEx(Instruction = 0x41, Mnemonic = "EOR ([d8],X)", Length = 2, Duration = 6, AddressingMode = AddressingMode.IndexedIndirectX)]
        [OpcodeEx(Instruction = 0x51, Mnemonic = "EOR ([d8]),Y", Length = 2, Duration = 5, AddressingMode = AddressingMode.IndexedIndirectY)]
        internal int BitwiseExclusiveOrAccumulatorWithMemory(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.A = (byte)(registers.A ^ GetMemory(opcode.AddressingMode, instruction));
            registers.Flags.N = (registers.A & 0x80) != 0;
            registers.Flags.Z = registers.A == 0;

            return AddressCrossesPageBoundary(opcode.AddressingMode, instruction) ? 1 : 0;
        }

        [OpcodeEx(Instruction = 0x09, Mnemonic = "ORA #[d8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Immediate)]
        [OpcodeEx(Instruction = 0x05, Mnemonic = "ORA [d8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0x15, Mnemonic = "ORA [d8],X", Length = 2, Duration = 3, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0x0d, Mnemonic = "ORA [a16]", Length = 3, Duration = 4, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0x1d, Mnemonic = "ORA [a16],X", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteX)]
        [OpcodeEx(Instruction = 0x19, Mnemonic = "ORA [a16],Y", Length = 3, Duration = 4, AddressingMode = AddressingMode.AbsoluteY)]
        [OpcodeEx(Instruction = 0x01, Mnemonic = "ORA ([d8],X)", Length = 2, Duration = 6, AddressingMode = AddressingMode.IndexedIndirectX)]
        [OpcodeEx(Instruction = 0x11, Mnemonic = "ORA ([d8]),Y", Length = 2, Duration = 5, AddressingMode = AddressingMode.IndexedIndirectY)]
        internal int BitwiseOrAccumulatorWithMemory(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.A = (byte)(registers.A | GetMemory(opcode.AddressingMode, instruction));
            registers.Flags.N = (registers.A & 0x80) != 0;
            registers.Flags.Z = registers.A == 0;

            return AddressCrossesPageBoundary(opcode.AddressingMode, instruction) ? 1 : 0;
        }

        [OpcodeEx(Instruction = 0x0a, Mnemonic = "ASL A", Length = 1, Duration = 2, AddressingMode = AddressingMode.Accumulator)]
        [OpcodeEx(Instruction = 0x06, Mnemonic = "ASL [d8]", Length = 2, Duration = 5, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0x16, Mnemonic = "ASL [d8],X", Length = 2, Duration = 6, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0x0e, Mnemonic = "ASL [a16]", Length = 3, Duration = 6, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0x1e, Mnemonic = "ASL [a16],X", Length = 3, Duration = 7, AddressingMode = AddressingMode.AbsoluteX)]
        internal int ArithmeticShiftLeft(OpcodeExAttribute opcode, byte[] instruction)
        {
            var value = GetMemory(opcode.AddressingMode, Fetch);
            registers.Flags.C = (value & 0x80) != 0;
            value <<= 1;

            SetMemory(opcode.AddressingMode, Fetch, value);
            registers.Flags.N = (value & 0x80) != 0;
            registers.Flags.Z = value == 0;

            return 0;
        }

        [OpcodeEx(Instruction = 0x4a, Mnemonic = "LSR A", Length = 1, Duration = 2, AddressingMode = AddressingMode.Accumulator)]
        [OpcodeEx(Instruction = 0x46, Mnemonic = "LSR [d8]", Length = 2, Duration = 5, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0x56, Mnemonic = "LSR [d8],X", Length = 2, Duration = 6, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0x4e, Mnemonic = "LSR [a16]", Length = 3, Duration = 6, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0x5e, Mnemonic = "LSR [a16],X", Length = 3, Duration = 7, AddressingMode = AddressingMode.AbsoluteX)]
        internal int LogicalShiftRight(OpcodeExAttribute opcode, byte[] instruction)
        {
            var value = GetMemory(opcode.AddressingMode, Fetch);
            registers.Flags.C = (value & 0x1) != 0;
            value >>= 1;

            SetMemory(opcode.AddressingMode, Fetch, value);
            registers.Flags.N = false;
            registers.Flags.Z = value == 0;

            return 0;
        }

        [OpcodeEx(Instruction = 0x2a, Mnemonic = "ROL A", Length = 1, Duration = 2, AddressingMode = AddressingMode.Accumulator)]
        [OpcodeEx(Instruction = 0x26, Mnemonic = "ROL [d8]", Length = 2, Duration = 5, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0x36, Mnemonic = "ROL [d8],X", Length = 2, Duration = 6, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0x2e, Mnemonic = "ROL [a16]", Length = 3, Duration = 6, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0x3e, Mnemonic = "ROL [a16],X", Length = 3, Duration = 7, AddressingMode = AddressingMode.AbsoluteX)]
        internal int RotateLeft(OpcodeExAttribute opcode, byte[] instruction)
        {
            var value = GetMemory(opcode.AddressingMode, Fetch);
            var c = (byte)(registers.Flags.C ? 0x1 : 0x0);
            registers.Flags.C = (value & 0x80) != 0;

            value <<= 1;
            value |= c;

            SetMemory(opcode.AddressingMode, Fetch, value);
            registers.Flags.N = (value & 0x80) != 0;
            registers.Flags.Z = value == 0;

            return 0;
        }

        [OpcodeEx(Instruction = 0x6a, Mnemonic = "ROR A", Length = 1, Duration = 2, AddressingMode = AddressingMode.Accumulator)]
        [OpcodeEx(Instruction = 0x66, Mnemonic = "ROR [d8]", Length = 2, Duration = 5, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0x76, Mnemonic = "ROR [d8],X", Length = 2, Duration = 6, AddressingMode = AddressingMode.ZeroPageX)]
        [OpcodeEx(Instruction = 0x6e, Mnemonic = "ROR [a16]", Length = 3, Duration = 6, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0x7e, Mnemonic = "ROR [a16],X", Length = 3, Duration = 7, AddressingMode = AddressingMode.AbsoluteX)]
        internal int RotateRight(OpcodeExAttribute opcode, byte[] instruction)
        {
            var value = GetMemory(opcode.AddressingMode, Fetch);
            var c = (byte)(registers.Flags.C ? 0x80 : 0x00);
            registers.Flags.C = (value & 0x1) != 0;

            value >>= 1;
            value |= c;

            SetMemory(opcode.AddressingMode, Fetch, value);
            registers.Flags.N = (value & 0x80) != 0;
            registers.Flags.Z = value == 0;

            return 0;
        }

        [OpcodeEx(Instruction = 0x24, Mnemonic = "BIT [d8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.ZeroPage)]
        [OpcodeEx(Instruction = 0x2c, Mnemonic = "BIT [a16]", Length = 3, Duration = 3, AddressingMode = AddressingMode.Absolute)]
        internal int TestBits(OpcodeExAttribute opcode, byte[] instruction)
        {
            var value = GetMemory(opcode.AddressingMode, Fetch);

            registers.Flags.N = (value & 0x80) != 0; // copy bit 7 from memory to N
            registers.Flags.V = (value & 0x40) != 0; // copy bit 6 from memory to V
            registers.Flags.Z = (value & registers.A) == 0; // the result of the AND is zero?

            return 0;
        }

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
            registers.Flags.V = false;

            return 0;
        }
    }
}
