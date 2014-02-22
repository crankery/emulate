// <copyright file="Cpu.Return.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Cpu
    {
        [Opcode(Instruction = 0xc9, Mnemonic = "RET", Length = 1, Duration = 10)]
        //[Opcode(Instruction = 0xd9, Mnemonic = "*RET", Length = 1, Duration = 10)]
        internal int ReturnUnconditional(byte[] instruction)
        {
            registers.ProgramCounter = Pop();

            return 0;
        }

        [Opcode(Instruction = 0xc0, Mnemonic = "RNZ", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0xc8, Mnemonic = "RZ", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0xd0, Mnemonic = "RNC", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0xd8, Mnemonic = "RC", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0xe0, Mnemonic = "RPO", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0xe8, Mnemonic = "RPE", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0xf0, Mnemonic = "RP", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0xf8, Mnemonic = "RM", Length = 1, Duration = 5)]
        internal int ReturnConditional(byte[] instruction)
        {
            // instruction encodes the flag from 0..3 in bits 4 & 5
            // instruction encodes the test (true or false) in bit 3.
            var flag = (Flag)((instruction[0] >> 4) & 3);
            var test = (instruction[0] & 8) == 0 ? false : true;

            if (registers.Flags[flag] == test)
            {
                registers.ProgramCounter = Pop();

                // if we successfully evaluated a condition, it took 6 more cycles.
                return 6;
            }

            return 0;
        }
    }
}
