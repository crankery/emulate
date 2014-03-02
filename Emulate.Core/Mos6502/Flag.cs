// <copyright file="Flag.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    using System;

    [Flags]
    public enum Flag
    {
        C = 1 << 0, // Carry
        Z = 1 << 1, // Zero
        I = 1 << 2, // IRQ
        D = 1 << 3, // BCD
        B = 1 << 4, // Break
        V = 1 << 6, // Overflow
        N = 1 << 7, // Negative
    }
}
