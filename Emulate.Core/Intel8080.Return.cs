// <copyright file="Intel8080.Return.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    public partial class Intel8080
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
            if (instruction[0] == 0xc0 && !flags.Z ||
                instruction[0] == 0xc8 && flags.Z ||
                instruction[0] == 0xd0 && !flags.C ||
                instruction[0] == 0xd8 && flags.C ||
                instruction[0] == 0xe0 && !flags.P ||
                instruction[0] == 0xe8 && flags.P ||
                instruction[0] == 0xf0 && !flags.S ||
                instruction[0] == 0xf8 && flags.S)
            {
                registers.ProgramCounter = Pop();

                // if we successfully evaluated a condition, it took 6 more cycles.
                return 6;
            }

            return 0;
        }
    }
}
