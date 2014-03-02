// <copyright file="Cpu.Call.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    using System;

    public partial class Cpu
    {
        [Opcode(Instruction = 0xcd, Mnemonic = "CALL  [a16]H", Length = 3, Duration = 17)]
        internal int CallUnconditional(OpcodeAttribute opcode, byte[] instruction)
        {
            var address = Utility.MakeWord(instruction[2], instruction[1]);

            PushWord(registers.ProgramCounter);
            registers.ProgramCounter = address;

            return 0;
        }

        [Opcode(Instruction = 0xc4, Mnemonic = "CNZ  [a16]H", Length = 3, Duration = 11)]
        [Opcode(Instruction = 0xcc, Mnemonic = "CZ   [a16]H", Length = 3, Duration = 11)]
        [Opcode(Instruction = 0xd4, Mnemonic = "CNC  [a16]H", Length = 3, Duration = 11)]
        [Opcode(Instruction = 0xdc, Mnemonic = "CC   [a16]H", Length = 3, Duration = 11)]
        [Opcode(Instruction = 0xe4, Mnemonic = "CPO  [a16]H", Length = 3, Duration = 11)]
        [Opcode(Instruction = 0xec, Mnemonic = "CPE  [a16]H", Length = 3, Duration = 11)]
        [Opcode(Instruction = 0xf4, Mnemonic = "CP   [a16]H", Length = 3, Duration = 11)]
        [Opcode(Instruction = 0xfc, Mnemonic = "CM   [a16]H", Length = 3, Duration = 11)]
        internal int CallConditional(OpcodeAttribute opcode, byte[] instruction)
        {
            // instruction encodes the flag from 0..3 in bits 4 & 5
            // instruction encodes the test (true or false) in bit 3.
            var flag = (Flag)((instruction[0] >> 4) & 3);
            var test = (instruction[0] & 8) == 0 ? false : true;

            if (registers.Flags[flag] == test)
            {
                PushWord(registers.ProgramCounter);

                var address = Utility.MakeWord(instruction[2], instruction[1]);
                registers.ProgramCounter = address;

                // if we successfully evaluated a condition, it took 7 more cycles.
                return 7;
            }

            return 0;
        }

        [Opcode(Instruction = 0xc7, Mnemonic = "RST  0", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0xcf, Mnemonic = "RST  1", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0xd7, Mnemonic = "RST  2", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0xdf, Mnemonic = "RST  3", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0xe7, Mnemonic = "RST  4", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0xef, Mnemonic = "RST  5", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0xf7, Mnemonic = "RST  6", Length = 1, Duration = 11)]
        [Opcode(Instruction = 0xff, Mnemonic = "RST  7", Length = 1, Duration = 11)]
        internal int CallSubroutine(OpcodeAttribute opcode, byte[] instruction)
        {
            PushWord(registers.ProgramCounter);

            // the address of the sub is in bits 4-6. (0b00111000 mask)
            // 0=0x0000, 1=0x0008, ..., 7=0x0038
            registers.ProgramCounter = (ushort)(instruction[0] & 0x38);

            return 0;
        }
    }
}
