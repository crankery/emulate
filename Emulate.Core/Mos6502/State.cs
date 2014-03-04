// <copyright file="State.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    using System.Text;

    public class State
    {
        private readonly byte fetch0;
        private readonly byte fetch1;
        private readonly byte fetch2;
        private readonly OpcodeExAttribute opcode;
        private readonly Registers registers;

        public State(OpcodeExAttribute opcode, byte[] fetch, Registers registers, ushort originalProgramCounter)
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
                m = m.Replace("[a16]", string.Format("${1:x2}{0:x2}", fetch1, fetch2));

            }
            else if (m.Contains("[d8]"))
            {
                m = m.Replace("[d8]", string.Format("${0:x2}", fetch1));
            }
            else if (m.Contains("[r8]"))
            {
                // this pc value for this is the one after the original instruction not the original one.
                var rel = (sbyte)fetch1;
                if (rel >= 0)
                {
                    var loc = (ushort)((registers.ProgramCounter + 2) + rel);
                    m = m.Replace("[r8]", string.Format("${1:x4} [+${0:x2}]", rel, loc));
                }
                else
                {
                    var r = -1 * rel;
                    var loc = (ushort)((registers.ProgramCounter + 2) - r);
                    m = m.Replace("[r8]", string.Format("${1:x4} [-${0:x2}]", (byte)(r), loc));
                }
            }

            tracer.AppendFormat(
                " {0,-20} | a={1:x2} x={2:x2} y={3:x2} sp=01{4:x2} {5}{6}{7}{8}{9}{10}{11}",
                m.ToLower(),
                registers.A,
                registers.X,
                registers.Y,
                registers.StackPointer,
                registers.Flags.N ? 'N' : '-',
                registers.Flags.V ? 'V' : '-',
                registers.Flags.B ? 'B' : '-',
                registers.Flags.D ? 'D' : '-',
                registers.Flags.I ? 'I' : '-',
                registers.Flags.Z ? 'Z' : '-',
                registers.Flags.C ? 'C' : '-');

            return tracer.ToString();
        }
    }
}
