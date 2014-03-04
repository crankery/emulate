// <copyright file="Mos6502Cpu.Misc.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    using System;

    public partial class Mos6502Cpu
    {
        [OpcodeEx(Instruction = 0xea, Mnemonic = "NOP", Length = 1, Duration = 2, AddressingMode = AddressingMode.Implied)]
        internal int NoOperation(OpcodeExAttribute opcode, byte[] instruction)
        {
            return 0;
        }

        [OpcodeEx(Instruction = 0x00, Mnemonic = "BRK", Length = 1, Duration = 7, AddressingMode = AddressingMode.Implied)]
        internal int Break(OpcodeExAttribute opcode, byte[] instruction)
        {
            // break is a bit screwy
            // the return address will be set to be one byte beyond the BRK
            // normally, BRK is followed by NOP to handle this sort of thing.
            registers.ProgramCounter++;

            // handle the trap
            Trap(InterruptRequestBreakVector, true);

            return 0;
        }
    }
}
