// <copyright file="Intel8080Cpu.InputOutput.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Intel8080Cpu
    {
        [Opcode(Instruction = 0xdb, Mnemonic = "IN   [d8]H", Length = 2, Duration = 10)]
        internal int In(OpcodeAttribute opcode, byte[] instruction)
        {
            registers.A = Devices.Read(instruction[1]);

            return 0;
        }

        [Opcode(Instruction = 0xd3, Mnemonic = "OUT  [d8]H", Length = 2, Duration = 10)]
        internal int Out(OpcodeAttribute opcode, byte[] instruction)
        {
            Devices.Write(instruction[1], registers.A);

            return 0;
        }
    }
}
