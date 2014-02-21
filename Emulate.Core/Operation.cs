// <copyright file="Operation.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    using System;

    public class Operation
    {
        public Func<byte[], int> Execute { get; set; }
       
        public OpcodeAttribute Opcode { get; set; }
    }
}
