// <copyright file="ByteParity.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    public static class ByteParity
    {
        private static readonly bool[] isEven;

        static ByteParity()
        {
            isEven = new bool[256];

            for (int i = 0; i < 256; i++)
            {
                isEven[i] = (
                  ((i & 0x01) != 0 ? 1 : 0) +
                  ((i & 0x02) != 0 ? 1 : 0) +
                  ((i & 0x04) != 0 ? 1 : 0) +
                  ((i & 0x08) != 0 ? 1 : 0) +
                  ((i & 0x10) != 0 ? 1 : 0) +
                  ((i & 0x20) != 0 ? 1 : 0) +
                  ((i & 0x40) != 0 ? 1 : 0) +
                  ((i & 0x80) != 0 ? 1 : 0)) % 2 == 0;
            }
        }

        /// <summary>
        /// Return true if the count of '1' bits in the supplied value is even.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>True on even parity.</returns>
        public static bool EvenParity(this byte v)
        {
            return isEven[(int)v];
        }
    }
}
