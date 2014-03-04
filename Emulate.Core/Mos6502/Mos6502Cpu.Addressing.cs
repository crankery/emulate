// <copyright file="Mos6502Cpu.Addressing.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    using System;

    public partial class Mos6502Cpu
    {
        internal void SetMemory(AddressingMode addressingMode, byte[] instruction, byte value)
        {
            switch (addressingMode)
            {
                case AddressingMode.Accumulator:
                    registers.A = value;
                    break;
                case AddressingMode.ZeroPage:
                    // use the first operand as the 8 bit address.
                    Memory.Write((ushort)instruction[1], value);
                    break;
                case AddressingMode.ZeroPageX:
                    // wrap to zero page
                    Memory.Write((ushort)((instruction[1] + registers.X) & 0xff), value);
                    break;
                case AddressingMode.ZeroPageY:
                    // wrap to zero page
                    Memory.Write((ushort)((instruction[1] + registers.Y) & 0xff), value);
                    break;
                case AddressingMode.Absolute:
                    Memory.Write(Utility.MakeWord(instruction[2], instruction[1]), value);
                    break;
                case AddressingMode.AbsoluteX:
                    Memory.Write((ushort)(Utility.MakeWord(instruction[2], instruction[1]) + registers.X), value);
                    break;
                case AddressingMode.AbsoluteY:
                    Memory.Write((ushort)(Utility.MakeWord(instruction[2], instruction[1]) + registers.Y), value);
                    break;
                case AddressingMode.IndexedIndirectX:
                    // build a pointer location from the 1 byte operand and the x register (wrap to 8 bits)
                    // load the pointer value from that zero page address
                    // write the value to that location
                    Memory.Write(Memory.ReadWord((ushort)((instruction[1] + registers.X) & 0xff)), value);
                    break;
                case AddressingMode.IndexedIndirectY:
                    // load the pointer from b0 in zero page
                    // add Y to the pointer's value
                    // set the byte at that address.
                    Memory.Write((ushort)(Memory.ReadWord((ushort)instruction[1]) + registers.Y), value);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        internal byte GetMemory(AddressingMode addressingMode, byte[] instruction)
        {
            switch (addressingMode)
            {
                case AddressingMode.Accumulator:
                    return registers.A;
                case AddressingMode.Immediate:
                    return instruction[1];
                case AddressingMode.ZeroPage:
                    return Memory.Read((ushort)instruction[1]);
                case AddressingMode.ZeroPageX:
                    return Memory.Read((ushort)((instruction[1] + registers.X) & 0xff));
                case AddressingMode.ZeroPageY:
                    return Memory.Read((ushort)((instruction[1] + registers.Y) & 0xff));
                case AddressingMode.Absolute:
                    return Memory.Read(Utility.MakeWord(instruction[2], instruction[1]));
                case AddressingMode.AbsoluteX:
                    return Memory.Read((ushort)(Utility.MakeWord(instruction[2], instruction[1]) + registers.X));
                case AddressingMode.AbsoluteY:
                    return Memory.Read((ushort)(Utility.MakeWord(instruction[2], instruction[1]) + registers.Y));
                case AddressingMode.IndexedIndirectX:
                    return Memory.Read(Memory.ReadWord((ushort)((instruction[1] + registers.X) & 0xff)));
                case AddressingMode.IndexedIndirectY:
                    return Memory.Read((ushort)(Memory.ReadWord((ushort)instruction[1]) + registers.Y));
                default:
                    throw new InvalidOperationException();
            }
        }

        internal bool AddressCrossesPageBoundary(AddressingMode addressingMode, byte[] instruction)
        {
            switch (addressingMode)
            {
                case AddressingMode.AbsoluteX:
                {
                    var a = Utility.MakeWord(instruction[2], instruction[1]);
                    var b = a + registers.X;
                    return (a & 0xff00) != (b & 0xff00);
                }
                case AddressingMode.AbsoluteY:
                {
                    var a = Utility.MakeWord(instruction[2], instruction[1]);
                    var b = a + registers.Y;
                    return (a & 0xff00) != (b & 0xff00);
                }
                case AddressingMode.IndexedIndirectY:
                {
                    var a = Memory.ReadWord((ushort)instruction[1]);
                    var b = a + registers.Y;
                    return (a & 0xff00) != (b & 0xff00);
                }
                default:
                    return false;
            }
        }
    }
}
