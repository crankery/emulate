// <copyright file="OpcodeAttribute.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class OpcodeAttribute : Attribute
    {
        public byte Instruction { get; set; }

        public string Mnemonic { get; set; }

        public int Length { get; set; }

        public int Duration { get; set; }

        public OpcodeAttribute()
        {
        }
    }
}
