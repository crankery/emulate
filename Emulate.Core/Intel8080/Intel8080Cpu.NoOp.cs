// <copyright file="Intel8080Cpu.NoOp.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Intel8080Cpu
    {
        [Opcode(Instruction = 0x00, Mnemonic = "NOP", Length = 1, Duration = 4)]
        internal int NoOperation(OpcodeAttribute opcode, byte[] instruction)
        {
            return 0;
        }
    }
}
