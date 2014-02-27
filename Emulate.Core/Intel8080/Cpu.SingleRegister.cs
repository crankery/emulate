// <copyright file="Cpu.SingleRegister.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    using System;

    public partial class Cpu
    {
        /// <summary>
        /// Increment a register.
        /// </summary>
        /// <param name="instruction"></param>
        [Opcode(Instruction = 0x04, Mnemonic = "INR  B", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x14, Mnemonic = "INR  D", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x24, Mnemonic = "INR  H", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x34, Mnemonic = "INR  M", Length = 1, Duration = 10)]
        [Opcode(Instruction = 0x0c, Mnemonic = "INR  C", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x1c, Mnemonic = "INR  E", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x2c, Mnemonic = "INR  L", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x3c, Mnemonic = "INR  A", Length = 1, Duration = 4)]
        internal int IncrementRegister(byte[] instruction)
        {
            IncrementOrDecrementRegister(instruction, true);

            return 0;
        }

        /// <summary>
        /// Decrement a register.
        /// </summary>
        /// <param name="instruction"></param>
        [Opcode(Instruction = 0x05, Mnemonic = "DCR  B", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x15, Mnemonic = "DCR  D", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x25, Mnemonic = "DCR  H", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x35, Mnemonic = "DCR  M", Length = 1, Duration = 10)]
        [Opcode(Instruction = 0x0d, Mnemonic = "DCR  C", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x1d, Mnemonic = "DCR  E", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x2d, Mnemonic = "DCR  L", Length = 1, Duration = 4)]
        [Opcode(Instruction = 0x3d, Mnemonic = "DCR  A", Length = 1, Duration = 4)]
        internal int DecrementRegister(byte[] instruction)
        {
            IncrementOrDecrementRegister(instruction, false);

            return 0;
        }

        /// <summary>
        /// Complement the accumulator (flip all the bits)
        /// </summary>
        /// <param name="instruction"></param>
        [Opcode(Instruction = 0x2f, Mnemonic = "CMA", Length = 1, Duration = 4)]
        internal int ComplementAccumulator(byte[] instruction)
        {
            registers.A ^= (byte)0xff;

            return 0;
        }

        /// <summary>
        /// Decimal adjust accumulator.
        /// </summary>
        /// <param name="instruction"></param>
        [Opcode(Instruction = 0x27, Mnemonic = "DAA", Length = 1, Duration = 4)]
        internal int DecimalAdjustAccumulator(byte[] instruction)
        {
            // http://www.motherboardpoint.com/8080-daa-opcode-t163192.html
            // (1) If the least significant four bits of the accumulator represent a
            // number greater than 9, or if the auxiliary carry bit is equal to one,
            // the accumulator is incremented by six. Otherwise, no incrementing
            // occurs.
            // 
            // (2) If the most significant four bits of the accumulator now represent
            // a number greater than 9, or if the normal carry bit is equal to one
            // (*), the most significant four bits of the accumulator are incremented
            // by six. Otherwise, no incrementing occurs.
            // 
            // (*) The bit I added to get BCD to work is to add "or step 1 caused a
            // carry from bit 7"

            var a = (int)registers.A;
            var l = a & 0xf;
            if (l > 9 || registers.Flags.A)
            {
                // the lower digit is A-F or the half carry is set
                // add 6 to the value to adjust it to a decimal value [0-9]
                a += 6;
                l = a & 0xf; // the lower digit is now [0-9]
            }

            // the high order digit may only get adjusted if the lower digit did.
            // i can't really tell... this seems to allow everything to function OK
            // I really don't think anything of value was ever accomplished with this useless instruction.
            var h = a >> 4;
            if (h > 9 || registers.Flags.C || a > 0xff)
            {
                // add 6 to the upper digit if it's [A-F] or the carry is set or there was a half carry from the lower digit adjustment
                h += 6;

                // combine low and high digits (retaining a possible carry in an upper byte here)
                a = (h << 4) | l;
            }

            registers.A = (byte)a;
            registers.Flags.C = a > 0xff; // the carry is set if the adjustment produced an overflow
            registers.Flags.A = false; // the half carry is always cleared here

            return 0;
        }

        private void IncrementOrDecrementRegister(byte[] instruction, bool increment)
        {
            byte oldValue;
            byte newValue;

            var register = (Register)((instruction[0] >> 3) & 7);
            oldValue = registers[register];

            if (increment)
            {
                // the zero bit is clear for increment instructions
                newValue = (byte)(oldValue + 1);
            }
            else
            {
                // the zero bit is set for decrement instructions
                newValue = (byte)(oldValue - 1);
            }

            registers.Flags.Update(newValue);

            // I think I got this crazy formula from SIMH: http://www.schorn.ch
            // it's repeated here wherever the alternate carry is touched.
            registers.Flags.A = (((1 ^ newValue) ^ oldValue) & 0x10) != 0;

            registers[register] = newValue;
        }
    }
}
