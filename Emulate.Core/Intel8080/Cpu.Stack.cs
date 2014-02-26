// <copyright file="Cpu.Stack.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Cpu
    {
        [Opcode(Instruction = 0xc5, Mnemonic = "PUSH BC", Length = 1, Duration = 11)]
        internal int PushBC(byte[] instruction)
        {
            Push(registers.BC);

            return 0;
        }

        [Opcode(Instruction = 0xd5, Mnemonic = "PUSH DE", Length = 1, Duration = 11)]
        internal int PushDE(byte[] instruction)
        {
            Push(registers.DE);

            return 0;
        }

        [Opcode(Instruction = 0xe5, Mnemonic = "PUSH HL", Length = 1, Duration = 11)]
        internal int PushHL(byte[] instruction)
        {
            Push(registers.HL);
            
            return 0;
        }

        [Opcode(Instruction = 0xf5, Mnemonic = "PUSH AF", Length = 1, Duration = 11)]
        internal int PushAF(byte[] instruction)
        {
            Push(Utility.MakeWord(registers.A, registers.Flags.Combined));

            return 0;
        }

        [Opcode(Instruction = 0xc1, Mnemonic = "POP BC", Length = 1, Duration = 10)]
        internal int PopBC(byte[] instruction)
        {
            registers.BC = Pop();

            return 0;
        }

        [Opcode(Instruction = 0xd1, Mnemonic = "POP DE", Length = 1, Duration = 10)]
        internal int PopDE(byte[] instruction)
        {
            registers.DE = Pop();

            return 0;
        }

        [Opcode(Instruction = 0xe1, Mnemonic = "POP HL", Length = 1, Duration = 10)]
        internal int PopHL(byte[] instruction)
        {
            registers.HL = Pop();

            return 0;
        }

        [Opcode(Instruction = 0xf1, Mnemonic = "POP AF", Length = 1, Duration = 10)]
        internal int PopAF(byte[] instruction)
        {
            var x = Pop();
            registers.Flags.Combined = x.GetLow();
            registers.A = x.GetHigh();

            return 0;
        }

        private void Push(ushort w)
        {
            registers.StackPointer -= 2;
            Memory.Write(registers.StackPointer, w);
        }

        private ushort Pop()
        {
            var result = Memory.ReadWord(registers.StackPointer);
            registers.StackPointer += 2;

            return result;
        }
    }
}
