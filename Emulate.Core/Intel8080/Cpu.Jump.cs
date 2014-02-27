// <copyright file="Cpu.Jump.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Cpu
    {
        /// <summary>
        /// PCHL or Jump to HL
        /// </summary>
        [Opcode(Instruction = 0xe9, Mnemonic = "PCHL", Length = 1, Duration = 5)]
        internal int JumpToHL(byte[] instruction)
        {
            registers.ProgramCounter = registers.HL;

            return 0;
        }
        
        [Opcode(Instruction = 0xc3, Mnemonic = "JMP  [a16]", Length = 3, Duration = 10)]
        internal int JumpDirectUnconditional(byte[] instruction)
        {
            var address = Utility.MakeWord(instruction[2], instruction[1]);
            registers.ProgramCounter = address;

            return 0;
        }

        /// <summary>
        /// JMP to a location in memory.
        /// </summary>
        [Opcode(Instruction = 0xc2, Mnemonic = "JNZ  [a16]", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0xca, Mnemonic = "JZ   [a16]", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0xd2, Mnemonic = "JNC  [a16]", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0xda, Mnemonic = "JC   [a16]", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0xe2, Mnemonic = "JPO  [a16]", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0xea, Mnemonic = "JPE  [a16]", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0xf2, Mnemonic = "JP   [a16]", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0xfa, Mnemonic = "JM   [a16]", Length = 3, Duration = 10)]
        internal int JumpDirectConditional(byte[] instruction)
        {
            // instruction encodes the flag from 0..3 in bits 4 & 5
            // instruction encodes the test (true or false) in bit 3.
            var flag = (Flag)((instruction[0] >> 4) & 3);
            var test = (instruction[0] & 8) == 0 ? false : true;

            if (registers.Flags[flag] == test)
            {
                var address = Utility.MakeWord(instruction[2], instruction[1]);
                registers.ProgramCounter = address;

                // if we successfully evaluated a condition, it took 5 more cycles.
                return 5;
            }

            return 0;
        }
    }
}
