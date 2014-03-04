// <copyright file="Mos6502Cpu.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// The main implementation of the MOS Technology 6502 CPU
    /// </summary>
    [Cpu(Endian = Endian.Little)]
    public partial class Mos6502Cpu : Cpu<OpcodeExAttribute>
    {
        public const ushort NonMaskableInterruptVector = 0xfffa;
        public const ushort ResetVector = 0xfffc;
        public const ushort InterruptRequestBreakVector = 0xfffe;

        private readonly Registers registers;

        public Mos6502Cpu(IMemory memory)
            : base(memory)
        {
            registers = new Registers();

            Reset();
        }

        public override ushort ProgramCounter
        {
            get
            {
                return registers.ProgramCounter;
            }

            set
            {
                registers.ProgramCounter = value;
            }
        }

        public override void Reset()
        {
            Trap(ResetVector, false);
        }

        protected override object GetLogMessage(OpcodeExAttribute opcode, ushort originalProgramCounter)
        {
            return new State(opcode, Fetch, registers, originalProgramCounter);
        }

        internal void Trap(ushort vector, bool software)
        {
            // exit the halted state (this is artificial anyway)
            IsHalted = false;

            if (vector != ResetVector)
            {
                // push the program counter
                Push(ProgramCounter.GetHigh());
                Push(ProgramCounter.GetLow());
                PushProcessorStatusWord(software);

                registers.Flags.I = true;
            }

            // continue executing at the address stored in the specefied vector.
            ProgramCounter = Memory.ReadWord(vector);
        }
    }
}
