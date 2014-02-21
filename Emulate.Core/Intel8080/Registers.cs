// <copyright file="Registers.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    using System;

    /// <summary>
    /// Registers
    /// </summary>
    public class Registers
    {
        private readonly IMemory memory;
        private readonly Flags flags;

        /// <summary>
        /// Create registers.
        /// </summary>
        /// <param name="memory">A reference to the system's memory.</param>
        /// <param name="flags">A reference to the CPU flags.</param>
        public Registers(IMemory memory, Flags flags)
        {
            this.memory = memory;
            this.flags = flags; 
        }

        /// <summary>
        /// A register
        /// </summary>
        public byte A { get; set; }

        /// <summary>
        /// B register
        /// </summary>
        public byte B { get; set; }

        /// <summary>
        /// C register
        /// </summary>
        public byte C { get; set; }

        /// <summary>
        /// D register
        /// </summary>
        public byte D { get; set; }

        /// <summary>
        /// E register
        /// </summary>
        public byte E { get; set; }

        /// <summary>
        /// H register
        /// </summary>
        public byte H { get; set; }

        /// <summary>
        /// L register
        /// </summary>
        public byte L { get; set; }

        /// <summary>
        /// Program counter pseudo register
        /// </summary>
        public ushort ProgramCounter { get; set; }

        /// <summary>
        /// Stack pointer pseudo register
        /// </summary>
        public ushort StackPointer { get; set; }

        /// <summary>
        /// B/C combined
        /// </summary>
        public ushort BC
        {
            get
            {
                return GetCombined(Register.B, Register.C);
            }

            set
            {
                SetCombined(value, Register.B, Register.C);
            }
        }

        /// <summary>
        /// D/E combined
        /// </summary>
        public ushort DE
        {
            get
            {
                return GetCombined(Register.D, Register.E);
            }

            set
            {
                SetCombined(value, Register.D, Register.E);
            }
        }

        /// <summary>
        /// H/L combined
        /// </summary>
        public ushort HL
        {
            get
            {
                return GetCombined(Register.H, Register.L);
            }

            set
            {
                SetCombined(value, Register.H, Register.L);
            }
        }

        public ushort this[RegisterPair pair]
        {
            get
            {
                switch (pair)
                {
                    case RegisterPair.BC:
                        return BC;
                    case RegisterPair.DE:
                        return DE;
                    case RegisterPair.HL:
                        return HL;
                    case RegisterPair.SP:
                        return StackPointer;
                    case RegisterPair.AF:
                        return Utility.GetWord(A, flags.Combined);
                    default:
                        throw new ApplicationException("Unknown pair: " + pair);
                }
            }

            set 
            {
                switch (pair)
                {
                    case RegisterPair.BC:
                        BC = value;
                        break;
                    case RegisterPair.DE:
                        DE = value;
                        break;
                    case RegisterPair.HL:
                        HL = value;
                        break;
                    case RegisterPair.SP:
                        StackPointer = value;
                        break;
                    case RegisterPair.AF:
                        A = value.GetHigh();
                        flags.Combined = value.GetLow();
                        break;
                    default:
                        throw new ApplicationException("Unknown pair: " + pair);
                }
            }
        }

        /// <summary>
        /// Get or set register by id.
        /// </summary>
        /// <param name="register">The register id.</param>
        /// <returns>The register's value.</returns>
        public byte this[Register register]
        {
            get
            { 
                switch(register)
                {
                    case Register.A:
                        return A;
                    case Register.B:
                        return B;
                    case Register.C:
                        return C;
                    case Register.D:
                        return D;
                    case Register.E:
                        return E;
                    case Register.H:
                        return H;
                    case Register.L:
                        return L;
                    case Register.M:
                        return memory.Read(HL);
                    default:
                        throw new ApplicationException("Unknown register: " + register);
                }
            }

            set
            {
                switch (register)
                {
                    case Register.A:
                        A = value;
                        break;
                    case Register.B:
                        B = value;
                        break;
                    case Register.C:
                        C = value;
                        break;
                    case Register.D:
                        D = value;
                        break;
                    case Register.E:
                        E = value;
                        break;
                    case Register.H:
                        H = value;
                        break;
                    case Register.L:
                        L = value;
                        break;
                    case Register.M:
                        memory.Write(HL, value);
                        break;
                    default:
                        throw new ApplicationException("Unknown register: " + register);
                }
            }
        }

        private ushort GetCombined(Register h, Register l)
        {
            return Utility.GetWord(this[h], this[l]);
        }

        private void SetCombined(ushort value, Register h, Register l)
        {
            this[h] = value.GetHigh();
            this[l] = value.GetLow();
        }
    }
}
