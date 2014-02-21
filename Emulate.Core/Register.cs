// <copyright file="Register.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    public enum Register
    {
        B = 0,
        C,
        D,
        E,
        H,
        L,
        M, // pseduo-register pointed to via HL
        A
    }
}
