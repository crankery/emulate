// <copyright file="BinLoad.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    /// <summary>
    /// Load a binary file at a location in memory.
    /// </summary>
    public static class BinLoad
    {
        public static void Load(this IMemory memory, byte[] bytes, ushort address)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                memory.Write(address++, bytes[i]);
            }
        }
    }
}
