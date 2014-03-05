// <copyright file="Intel8080Cpu.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    using System;

    /// <summary>
    /// The main implementation of the Intel 8080 CPU.
    /// </summary>
    [Cpu(Endian = Endian.Little)]
    public partial class Intel8080Cpu : Cpu<OpcodeAttribute>
    {
        private readonly Registers registers;
        private bool interruptEnable;

        public Intel8080Cpu(IMemory memory, IDevices devices)
            : base(memory)
        {
            Devices = devices;
            registers = new Registers(memory);

            Reset();
        }

        public IDevices Devices { get; private set; }

        public override ushort ProgramCounter
        {
            get
            {
                return registers.ProgramCounter;
            }

            set
            {
                registers.ProgramCounter = value;
            }
        }

        public override void Reset()
        {
            // the only documented effect of reset is to set the pc to 0
            // we'll clear the entire Registers instance.
            registers.Clear();

            // you'd think it'd bring it out of halt though
            IsHalted = false;

            // And interrupts are probably enabled again.
            interruptEnable = true;
        }

        public int Interrupt(byte opcode)
        {
            if (interruptEnable)
            {
                // disable interrupts as soon as we start processing one.
                interruptEnable = false;

                // exit halted state.
                IsHalted = false;

                var operation = GetOperation(opcode);
                if (operation.Opcode.Length == 1)
                {
                    // the interrupt's instruction has to be a one byte instruction
                    return ExecuteOperation(operation, new byte[] { opcode });
                }
                else
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "Invalid instruction provided by interrupt: {0}",
                            opcode));
                }
            }

            return 0;
        }

        protected override object GetLogMessage(OpcodeAttribute opcode, ushort originalProgramCounter)
        {
            return new State(opcode, Fetch, registers, originalProgramCounter);
        }
    }
}
