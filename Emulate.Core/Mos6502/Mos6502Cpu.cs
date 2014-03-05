// <copyright file="Mos6502Cpu.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
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
    }
}
