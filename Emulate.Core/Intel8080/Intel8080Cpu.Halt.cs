// <copyright file="Intel8080Cpu.Halt.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Intel8080Cpu
    {
        [Opcode(Instruction = 0x76, Mnemonic = "HLT", Length = 1, Duration = 7)]
        internal int Halt(OpcodeAttribute opcode, byte[] instruction)
        {
            IsHalted = true;

            return 0;
        }
    }
}
