// <copyright file="Flags.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    using System;

    public class Flags
    {
        private Flag value;

        public Flags()
        {
            value = (Flag)0;
        }

        public Flags(Flags flags)
        {
            Combined = flags.Combined;
        }

        public bool C
        {
            get
            {
                return value.HasFlag(Flag.C);
            }

            set
            {
                if (value)
                {
                    this.value |= Flag.C;
                }
                else
                {
                    this.value &= ~Flag.C;
                }
            }
        }

        public bool Z
        {
            get
            {
                return value.HasFlag(Flag.Z);
            }

            set
            {
                if (value)
                {
                    this.value |= Flag.Z;
                }
                else
                {
                    this.value &= ~Flag.Z;
                }
            }
        }

        public bool I
        {
            get
            {
                return value.HasFlag(Flag.I);
            }

            set
            {
                if (value)
                {
                    this.value |= Flag.I;
                }
                else
                {
                    this.value &= ~Flag.I;
                }
            }
        }

        public bool D
        {
            get
            {
                return value.HasFlag(Flag.D);
            }

            set
            {
                if (value)
                {
                    this.value |= Flag.D;
                }
                else
                {
                    this.value &= ~Flag.D;
                }
            }
        }

        public bool B
        {
            get
            {
                return value.HasFlag(Flag.B);
            }

            set
            {
                if (value)
                {
                    this.value |= Flag.B;
                }
                else
                {
                    this.value &= ~Flag.B;
                }
            }
        }

        public bool V
        {
            get
            {
                return value.HasFlag(Flag.V);
            }

            set
            {
                if (value)
                {
                    this.value |= Flag.V;
                }
                else
                {
                    this.value &= ~Flag.V;
                }
            }
        }

        public bool N
        {
            get
            {
                return value.HasFlag(Flag.N);
            }

            set
            {
                if (value)
                {
                    this.value |= Flag.N;
                }
                else
                {
                    this.value &= ~Flag.N;
                }
            }
        }

        public byte Combined
        {
            get
            {
                return (byte)value;
            }

            set
            {
                this.value = (Flag)value;
            }
        }

        public bool this[Flag flag]
        {
            get
            {
                switch (flag)
                {
                    case Flag.C:
                        return C;

                    case Flag.Z:
                        return Z;

                    case Flag.I:
                        return I;

                    case Flag.D:
                        return D;

                    case Flag.B:
                        return B;

                    case Flag.N:
                        return N;

                    default:
                        return false;
                }
            }

            set
            {
                switch (flag)
                {
                    case Flag.C:
                        C = value;
                        break;

                    case Flag.Z:
                        Z = value;
                        break;

                    case Flag.I:
                        I = value;
                        break;

                    case Flag.D:
                        D = value;
                        break;

                    case Flag.B:
                        B = value;
                        break;

                    case Flag.N:
                        N = value;
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}
