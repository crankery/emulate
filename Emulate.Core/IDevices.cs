// <copyright file="IDevices.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    public interface IDevices
    {
        byte Read(byte port);
        void Write(byte port, byte value);
    }
}
