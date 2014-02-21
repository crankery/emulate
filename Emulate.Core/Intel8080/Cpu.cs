// <copyright file="Cpu.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Intel8080
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// The main implmentation of the i8080 CPU.
    /// </summary>
    public partial class Cpu : ICpu
    {
        private readonly Dictionary<byte, Operation> operations;
        private readonly Registers registers;
        private readonly byte[] fetch;
        private bool interruptEnable;
        private bool isHalted;
        private CircularBuffer<DebugLogEntry> log;

        public Cpu(IMemory memory, IDevices devices)
        {
            Memory = memory;
            Devices = devices;

            operations = new Dictionary<byte, Operation>();
            registers = new Registers(memory);

            BuildOperations();
            Reset();

            fetch = new byte[3];
        }

        public bool IsHalted
        {
            get
            {
                return isHalted;
            }

            private set
            {
                if (value != isHalted)
                {
                    isHalted = value;
                }
            }
        }

        public bool Debug
        {
            get
            {
                return log != null; 
            }

            set
            {
                if (value)
                {
                    log = new CircularBuffer<DebugLogEntry>(100);
                }
                else
                {
                    log = null;
                }
            }
        }

        public IEnumerable<string> History
        {
            get
            {
                return
                    log == null ? 
                    Enumerable.Empty<string>() :
                    log.Values.Select(x => x.ToString());
            }
        }

        public ulong Cycle { get; private set; }

        public IMemory Memory { get; private set; }

        public IDevices Devices { get; private set; }

        public ushort ProgramCounter
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

        public void Reset()
        {
            // the only documented effect of reset is to set the pc to 0
            // we'll clear the entire Registers instance.
            registers.Clear();

            // you'd think it'd bring it out of halt though
            IsHalted = false;

            // And interrupts are probably enabled again.
            interruptEnable = true;

            // and restart time.
            Cycle = 0;
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

        /// <summary>
        /// Execute a single instruction
        /// </summary>
        /// <returns>How long the instruction took in cycles.</returns>
        public int Step()
        {
            var cycles = 0;
            
            // don't do anyting if we're halted.
            if (!IsHalted)
            {
                // get the next operation code
                fetch[0] = Memory.Read(registers.ProgramCounter++);
                var operation = GetOperation(fetch[0]);

                // grab any operands for the instruction (max two so no need for a loop)
                if (operation.Opcode.Length > 1)
                {
                    fetch[1] = Memory.Read(registers.ProgramCounter++);
                }

                if (operation.Opcode.Length > 2)
                {
                    fetch[2] = Memory.Read(registers.ProgramCounter++);
                }

                // execute the operation
                cycles = ExecuteOperation(operation, fetch);

                if (Debug)
                {
                    log.Submit(new DebugLogEntry(operation.Opcode, fetch, registers));
                }
            }

            return cycles;
        }

        internal Operation GetOperation(byte opcode)
        {
            Operation operation;

            if (operations.TryGetValue(opcode, out operation))
            {
                return operation;
            }

            throw new InvalidOperationException(
                string.Format(
                    "Unknown instruction {0}",
                    opcode));
        }

        internal int ExecuteOperation(Operation operation, byte[] instruction)
        {
            // execute the operation
            var extraCycles = operation.Execute(instruction);

            // update our cycle count
            var cycles = operation.Opcode.Duration + extraCycles;

            Cycle += (ulong)cycles;

            return cycles;
        }

        internal void BuildOperations()
        {
            var methodInfos = typeof(Cpu).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var methodInfo in methodInfos)
            {
                var opcodeAttributes = methodInfo.GetCustomAttributes(typeof(OpcodeAttribute));
                foreach (OpcodeAttribute opcodeAttribute in opcodeAttributes)
                {
                    var execute = new Func<byte[], int>(
                        (p) => (int)methodInfo.Invoke(this, new object[] { p }));

                    var operation = new Operation
                    {
                        Execute = execute,
                        Opcode = opcodeAttribute
                    };

                    operations[opcodeAttribute.Instruction] = operation;
                }
            }
        }
    }
}
