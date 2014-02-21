// <copyright file="Port.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    public abstract class Port
    {
        public Port(byte address)
        {
            Address = address;
        }

        public byte Address { get; private set; }
    }
}
