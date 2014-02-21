// <copyright file="OutputPort.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    public class OutputPort : Port
    {
        public OutputPort(byte address)
            : base(address)
        {
        }

        public virtual void Write(byte value)
        { 
        }
    }
}
