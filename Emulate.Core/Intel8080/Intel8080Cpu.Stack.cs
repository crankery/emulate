// <copyright file="Intel8080Cpu.Stack.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Intel8080Cpu
    {
        [Opcode(Instruction = 0xc5, Mnemonic = "PUSH B", Length = 1, Duration = 11)]
        internal int PushBC(OpcodeAttribute opcode, byte[] instruction)
        {
            PushWord(registers.BC);

            return 0;
        }

        [Opcode(Instruction = 0xd5, Mnemonic = "PUSH D", Length = 1, Duration = 11)]
        internal int PushDE(OpcodeAttribute opcode, byte[] instruction)
        {
            PushWord(registers.DE);

            return 0;
        }

        [Opcode(Instruction = 0xe5, Mnemonic = "PUSH H", Length = 1, Duration = 11)]
        internal int PushHL(OpcodeAttribute opcode, byte[] instruction)
        {
            PushWord(registers.HL);
            
            return 0;
        }

        [Opcode(Instruction = 0xf5, Mnemonic = "PUSH PSW", Length = 1, Duration = 11)]
        internal int PushPsw(OpcodeAttribute opcode, byte[] instruction)
        {
            PushWord(Utility.MakeWord(registers.A, registers.Flags.Combined));

            return 0;
        }

        [Opcode(Instruction = 0xc1, Mnemonic = "POP  B", Length = 1, Duration = 10)]
        internal int PopBC(OpcodeAttribute opcode, byte[] instruction)
        {
            registers.BC = PopWord();

            return 0;
        }

        [Opcode(Instruction = 0xd1, Mnemonic = "POP  D", Length = 1, Duration = 10)]
        internal int PopDE(OpcodeAttribute opcode, byte[] instruction)
        {
            registers.DE = PopWord();

            return 0;
        }

        [Opcode(Instruction = 0xe1, Mnemonic = "POP  H", Length = 1, Duration = 10)]
        internal int PopHL(OpcodeAttribute opcode, byte[] instruction)
        {
            registers.HL = PopWord();

            return 0;
        }

        [Opcode(Instruction = 0xf1, Mnemonic = "POP  PSW", Length = 1, Duration = 10)]
        internal int PopPsw(OpcodeAttribute opcode, byte[] instruction)
        {
            var x = PopWord();
            registers.Flags.Combined = x.GetLow();
            registers.A = x.GetHigh();

            return 0;
        }

        private void PushWord(ushort w)
        {
            registers.StackPointer -= 2;
            Memory.Write(registers.StackPointer, w);
        }

        private ushort PopWord()
        {
            var result = Memory.ReadWord(registers.StackPointer);
            registers.StackPointer += 2;

            return result;
        }
    }
}
