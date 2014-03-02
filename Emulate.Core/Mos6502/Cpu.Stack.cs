// <copyright file="Cpu.Stack.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    public partial class Cpu
    {
        [OpcodeEx(Instruction = 0x48, Mnemonic = "PHA", Length = 1, Duration = 3, AddressingMode = AddressingMode.Implied)]
        internal int PushAccumulator(OpcodeExAttribute opcode, byte[] instruction)
        {
            Push(registers.A);

            return 0;
        }

        [OpcodeEx(Instruction = 0x68, Mnemonic = "PLA", Length = 1, Duration = 4, AddressingMode = AddressingMode.Implied)]
        internal int PullAccumulator(OpcodeExAttribute opcode, byte[] instruction)
        {
            registers.A = Pop();
            registers.Flags.N = (registers.A & 0x80) != 0;
            registers.Flags.Z = registers.A == 0;

            return 0;
        }

        [OpcodeEx(Instruction = 0x08, Mnemonic = "PHP", Length = 1, Duration = 3, AddressingMode = AddressingMode.Implied)]
        internal int PushProcessorStatusWord(OpcodeExAttribute opcode, byte[] instruction)
        {
            Push(registers.Flags.Combined);

            return 0;
        }

        [OpcodeEx(Instruction = 0x28, Mnemonic = "PLP", Length = 1, Duration = 4, AddressingMode = AddressingMode.Implied)]
        internal int PullProcessorStatusWord(OpcodeExAttribute opcode, byte[] instruction)
        {
            PullProcessorStatusWord();

            return 0;
        }


        private void Push(byte value)
        {
            Memory.Write(Utility.MakeWord(0x1, registers.StackPointer), value);
            registers.StackPointer--;
        }

        private byte Pop()
        {
            registers.StackPointer++;
            return Memory.Read(Utility.MakeWord(0x1, registers.StackPointer));
        }

        internal void PullProcessorStatusWord()
        {
            registers.Flags.Combined = Pop();
        }
    }
}
