// <copyright file="Mos6502Cpu.FlowControl.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    using System;

    public partial class Mos6502Cpu
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
                    Goto(p, opcode.Length);
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

        [OpcodeEx(Instruction = 0x20, Mnemonic = "JSR [a16]", Length = 3, Duration = 6, AddressingMode = AddressingMode.Absolute)]
        internal int JumpSubroutine(OpcodeExAttribute opcode, byte[] instruction)
        {
            // push PC - 1 to the stack. odd.
            var t = (ushort)(registers.ProgramCounter - 1);
            Push(t.GetHigh());
            Push(t.GetLow());
            Goto(Utility.MakeWord(instruction[2], instruction[1]), opcode.Length);

            return 0;
        }

        [OpcodeEx(Instruction = 0x90, Mnemonic = "BCC [r8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Relative)]
        internal int BranchCarryClear(OpcodeExAttribute opcode, byte[] instruction)
        {
            return Branch(!registers.Flags.C, (sbyte)instruction[1]);
        }

        [OpcodeEx(Instruction = 0xb0, Mnemonic = "BCS [r8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Relative)]
        internal int BranchCarrySet(OpcodeExAttribute opcode, byte[] instruction)
        {
            return Branch(registers.Flags.C, (sbyte)instruction[1]);
        }

        [OpcodeEx(Instruction = 0xf0, Mnemonic = "BEQ [r8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Relative)]
        internal int BranchEqual(OpcodeExAttribute opcode, byte[] instruction)
        {
            return Branch(registers.Flags.Z, (sbyte)instruction[1]);
        }

        [OpcodeEx(Instruction = 0xd0, Mnemonic = "BNE [r8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Relative)]
        internal int BranchNotEqual(OpcodeExAttribute opcode, byte[] instruction)
        {
            return Branch(!registers.Flags.Z, (sbyte)instruction[1]);
        }

        [OpcodeEx(Instruction = 0x30, Mnemonic = "BMI [r8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Relative)]
        internal int BranchMinus(OpcodeExAttribute opcode, byte[] instruction)
        {
            return Branch(registers.Flags.N, (sbyte)instruction[1]);
        }

        [OpcodeEx(Instruction = 0x10, Mnemonic = "BPL [r8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Relative)]
        internal int BranchPlus(OpcodeExAttribute opcode, byte[] instruction)
        {
            return Branch(!registers.Flags.N, (sbyte)instruction[1]);
        }

        [OpcodeEx(Instruction = 0x50, Mnemonic = "BVC [r8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Relative)]
        internal int BranchOverflowClear(OpcodeExAttribute opcode, byte[] instruction)
        {
            return Branch(!registers.Flags.V, (sbyte)instruction[1]);
        }

        [OpcodeEx(Instruction = 0x70, Mnemonic = "BVS [r8]", Length = 2, Duration = 2, AddressingMode = AddressingMode.Relative)]
        internal int BranchOverflowSet(OpcodeExAttribute opcode, byte[] instruction)
        {
            return Branch(registers.Flags.V, (sbyte)instruction[1]);
        }

        [OpcodeEx(Instruction = 0x60, Mnemonic = "RTS", Length = 1, Duration = 6, AddressingMode = AddressingMode.Absolute)]
        internal int ReturnFromSubroutine(OpcodeExAttribute opcode, byte[] instruction)
        {
            ReturnFromSubroutine(opcode.Length, 1);

            return 0;
        }

        [OpcodeEx(Instruction = 0x40, Mnemonic = "RTI", Length = 1, Duration = 6, AddressingMode = AddressingMode.Absolute)]
        internal int ReturnFromInterrupt(OpcodeExAttribute opcode, byte[] instruction)
        {
            PullProcessorStatusWord();
            ReturnFromSubroutine(opcode.Length, 0);

            return 0;
        }

        internal void ReturnFromSubroutine(int opcodeLength, int adjust)
        {
            // pull PC - 1 from the stack then add one to it to fix the address.
            var l = Pop();
            var h = Pop();
            var p = (ushort)(Utility.MakeWord(h, l) + adjust);

            Goto(p, opcodeLength);
        }

        internal void Goto(ushort address, int opcodeLength)
        {
            var oldProgramCounter = registers.ProgramCounter - opcodeLength;
            registers.ProgramCounter = address;

            // detect infinite loops and call them halts
            if (oldProgramCounter == address)
            {
                Log(string.Format("-- HALT -- (infinite loop detected at {0:x4})", address));
                IsHalted = true;
            }
        }

        internal int Branch(bool condition, sbyte where)
        {
            if (condition)
            {
                // branch to a relative position from the start of the branch instruction (2 bytes)
                var address = (ushort)(registers.ProgramCounter + where);

                // use the goto to detect looping in one place :)
                Goto(address, 2);

                // return 1 extra cycle if the branch occurred
                // return 2 extra cycles if the branch went to another page.
                return ((address & 0xff00) != (registers.ProgramCounter & 0xff00)) ? 2 : 1;
            }

            // branch didn't occur so no extra cycles.
            return 0;
        }
    }
}
