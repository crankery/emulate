// <copyright file="Cpu.FlowControl.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    using System;

    public partial class Cpu
    {
        [OpcodeEx(Instruction = 0x4c, Mnemonic = "JMP [a16]", Length = 3, Duration = 3, AddressingMode = AddressingMode.Absolute)]
        [OpcodeEx(Instruction = 0x6c, Mnemonic = "JMP ([a16])", Length = 3, Duration = 5, AddressingMode = AddressingMode.Indirect)]
        internal int Jump(OpcodeExAttribute opcode, byte[] instruction)
        {
            var p = Utility.MakeWord(instruction[2], instruction[1]);

            switch(opcode.AddressingMode)
            {
                case AddressingMode.Absolute:
                {
                    registers.ProgramCounter = p;
                    break;
                }
                case AddressingMode.Indirect:
                {
                    if ((p & 0xff) == 0xff)
                    {
                        // sort of a bug in this cpu.
                        var l = Memory.Read(p); // this is the last byte of a page.
                        var h = Memory.Read((ushort)(p & 0xff00)); // and the high byte comes from the first byte of the same page.
                        Goto(Utility.MakeWord(h, l), opcode.Length);
                    }
                    else
                    {
                        Goto(Memory.ReadWord(p), opcode.Length);
                    }

                    break;
                }
                default:
                {
                    throw new NotSupportedException();
                }
            }

            return 0;
        }

        [OpcodeEx(Instruction = 0x48, Mnemonic = "JSR [a16]", Length = 3, Duration = 6, AddressingMode = AddressingMode.Absolute)]
        internal int JumpSubroutine(OpcodeExAttribute opcode, byte[] instruction)
        {
            // push PC - 1 to the stack. odd.
            var t = (ushort)(registers.ProgramCounter - 1);
            Push(t.GetHigh());
            Push(t.GetLow());
            Goto(Utility.MakeWord(instruction[2], instruction[1]), opcode.Length);

            return 0;
        }


        [OpcodeEx(Instruction = 0x60, Mnemonic = "RTS", Length = 1, Duration = 6, AddressingMode = AddressingMode.Absolute)]
        internal int ReturnFromSubroutine(OpcodeExAttribute opcode, byte[] instruction)
        {
            ReturnFromSubroutine(opcode.Length);

            return 0;
        }

        [OpcodeEx(Instruction = 0x40, Mnemonic = "RTI", Length = 1, Duration = 6, AddressingMode = AddressingMode.Absolute)]
        internal int ReturnFromInterrupt(OpcodeExAttribute opcode, byte[] instruction)
        {
            PullProcessorStatusWord();
            ReturnFromSubroutine(opcode.Length);

            return 0;
        }

        internal void ReturnFromSubroutine(int opcodeLength)
        {
            // pull PC - 1 from the stack then add one to it to fix the address.
            var l = Pop();
            var h = Pop();
            var p = (ushort)(Utility.MakeWord(h, l) + 1);

            Goto(p, opcodeLength);
        }

        internal void Goto(ushort address, int opcodeLength)
        {
            var oldProgramCounter = registers.ProgramCounter - opcodeLength;
            registers.ProgramCounter = address;

            // detect infinite loops and call them "halts".
            if (oldProgramCounter == address)
            {
                IsHalted = true;
            }
        }
    }
}
