// <copyright file="Cpu.InputOutput.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Cpu
    {
        [Opcode(Instruction = 0xdb, Mnemonic = "IN   [d8]", Length = 2, Duration = 10)]
        internal int In(byte[] instruction)
        {
            registers.A = Devices.Read(instruction[1]);

            return 0;
        }

        [Opcode(Instruction = 0xd3, Mnemonic = "OUT  [d8]", Length = 2, Duration = 10)]
        internal int Out(byte[] instruction)
        {
            Devices.Write(instruction[1], registers.A);

            return 0;
        }
    }
}
