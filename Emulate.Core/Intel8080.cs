namespace Crankery.Emulate.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public partial class Intel8080
    {
        private readonly Dictionary<byte, Operation> operations;
        private readonly Registers registers;
        private readonly Flags flags;
        private readonly byte[] fetch;
        private bool interruptEnable;
        private bool isHalted;
        private List<string> log;

        public Intel8080(IMemory memory, IDevices devices)
        {
            Memory = memory;
            Devices = devices;

            operations = new Dictionary<byte, Operation>();
            flags = new Flags();
            registers = new Registers(memory, flags);

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

                    if (Debug)
                    {
                        Trace.WriteLine(
                            string.Format("-- HALT --"));
                    }
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
                log = new List<string>();
            }
        }

        public IEnumerable<string> History
        {
            get
            {
                return log ?? Enumerable.Empty<string>();
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
            // the only documented effect of reset is to set the pc to 0.
            registers.ProgramCounter = 0;

            // you'd think it'd bring it out of halt though
            IsHalted = false;

            // And interrupts are probably enabled again.
            interruptEnable = true;

            // here's some sensible things to reset although they're not necessary.
            flags.Clear();
            registers.StackPointer = 0;

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
            var result = 0;
            
            // don't do anyting if we're halted.
            if (!IsHalted)
            {
                // get the next operation code
                fetch[0] = Memory.Read(registers.ProgramCounter++);
                var operation = GetOperation(fetch[0]);

                // grab any operands for the instruction
                if (operation.Opcode.Length > 1)
                {
                    fetch[1] = Memory.Read(registers.ProgramCounter++);
                }

                if (operation.Opcode.Length > 2)
                {
                    fetch[2] = Memory.Read(registers.ProgramCounter++);
                }

                result = ExecuteOperation(operation, fetch);

                if (Debug)
                {
                    WriteDebug(operation);
                }
            }

            return result;
        }

        private void WriteDebug(Operation operation)
        {
            var tracer = new StringBuilder();

            // the PC has already been advanced to the next instruction
            tracer.AppendFormat(
                "{0:x4}: {1:x2} {2} {3}",
                registers.ProgramCounter - operation.Opcode.Length,
                fetch[0],
                operation.Opcode.Length > 1 ? fetch[1].ToString("x2") : "  ",
                operation.Opcode.Length > 2 ? fetch[2].ToString("x2") : "  ");

            // fix up the mnemonic to have display immediate constant values
            var m = operation.Opcode.Mnemonic;
            if (m.Contains("[a16]"))
            {
                m = m.Replace("[a16]", string.Format("${1:x2}{0:x2}", fetch[1], fetch[2]));

            }
            else if (m.Contains("[d8]"))
            {
                m = m.Replace("[d8]", string.Format("${0:x2}", fetch[1]));
            }

            tracer.AppendFormat(
                " {0,-20} | bc={1:x2}{2:x2} de={3:x2}{4:x2} hl={5:x2}{6:x2} a={7:x2} sp={8:x4} {9}{10}{11}{12}{13}",
                m.ToLower(),
                registers.B,
                registers.C,
                registers.D,
                registers.E,
                registers.H,
                registers.L,
                registers.A,
                registers.StackPointer,
                flags.S ? 'S' : '-',
                flags.Z ? 'Z' : '-',
                flags.A ? 'A' : '-',
                flags.P ? 'P' : '-',
                flags.C ? 'C' : '-');

            log.Add(tracer.ToString());
            if (log.Count > 100)
            {
                log.RemoveAt(0);
            }
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
            var duration = operation.Opcode.Duration + extraCycles;

            Cycle += (ulong)duration;

            return duration;
        }

        internal void BuildOperations()
        {
            var methodInfos = typeof(Intel8080).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
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
