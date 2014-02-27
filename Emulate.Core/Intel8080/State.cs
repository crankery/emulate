// <copyright file="DebugLogEntry.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    using System.Text;

    public class State
    {
        private readonly byte fetch0;
        private readonly byte fetch1;
        private readonly byte fetch2;
        private readonly OpcodeAttribute opcode;
        private readonly Registers registers;

        public State(OpcodeAttribute opcode, byte[] fetch, Registers registers, ushort originalProgramCounter)
        {
            this.opcode = opcode;
            this.fetch0 = fetch[0];
            this.fetch1 = fetch[1];
            this.fetch2 = fetch[2];
            this.registers = new Registers(registers);
            this.registers.ProgramCounter = originalProgramCounter;
        }

        public override string ToString()
        {
            var tracer = new StringBuilder();

            tracer.AppendFormat(
                "{0:x4}: {1:x2} {2} {3}",
                registers.ProgramCounter,
                fetch0,
                opcode.Length > 1 ? fetch1.ToString("x2") : "  ",
                opcode.Length > 2 ? fetch2.ToString("x2") : "  ");

            // fix up the mnemonic to display immediate constant values
            var m = opcode.Mnemonic;
            if (m.Contains("[a16]"))
            {
                m = m.Replace("[a16]", string.Format("{1:x2}{0:x2}h", fetch1, fetch2));

            }
            else if (m.Contains("[d8]"))
            {
                m = m.Replace("[d8]", string.Format("{0:x2}h", fetch1));
            }

            tracer.AppendFormat(
                " {0,-20} | bc={1:x2}{2:x2} de={3:x2}{4:x2} hl={5:x2}{6:x2} a={7:x2} sp={8:x4} {9}{10}{11}{12}{13}",
                m.ToLower(),
                registers.B,
                registers.C,
                registers.D,
                registers.E,
                registers.H,
                registers.L,
                registers.A,
                registers.StackPointer,
                registers.Flags.S ? 'S' : '-',
                registers.Flags.Z ? 'Z' : '-',
                registers.Flags.A ? 'A' : '-',
                registers.Flags.P ? 'P' : '-',
                registers.Flags.C ? 'C' : '-');

            return tracer.ToString();
        }
    }
}
