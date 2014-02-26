// <copyright file="ICpu.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    public interface ICpu
    {
        /// <summary>
        /// Get the halt state of the CPU
        /// </summary>
        bool IsHalted { get; }

        /// <summary>
        /// Reset the CPU
        /// </summary>
        void Reset();

        /// <summary>
        /// Execute a single instruction.
        /// </summary>
        /// <returns>Number of clock cycles taken.</returns>
        int Step();
    }
}
