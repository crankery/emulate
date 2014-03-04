// <copyright file="Intel8080Cpu.Interrupt.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Intel8080Cpu
    {
        [Opcode(Instruction = 0xf3, Mnemonic = "DI", Length = 1, Duration = 4)]
        internal int DisableInterurrupts(OpcodeAttribute opcode, byte[] instruction)
        {
            interruptEnable = false;

            return 0;
        }

        [Opcode(Instruction = 0xfb, Mnemonic = "EI", Length = 1, Duration = 4)]
        internal int EnableInterrupts(OpcodeAttribute opcode, byte[] instruction)
        {
            interruptEnable = true;

            return 0;
        }
    }
}
