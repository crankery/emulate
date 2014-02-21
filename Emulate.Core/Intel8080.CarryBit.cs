// <copyright file="Intel8080.CarrryBit.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    public partial class Intel8080
    {
        /// <summary>
        /// Complement the carry.
        /// </summary>
        [Opcode(Instruction = 0x3f, Mnemonic = "CMC", Length = 1, Duration = 4)]
        internal int ComplementCarry(byte[] instruction)
        {
            flags.C = !flags.C;

            return 0;
        }

        /// <summary>
        /// Set the carry.
        /// </summary>
        [Opcode(Instruction = 0x37, Mnemonic = "STC", Length = 1, Duration = 4)]
        internal int SetCarry(byte[] instruction)
        {
            flags.C = true;

            return 0;
        }
    }
}
