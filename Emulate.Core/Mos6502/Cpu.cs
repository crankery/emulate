// <copyright file="Cpu.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core.Mos6502
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The main implementation of the MOS Technology 6502 CPU
    /// </summary>
    public partial class Cpu : ICpu
    {
        private readonly Dictionary<byte, Operation> operations;
        private readonly Registers registers;
        private readonly byte[] fetch;
        private readonly Action<string> writeDebugMessage;
        private bool isHalted;

        public Cpu(IMemory memory)
        {
            Memory = memory;
            operations = new Dictionary<byte, Operation>();
            registers = new Registers(memory);
            fetch = new byte[3];

            BuildOperations();
            Reset();
        }

        public Cpu(IMemory memory, Action<string> writeDebugMessage)
            : this(memory)
        {
            this.writeDebugMessage = writeDebugMessage;
        }
           
        public IMemory Memory { get; private set; }

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
        }

        public int Step()
        {
            var cycles = 0;
            var originalProgramCounter = registers.ProgramCounter;

            // don't do anyting if we're halted.
            if (!IsHalted)
            {
                // get the next operation code
                fetch[0] = Memory.Read(registers.ProgramCounter++);
                var operation = GetOperation(fetch[0]);

                // grab any operands for the instruction (max two so no need for a loop)
                if (operation.Opcode.Length > 1)
                {
                    // TODO: make this behave properly if we're executing an instruction at end of address space
                    fetch[1] = Memory.Read(registers.ProgramCounter++);
                }

                if (operation.Opcode.Length > 2)
                {
                    // TODO: make this behave properly if we're executing an instruction at end of address space
                    fetch[2] = Memory.Read(registers.ProgramCounter++);
                }

                // execute the operation
                cycles = ExecuteOperation(operation, fetch);

                if (writeDebugMessage != null)
                {
                    var s = new State(operation.Opcode, fetch, registers, originalProgramCounter);
                    writeDebugMessage(s.ToString());
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
            // execute the operation & update our cycle count
            return operation.Opcode.Duration + operation.Execute(operation.Opcode, instruction);
        }

        internal void BuildOperations()
        {
            var methodInfos = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (var methodInfo in methodInfos)
            {
                var opcodeAttributes = methodInfo.GetCustomAttributes<OpcodeExAttribute>();
                foreach (var opcodeAttribute in opcodeAttributes)
                {
                    var d = Delegate.CreateDelegate(typeof(Func<OpcodeExAttribute, byte[], int>), this, methodInfo);
                    var f = (Func<OpcodeExAttribute, byte[], int>)d;
                    var execute = new Func<OpcodeAttribute, byte[], int>(
                        (o, i) => f(o as OpcodeExAttribute, i));

                    var operation = new Operation
                    {
                        Execute = execute,
                        Opcode = opcodeAttribute
                    };

                    operations[opcodeAttribute.Instruction] = operation;
                }
            }
        }

        internal void SetMemory(AddressingMode addressingMode, byte[] instruction, byte value)
        {
            switch (addressingMode)
            {
                case AddressingMode.ZeroPage:
                    // use the first operand as the 8 bit address.
                    Memory.Write((ushort)instruction[1], value);
                    break;
                case AddressingMode.ZeroPageX:
                    Memory.Write((ushort)(instruction[1] + registers.X), value);
                    break;
                case AddressingMode.ZeroPageY:
                    Memory.Write((ushort)(instruction[1] + registers.Y), value);
                    break;
                case AddressingMode.Absolute:
                    Memory.Write(Utility.MakeWord(instruction[2], instruction[1]), value);
                    break;
                case AddressingMode.AbsoluteX:
                    Memory.Write((ushort)(Utility.MakeWord(instruction[2], instruction[1]) + registers.X), value);
                    break;
                case AddressingMode.AbsoluteY:
                    Memory.Write((ushort)(Utility.MakeWord(instruction[2], instruction[1]) + registers.Y), value);
                    break;
                case AddressingMode.IndexedIndirectX:
                    // build a pointer location from the 1 byte operand and the x register (wrap to 8 bits)
                    // load the pointer value from that zero page address
                    // write the value to that location
                    Memory.Write(Memory.ReadWord((ushort)((instruction[1] + registers.X) & 0xff)), value);
                    break;
                case AddressingMode.IndexedIndirectY:
                    // load the pointer from b0 in zero page
                    // add Y to the pointer's value
                    // set the byte at that address.
                    Memory.Write((ushort)(Memory.ReadWord((ushort)instruction[1]) + registers.Y), value);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        internal byte GetMemory(AddressingMode addressingMode, byte[] instruction)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return instruction[1];
                case AddressingMode.ZeroPage:
                    return Memory.Read((ushort)instruction[1]);
                case AddressingMode.ZeroPageX:
                    return Memory.Read((ushort)(instruction[1] + registers.X));
                case AddressingMode.ZeroPageY:
                    return Memory.Read((ushort)(instruction[1] + registers.Y));
                case AddressingMode.Absolute:
                    return Memory.Read(Utility.MakeWord(instruction[2], instruction[1]));
                case AddressingMode.AbsoluteX:
                    return Memory.Read((ushort)(Utility.MakeWord(instruction[2], instruction[1]) + registers.X));
                case AddressingMode.AbsoluteY:
                    return Memory.Read((ushort)(Utility.MakeWord(instruction[2], instruction[1]) + registers.Y));
                case AddressingMode.IndexedIndirectX:
                    return Memory.Read(Memory.ReadWord((ushort)((instruction[1] + registers.X) & 0xff)));
                case AddressingMode.IndexedIndirectY:
                    return Memory.Read((ushort)(Memory.ReadWord((ushort)instruction[1]) + registers.Y));
                default:
                    throw new InvalidOperationException();
            }
        }

        internal bool AddressCrossesPageBoundary(AddressingMode addressingMode, byte[] instruction)
        {
            switch(addressingMode)
            {
                case AddressingMode.AbsoluteX:
                {
                    var a = Utility.MakeWord(instruction[2], instruction[1]);
                    var b = a + registers.X;
                    return (a & 0xff00) != (b & 0xff00);
                }
                case AddressingMode.AbsoluteY:
                {
                    var a = Utility.MakeWord(instruction[2], instruction[1]);
                    var b = a + registers.Y;
                    return (a & 0xff00) != (b & 0xff00);
                }
                case AddressingMode.IndexedIndirectY:
                {
                    var a = Memory.ReadWord((ushort)instruction[1]);
                    var b = a + registers.Y;
                    return (a & 0xff00) != (b & 0xff00);
                }
                default:
                    return false;
            }
        }
    }
}
