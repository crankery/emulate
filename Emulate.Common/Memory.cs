﻿// <copyright file="Memory.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Common
{
    using Crankery.Emulate.Core;

    /// <summary>
    /// Basic block of 64k of ram.
    /// </summary>
    public class Memory : IMemory
    {
        private readonly byte[] ram = new byte[0x10000];

        public Memory()
        {
        }

        public byte Read(ushort address)
        {
            return ram[address];
        }

        public ushort ReadWord(ushort address)
        {
            // TODO: this is a big/little endian issue
            // this is the little impl
            return Utility.MakeWord(ram[address + 1], ram[address]);
        }

        public void Write(ushort address, byte value)
        {
            ram[address] = value;
        }

        public void Write(ushort address, ushort value)
        {
            // TODO: this is a big/little endian issue
            // this is the little impl
            ram[address] = value.GetLow();
            ram[address + 1] = value.GetHigh();
        }
    }
}
