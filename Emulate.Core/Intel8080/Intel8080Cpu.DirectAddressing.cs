// <copyright file="Intel8080Cpu.DirectAddressing.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Intel8080Cpu
    {
        /// <summary>
        /// Store accumulator at location.
        /// </summary>
        [Opcode(Instruction = 0x32, Mnemonic = "STA  [a16]H", Length = 3, Duration = 13)]
        internal int StoreAccumulatorDirect(OpcodeAttribute opcode, byte[] instruction)
        {
            var address = Utility.MakeWord(instruction[2], instruction[1]);
            Memory.Write(address, registers.A);

            return 0;
        }

        /// <summary>
        /// Load accumulator from location.
        /// </summary>
        [Opcode(Instruction = 0x3a, Mnemonic = "LDA  [a16]H", Length = 3, Duration = 13)]
        internal int LoadAccumulatorDirect(OpcodeAttribute opcode, byte[] instruction)
        {
            var address = Utility.MakeWord(instruction[2], instruction[1]);
            registers.A = Memory.Read(address);

            return 0;
        }

        /// <summary>
        /// Load HL from direct address.
        /// </summary>
        [Opcode(Instruction = 0x2a, Mnemonic = "LHLD [a16]H", Length = 3, Duration = 16)]
        internal int LoadHLDirect(OpcodeAttribute opcode, byte[] instruction)
        {
            var address = Utility.MakeWord(instruction[2], instruction[1]);
            registers.L = Memory.Read(address);
            registers.H = Memory.Read((ushort)(address + 1));

            return 0;
        }

        /// <summary>
        /// Store HL to direct address.
        /// </summary>
        [Opcode(Instruction = 0x22, Mnemonic = "SHLD [a16]H", Length = 3, Duration = 16)]
        internal int StoreHLDirect(OpcodeAttribute opcode, byte[] instruction)
        {
            var address = Utility.MakeWord(instruction[2], instruction[1]);
            Memory.Write(address, registers.L);
            Memory.Write((ushort)(address + 1), registers.H);

            return 0;
        }
    }
}
