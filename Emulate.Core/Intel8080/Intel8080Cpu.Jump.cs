// <copyright file="Intel8080.Jump.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Intel8080Cpu
    {
        /// <summary>
        /// PCHL or Jump to HL
        /// </summary>
        [Opcode(Instruction = 0xe9, Mnemonic = "JP (HL)", Length = 1, Duration = 5)]
        internal int JumpToHL(byte[] instruction)
        {
            registers.ProgramCounter = registers.HL;

            return 0;
        }

        [Opcode(Instruction = 0xc3, Mnemonic = "JMP [a16]", Length = 3, Duration = 10)]
        //[Opcode(Instruction = 0xc3, Mnemonic = "*JMP [a16]", Length = 3, Duration = 10)]
        internal int JumpDirectUnconditional(byte[] instruction)
        {
            var address = (ushort)(instruction[2] << 8 | instruction[1]);
            registers.ProgramCounter = address;

            return 0;
        }

        /// <summary>
        /// JMP to a location in memory.
        /// </summary>
        [Opcode(Instruction = 0xc2, Mnemonic = "JNZ [a16]", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0xca, Mnemonic = "JZ [a16]", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0xd2, Mnemonic = "JNC [a16]", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0xda, Mnemonic = "JC [a16]", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0xe2, Mnemonic = "JPO [a16]", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0xea, Mnemonic = "JPE [a16]", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0xf2, Mnemonic = "JP [a16]", Length = 3, Duration = 10)]
        [Opcode(Instruction = 0xfa, Mnemonic = "JM [a16]", Length = 3, Duration = 10)]
        internal int JumpDirectConditional(byte[] instruction)
        {
            if (instruction[0] == 0xc2 && !flags.Z ||
                instruction[0] == 0xca && flags.Z ||
                instruction[0] == 0xd2 && !flags.C ||
                instruction[0] == 0xda && flags.C ||
                instruction[0] == 0xe2 && !flags.P ||
                instruction[0] == 0xea && flags.P ||
                instruction[0] == 0xf2 && !flags.S ||
                instruction[0] == 0xfa && flags.S)
            {
                var address = (ushort)(instruction[2] << 8 | instruction[1]);
                registers.ProgramCounter = address;

                // if we successfully evaluated a condition, it took 5 more cycles.
                return 5;
            }

            return 0;
        }
    }
}
