// <copyright file="Intel8080Cpu.CarrryBit.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Intel8080Cpu
    {
        /// <summary>
        /// Complement the carry.
        /// </summary>
        [Opcode(Instruction = 0x3f, Mnemonic = "CMC", Length = 1, Duration = 4)]
        internal int ComplementCarry(OpcodeAttribute opcode, byte[] instruction)
        {
            registers.Flags.C = !registers.Flags.C;

            return 0;
        }

        /// <summary>
        /// Set the carry.
        /// </summary>
        [Opcode(Instruction = 0x37, Mnemonic = "STC", Length = 1, Duration = 4)]
        internal int SetCarry(OpcodeAttribute opcode, byte[] instruction)
        {
            registers.Flags.C = true;

            return 0;
        }
    }
}
