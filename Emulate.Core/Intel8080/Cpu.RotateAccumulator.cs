// <copyright file="Cpu.RotateAccumulator.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public partial class Cpu
    {
        [Opcode(Instruction = 0x07, Mnemonic = "RLC", Length = 1, Duration = 4)]
        internal int RotateAccumulatorLeftThroughCarry(byte[] instruction)
        {
            var c = registers.A >> 7;
            registers.Flags.C = c != 0;
            registers.A = (byte)(((registers.A << 1) & 0xff) | c);
            
            return 0;
        }

        [Opcode(Instruction = 0x0f, Mnemonic = "RRC", Length = 1, Duration = 4)]
        internal int RotateAccumulatorRightThroughCarry(byte[] instruction)
        {
            var c = (registers.A & 1) << 7;
            registers.Flags.C = c != 0;
            registers.A = (byte)((registers.A & 0xfe) >> 1 | c);

            return 0;
        }

        [Opcode(Instruction = 0x17, Mnemonic = "RAL", Length = 1, Duration = 4)]
        internal int RotateAccumulatorLeft(byte[] instruction)
        {
            var c = registers.Flags.C ? 1 : 0;
            registers.Flags.C = (registers.A & 0x80) != 0;
            registers.A = (byte)(((registers.A << 1) & 0xfe) | c);

            return 0;
        }

        [Opcode(Instruction = 0x1f, Mnemonic = "RAR", Length = 1, Duration = 4)]
        internal int RotateAccumulatorRight(byte[] instruction)
        {
            var c = registers.Flags.C ? 0x80 : 0;
            registers.Flags.C = (registers.A & 1) != 0;
            registers.A = (byte)(((registers.A & 0xfe) >> 1) | c);

            return 0;
        }
    }
}
