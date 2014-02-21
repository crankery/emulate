// <copyright file="InputPort.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    public class InputPort : Port
    {
        public InputPort(byte address)
            : base(address)
        {
        }

        public virtual byte Read()
        {
            return 0;
        }
    }
}
