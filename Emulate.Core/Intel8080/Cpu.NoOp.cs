// <copyright file="Cpu.NoOp.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Cpu
    {
        [Opcode(Instruction = 0x00, Mnemonic = "NOP", Length = 1, Duration = 4)]
        internal int NoOp(byte[] instruction)
        {
            return 0;
        }
    }
}
