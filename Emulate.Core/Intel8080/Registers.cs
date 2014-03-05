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

        /// <summary>
        /// Create registers.
        /// </summary>
        /// <param name="memory">A reference to the system's memory.</param>
        public Registers(IMemory memory)
        {
            this.memory = memory;
            
            Flags = new Flags(); 
        }

        public Registers(Registers registers)
        {
            memory = registers.memory;
            Flags = new Flags(registers.Flags);
            A = registers.A;
            B = registers.B;
            C = registers.C;
            D = registers.D;
            E = registers.E;
            H = registers.H;
            L = registers.L;
            ProgramCounter = registers.ProgramCounter;
            StackPointer = registers.StackPointer;
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
        /// Flag registers.
        /// </summary>
        public Flags Flags { get; private set; }

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
                        return Utility.MakeWord(A, Flags.Combined);
                    default:
                        return 0x0;
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
                        Flags.Combined = value.GetLow();
                        break;
                    default:
                        throw new NotImplementedException("Unknown pair: " + pair);
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
                        return 0x0;
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
                        throw new NotImplementedException("Unknown register: " + register);
                }
            }
        }

        public void Clear()
        {
            A = 0;
            B = 0;
            C = 0;
            D = 0;
            E = 0;
            H = 0;
            L = 0;
            Flags.Clear();
            ProgramCounter = 0;
            StackPointer = 0;
        }

        private ushort GetCombined(Register h, Register l)
        {
            return Utility.MakeWord(this[h], this[l]);
        }

        private void SetCombined(ushort value, Register h, Register l)
        {
            this[h] = value.GetHigh();
            this[l] = value.GetLow();
        }
    }
}
