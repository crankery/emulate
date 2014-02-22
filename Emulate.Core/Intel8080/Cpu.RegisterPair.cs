// <copyright file="Cpu.RegisterPair.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Cpu
    {
        [Opcode(Instruction = 0x03, Mnemonic = "INC BC", Length = 1, Duration = 6)]
        [Opcode(Instruction = 0x13, Mnemonic = "INC DE", Length = 1, Duration = 6)]
        [Opcode(Instruction = 0x23, Mnemonic = "INC HL", Length = 1, Duration = 6)]
        [Opcode(Instruction = 0x33, Mnemonic = "INC SP", Length = 1, Duration = 6)]
        internal int IncrementRegisterPair(byte[] instruction)
        {
            var pair = (RegisterPair)((instruction[0] >> 4) & 3);

            registers[pair] = (ushort)(registers[pair] + 1);

            return 0;
        }

        [Opcode(Instruction = 0x0b, Mnemonic = "DEC BC", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x1b, Mnemonic = "DEC DE", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x2b, Mnemonic = "DEC HL", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x3b, Mnemonic = "DEC SP", Length = 1, Duration = 5)]
        internal int DecrementRegisterPair(byte[] instruction)
        {
            var pair = (RegisterPair)((instruction[0] >> 4) & 3);

            registers[pair] = (ushort)(registers[pair] - 1);

            return 0;
        }
        [Opcode(Instruction = 0x09, Mnemonic = "DAD BC", Length = 1, Duration = 11)]
        internal int DoubleAddBC(byte[] instruction)
        {
            registers.HL = AddWord(registers.HL, registers.BC);

            return 0;
        }

        [Opcode(Instruction = 0x19, Mnemonic = "DAD DE", Length = 1, Duration = 11)]
        internal int DoubleAddDE(byte[] instruction)
        {
            registers.HL = AddWord(registers.HL, registers.DE);

            return 0;
        }

        [Opcode(Instruction = 0x29, Mnemonic = "DAD HL", Length = 1, Duration = 11)]
        internal int DoubleAddHL(byte[] instruction)
        {
            registers.HL = AddWord(registers.HL, registers.HL);

            return 0;
        }

        [Opcode(Instruction = 0x39, Mnemonic = "DAD SP", Length = 1, Duration = 11)]
        internal int DoubleAddSP(byte[] instruction)
        {
            registers.HL = AddWord(registers.HL, registers.StackPointer);

            return 0;
        }

        [Opcode(Instruction = 0xeb, Mnemonic = "EX DE,HL", Length = 1, Duration = 4)]
        internal int ExchangeDEAndHL(byte[] instruction)
        {
            var x = registers.DE;
            registers.DE = registers.HL;
            registers.HL = x;

            return 0;
        }

        [Opcode(Instruction = 0xe3, Mnemonic = "EX (SP),HL", Length = 1, Duration = 18)]
        internal int ExhangeSPAndHL(byte[] instruction)
        {
            var x = Memory.ReadWord(registers.StackPointer);
            Memory.Write(registers.StackPointer, registers.HL);
            registers.HL = x;

            return 0;
        }

        [Opcode(Instruction = 0xf9, Mnemonic = "LD SP,HL", Length = 1, Duration = 5)]
        internal int LoadStackPointerHL(byte[] instruction)
        {
            registers.HL = registers.StackPointer;

            return 0;
        }

        private ushort AddWord(ushort a, ushort b)
        {
            var result = a + b;
            registers.Flags.C = result > 0xffff;

            return (ushort)result;
        }
    }
}
