// <copyright file="OpcodeExAttribute.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    public class OpcodeExAttribute : 
        OpcodeAttribute
    {
        public AddressingMode AddressingMode { get; set; }
    }
}
