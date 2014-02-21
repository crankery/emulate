// <copyright file="Flags.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    /// <summary>
    /// Condition codes (bits)
    /// </summary>
    public class Flags
    {
        public Flags()
        {
        }

        public Flags(Flags flags)
        {
            Combined = flags.Combined;
        }

        /// <summary>
        /// Zero
        /// </summary>
        public bool Z { get; set; }

        /// <summary>
        /// Sign
        /// </summary>
        public bool S { get; set; }

        /// <summary>
        /// Parity
        /// </summary>
        public bool P { get; set; }

        /// <summary>
        /// Carry
        /// </summary>
        public bool C { get; set; }

        /// <summary>
        /// Auxillary carry
        /// </summary>
        public bool A { get; set; }

        /// <summary>
        /// Get the value as a byte
        /// </summary>
        public byte Combined
        {
            get
            {
                return (byte)
                    ((S ? 128 : 0) +
                    (Z ? 64 : 0) +
                    (A ? 16 : 0) +
                    (P ? 4 : 0) +
                    2 + // always set
                    (C ? 1 : 0));
            }

            set
            {
                S = (value & 128) != 0;
                Z = (value & 64) != 0;
                A = (value & 16) != 0;
                P = (value & 4) != 0;
                C = (value & 1) != 0;
            }
        }

        public void Clear()
        {
            Combined = 0;
        }

        /// <summary>
        /// Set S, Z & P flags based on the value.
        /// </summary>
        /// <param name="value"></param>
        public void Update(byte value)
        {
            S = value.IsNegative();
            Z = value == 0;
            P = value.EvenParity();
        }
    }
}
