// <copyright file="Registers.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    public class Registers
    {
        public Registers(IMemory memory)
        {
            Flags = new Flags();
        }

        public ushort ProgramCounter { get; set; }

        public byte StackPointer { get; set; }

        public byte A { get; set; }

        public byte X { get; set; }

        public byte Y { get; set; }

        public Flags Flags { get; private set; }
    }
}
