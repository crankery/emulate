// <copyright file="Intel8080.DataTransfer.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Intel8080Cpu
    {
        /// <summary>
        /// Move a value from a register back onto itself.
        /// </summary>
        /// <param name="instruction"></param>
        [Opcode(Instruction = 0x40, Mnemonic = "MOV B,B", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x49, Mnemonic = "MOV C,C", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x52, Mnemonic = "MOV D,D", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x5b, Mnemonic = "MOV E,E", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x64, Mnemonic = "MOV H,H", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x6d, Mnemonic = "MOV L,L", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x7f, Mnemonic = "MOV A,A", Length = 1, Duration = 5)]
        internal int MoveNoOp(byte[] instruction)
        {
            return 0;
        }

        /// <summary>
        /// Move a value from one register to another.
        /// </summary>
        /// <param name="instruction"></param>
        [Opcode(Instruction = 0x41, Mnemonic = "MOV B,C", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x42, Mnemonic = "MOV B,D", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x43, Mnemonic = "MOV B,E", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x44, Mnemonic = "MOV B,H", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x45, Mnemonic = "MOV B,L", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x46, Mnemonic = "MOV B,M", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x47, Mnemonic = "MOV B,A", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x48, Mnemonic = "MOV C,B", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x4a, Mnemonic = "MOV C,D", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x4b, Mnemonic = "MOV C,E", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x4c, Mnemonic = "MOV C,H", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x4d, Mnemonic = "MOV C,L", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x4e, Mnemonic = "MOV C,M", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x4f, Mnemonic = "MOV C,A", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x50, Mnemonic = "MOV D,B", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x51, Mnemonic = "MOV D,C", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x53, Mnemonic = "MOV D,E", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x54, Mnemonic = "MOV D,H", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x55, Mnemonic = "MOV D,L", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x56, Mnemonic = "MOV D,M", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x57, Mnemonic = "MOV D,A", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x58, Mnemonic = "MOV E,B", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x59, Mnemonic = "MOV E,C", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x5a, Mnemonic = "MOV E,D", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x5c, Mnemonic = "MOV E,H", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x5d, Mnemonic = "MOV E,L", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x5e, Mnemonic = "MOV E,M", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x5f, Mnemonic = "MOV E,A", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x60, Mnemonic = "MOV H,B", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x61, Mnemonic = "MOV H,C", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x62, Mnemonic = "MOV H,D", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x63, Mnemonic = "MOV H,E", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x65, Mnemonic = "MOV H,L", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x66, Mnemonic = "MOV H,M", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x67, Mnemonic = "MOV H,A", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x68, Mnemonic = "MOV L,B", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x69, Mnemonic = "MOV L,C", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x6a, Mnemonic = "MOV L,D", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x6b, Mnemonic = "MOV L,E", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x6c, Mnemonic = "MOV L,H", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x6e, Mnemonic = "MOV L,M", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x6f, Mnemonic = "MOV L,A", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x70, Mnemonic = "MOV M,B", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x71, Mnemonic = "MOV M,C", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x72, Mnemonic = "MOV M,D", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x73, Mnemonic = "MOV M,E", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x74, Mnemonic = "MOV M,H", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x75, Mnemonic = "MOV M,L", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x77, Mnemonic = "MOV M,A", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x78, Mnemonic = "MOV A,B", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x79, Mnemonic = "MOV A,C", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x7a, Mnemonic = "MOV A,D", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x7b, Mnemonic = "MOV A,E", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x7c, Mnemonic = "MOV A,H", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x7d, Mnemonic = "MOV A,L", Length = 1, Duration = 5)]
        [Opcode(Instruction = 0x7e, Mnemonic = "MOV A,M", Length = 1, Duration = 7)]
        internal int Move(byte[] instruction)
        {
            var source = (Register)(instruction[0] & 7);
            var target = (Register)((instruction[0] >> 3) & 7);

            registers[target] = registers[source];

            return 0;
        }

        /// <summary>
        /// Store the accumulator in memory at BC or DE.
        /// </summary>
        /// <param name="instruction"></param>
        [Opcode(Instruction = 0x02, Mnemonic = "STAX B", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x12, Mnemonic = "STAX D", Length = 1, Duration = 7)]
        internal int StoreAccumulator(byte[] instruction)
        {
            var location = instruction[0] == 0x02 ? registers.BC : registers.DE;

            Memory.Write(location, registers.A);

            return 0;
        }

        /// <summary>
        /// Read a value from memory at BC or DE into the accumulator.
        /// </summary>
        /// <param name="instruction"></param>
        [Opcode(Instruction = 0x0a, Mnemonic = "LDAX B", Length = 1, Duration = 7)]
        [Opcode(Instruction = 0x1a, Mnemonic = "LDAX D", Length = 1, Duration = 7)]
        internal int LoadAccumulator(byte[] instruction)
        {
            var location = instruction[0] == 0x0a ? registers.BC : registers.DE;

            registers.A = Memory.Read(location);

            return 0;
        }
    }
}
