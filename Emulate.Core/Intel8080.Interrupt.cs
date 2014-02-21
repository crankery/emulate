// <copyright file="Intel8080.Interrupt.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    public partial class Intel8080
    {
        [Opcode(Instruction = 0xf3, Mnemonic = "DI", Length = 1, Duration = 4)]
        internal int DisableInterurrupts(byte[] instruction)
        {
            interruptEnable = false;

            return 0;
        }

        [Opcode(Instruction = 0xfb, Mnemonic = "EI", Length = 1, Duration = 4)]
        internal int EnableInterrupts(byte[] instruction)
        {
            interruptEnable = true;

            return 0;
        }
    }
}
