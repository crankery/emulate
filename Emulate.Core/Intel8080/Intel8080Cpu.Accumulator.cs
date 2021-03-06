﻿// <copyright file="Intel8080Cpu.Accumulator.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    using System;

    public partial class Intel8080Cpu
    {
        [Opcode(Instruction = 0x80, Mnemonic = "ADD  B", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x81, Mnemonic = "ADD  C", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x82, Mnemonic = "ADD  D", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x83, Mnemonic = "ADD  E", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x84, Mnemonic = "ADD  H", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x85, Mnemonic = "ADD  L", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x86, Mnemonic = "ADD  M", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x87, Mnemonic = "ADD  A", Length = 1, Duration = 4)]
        internal int AddToAccumulator(OpcodeAttribute opcode, byte[] instruction)
        {
            var source = (Register)(instruction[0] & 7);

            registers.A = AddByteToAccumulator(source, false);

            return 0;
        }

        [Opcode(Instruction = 0x88, Mnemonic = "ADC  B", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x89, Mnemonic = "ADC  C", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x8a, Mnemonic = "ADC  D", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x8b, Mnemonic = "ADC  E", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x8c, Mnemonic = "ADC  H", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x8d, Mnemonic = "ADC  L", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x8e, Mnemonic = "ADC  M", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x8f, Mnemonic = "ADC  A", Length = 1, Duration = 4)]
        internal int AddToAccumulatorWithCarry(OpcodeAttribute opcode, byte[] instruction)
        {
            var source = (Register)(instruction[0] & 7);

            registers.A = AddByteToAccumulator(source, true);
        
            return 0;
        }

        [Opcode(Instruction = 0x90, Mnemonic = "SUB  B", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x91, Mnemonic = "SUB  C", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x92, Mnemonic = "SUB  D", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x93, Mnemonic = "SUB  E", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x94, Mnemonic = "SUB  H", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x95, Mnemonic = "SUB  L", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x96, Mnemonic = "SUB  M", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x97, Mnemonic = "SUB  A", Length = 1, Duration = 4)]
        internal int SubtractFromAccumulator(OpcodeAttribute opcode, byte[] instruction)
        {
            var source = (Register)(instruction[0] & 7);

            registers.A = SubtractByteFromAccumulator(source, false);

            return 0;
        }

        [Opcode(Instruction = 0x98, Mnemonic = "SBB  B", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x99, Mnemonic = "SBB  C", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x9a, Mnemonic = "SBB  D", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x9b, Mnemonic = "SBB  E", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x9c, Mnemonic = "SBB  H", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x9d, Mnemonic = "SBB  L", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x9e, Mnemonic = "SBB  M", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x9f, Mnemonic = "SBB  A", Length = 1, Duration = 4)]
        internal int SubtractFromAccumulatorWithBorrow(OpcodeAttribute opcode, byte[] instruction)
        {
            var source = (Register)(instruction[0] & 7);

            registers.A = SubtractByteFromAccumulator(source, true);

            return 0;
        }

        [Opcode(Instruction = 0xb8, Mnemonic = "CMP  B", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xb9, Mnemonic = "CMP  C", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xba, Mnemonic = "CMP  D", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xbb, Mnemonic = "CMP  E", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xbc, Mnemonic = "CMP  H", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xbd, Mnemonic = "CMP  L", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xbe, Mnemonic = "CMP  M", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0xbf, Mnemonic = "CMP  A", Length = 1, Duration = 4)]
        internal int CompareAccumulator(OpcodeAttribute opcode, byte[] instruction)
        {
            var source = (Register)(instruction[0] & 7);

			// ignore the output of this (it goes nowhere for CMP)
            SubtractByteFromAccumulator(source, false);

            return 0;
        }

        [Opcode(Instruction = 0xa0, Mnemonic = "ANA  B", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xa1, Mnemonic = "ANA  C", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xa2, Mnemonic = "ANA  D", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xa3, Mnemonic = "ANA  E", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xa4, Mnemonic = "ANA  H", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xa5, Mnemonic = "ANA  L", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xa6, Mnemonic = "ANA  M", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0xa7, Mnemonic = "ANA  A", Length = 1, Duration = 4)]
        internal int AndAccumulator(OpcodeAttribute opcode, byte[] instruction)
        {
            var source = (Register)(instruction[0] & 7);

            registers.A = AndBytes(registers.A, registers[source]);

            return 0;
        }

        [Opcode(Instruction = 0xa8, Mnemonic = "XRA  B", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xa9, Mnemonic = "XRA  C", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xaa, Mnemonic = "XRA  D", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xab, Mnemonic = "XRA  E", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xac, Mnemonic = "XRA  H", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xad, Mnemonic = "XRA  L", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xae, Mnemonic = "XRA  M", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0xaf, Mnemonic = "XRA  A", Length = 1, Duration = 4)]
        internal int ExclusiveOrAccumulator(OpcodeAttribute opcode, byte[] instruction)
        {
            var source = (Register)(instruction[0] & 7);

            registers.A = ExclusiveOrBytes(registers.A, registers[source]);

            return 0;
        }

        [Opcode(Instruction = 0xb0, Mnemonic = "ORA  B", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xb1, Mnemonic = "ORA  C", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xb2, Mnemonic = "ORA  D", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xb3, Mnemonic = "ORA  E", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xb4, Mnemonic = "ORA  H", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xb5, Mnemonic = "ORA  L", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0xb6, Mnemonic = "ORA  M", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0xb7, Mnemonic = "ORA  A", Length = 1, Duration = 4)]
        internal int OrAccumulator(OpcodeAttribute opcode, byte[] instruction)
        {
            var source = (Register)(instruction[0] & 7);

            registers.A = OrBytes(registers.A, registers[source]);

            return 0;
        }

        private byte AddByteToAccumulator(Register source, bool withCarry)
        {
            return AddBytes(registers.A, registers[source], withCarry);
        }

        private byte SubtractByteFromAccumulator(Register source, bool withBorrow)
        {
            return SubtractBytes(registers.A, registers[source], withBorrow);
        }

        private byte OrBytes(byte a, byte b)
        {
            var result = (byte)(a | b);

            registers.Flags.Update(result);
            registers.Flags.C = false;
            registers.Flags.A = false;

            return result;
        }

        private byte ExclusiveOrBytes(byte a, byte b)
        {
            var result = (byte)(a ^ b);

            registers.Flags.Update(result);
            registers.Flags.C = false;
            registers.Flags.A = false;

            return result;
        }

        private byte AndBytes(byte a, byte b)
        {
            var result = (byte)(a & b);

            registers.Flags.Update(result);
            registers.Flags.C = false;
            registers.Flags.A = false;

            return result;
        }

        private byte AddBytes(byte a, byte b, bool withCarry)
        {
            var carry = (withCarry && registers.Flags.C ? 1 : 0);
            var result = a + b + carry;
            var final = (byte)result;

            registers.Flags.Update((byte)result);
            registers.Flags.C = result > 255;
            registers.Flags.A = (((b ^ final) ^ a) & 0x10) != 0;

            return final;
        }

        public byte SubtractBytes(byte a, byte b, bool withBorrow)
        {
            var borrow = (withBorrow && registers.Flags.C ? 1 : 0);
            var result = (a - b) - borrow;
            var final = (byte)result;

            registers.Flags.Update(final);
            registers.Flags.C = result < 0;
            registers.Flags.A = (((b ^ final) ^ a) & 0x10) != 0;

            return final;
        }
    }
}
