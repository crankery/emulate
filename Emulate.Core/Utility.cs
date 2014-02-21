// <copyright file="Utility.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    public static class Utility
    {
        public static bool IsNegative(this byte value)
        {
            var x = (sbyte)value;
            return x < 0;
        }

        public static byte GetHigh(this ushort w)
        {
            return (byte)(w >> 8);
        }

        public static byte GetLow(this ushort w)
        {
            return (byte)w;
        }

        public static ushort GetWord(byte h, byte l)
        {
            return (ushort)((int)h << 8 | (int)l);
        }
    }
}
