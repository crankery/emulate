﻿// <copyright file="RegisterPair.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    public enum RegisterPair
    {
        BC,
        DE,
        HL,
        AF, // accumulator/flags
        SP // stack pointer
    }
}
