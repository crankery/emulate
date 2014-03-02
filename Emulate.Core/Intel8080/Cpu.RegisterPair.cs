// <copyright file="Cpu.RegisterPair.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Cpu
    {
        [Opcode(Instruction = 0x03, Mnemonic = "INC  B", Length = 1, Duration = 6)]
        [Opcode(Instruction = 0x13, Mnemonic = "INC  D", Length = 1, Duration = 6)]
        [Opcode(Instruction = 0x23, Mnemonic = "INC  H", Length = 1, Duration = 6)]
        [Opcode(Instruction = 0x33, Mnemonic = "INC  SP", Length = 1, Duration = 6)]
        internal int IncrementRegisterPair(OpcodeAttribute opcode, byte[] instruction)
        {
            var pair = (RegisterPair)((instruction[0] >> 4) & 3);

            registers[pair] = (ushort)(registers[pair] + 1);

            return 0;
        }

        [Opcode(Instruction = 0x0b, Mnemonic = "DEC  B", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x1b, Mnemonic = "DEC  D", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x2b, Mnemonic = "DEC  H", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x3b, Mnemonic = "DEC  SP", Length = 1, Duration = 5)]
        internal int DecrementRegisterPair(OpcodeAttribute opcode, byte[] instruction)
        {
            var pair = (RegisterPair)((instruction[0] >> 4) & 3);

            registers[pair] = (ushort)(registers[pair] - 1);

            return 0;
        }

        [Opcode(Instruction = 0x09, Mnemonic = "DAD  B", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0x19, Mnemonic = "DAD  D", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0x29, Mnemonic = "DAD  H", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0x39, Mnemonic = "DAD  SP", Length = 1, Duration = 11)]
        internal int DoubleAddRegisterPair(OpcodeAttribute opcode, byte[] instruction)
        {
            var pair = (RegisterPair)((instruction[0] >> 4) & 3);

            registers.HL = AddWord(registers.HL, registers[pair]);

            return 0;
        }

        [Opcode(Instruction = 0xeb, Mnemonic = "XCHG", Length = 1, Duration = 4)]
        internal int ExchangeDEAndHL(OpcodeAttribute opcode, byte[] instruction)
        {
            var x = registers.DE;
            registers.DE = registers.HL;
            registers.HL = x;

            return 0;
        }

        [Opcode(Instruction = 0xe3, Mnemonic = "XTHL", Length = 1, Duration = 18)]
        internal int ExhangeSPAndHL(OpcodeAttribute opcode, byte[] instruction)
        {
            var x = Memory.ReadWord(registers.StackPointer);
            Memory.Write(registers.StackPointer, registers.HL);
            registers.HL = x;

            return 0;
        }

        [Opcode(Instruction = 0xf9, Mnemonic = "SPHL", Length = 1, Duration = 5)]
        internal int LoadStackPointerHL(OpcodeAttribute opcode, byte[] instruction)
        {
            registers.StackPointer = registers.HL;

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
