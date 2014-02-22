// <copyright file="RegisterPair.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public enum RegisterPair
    {
        BC,
        DE,
        HL,
        SP, // stack pointer
        AF // accumulator/flags
    }
}
