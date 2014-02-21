// <copyright file="Cpu.Halt.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Cpu
    {
        [Opcode(Instruction = 0x76, Mnemonic = "HLT", Length = 1, Duration = 7)]
        internal int Halt(byte[] instruction)
        {
            IsHalted = true;

            return 0;
        }
    }
}
