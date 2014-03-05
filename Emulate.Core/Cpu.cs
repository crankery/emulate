// <copyright file="Cpu.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public abstract class Cpu<TOpcode> where TOpcode : OpcodeAttribute
    {
        private readonly Dictionary<byte, Operation<TOpcode>> operations = new Dictionary<byte, Operation<TOpcode>>();
        private readonly byte[] fetch = new byte[3];

        public delegate void WriteMessage(object sender, WriteMessageEventArgs e);

        public event WriteMessage WriteLogMessage;

        protected Cpu(IMemory memory)
        {
            Memory = memory;
            BuildOperations();
        }

        public IMemory Memory { get; private set; }

        public bool IsHalted { get; set; }

        public abstract ushort ProgramCounter { get; set; }

        protected byte[] Fetch
        {
            get
            {
                return fetch;
            }
        }

        public abstract void Reset();

        /// <summary>
        /// Execute a single instruction
        /// </summary>
        /// <returns>How long the instruction took in cycles.</returns>
        public int Step()
        {
            var cycles = 0;
            var originalProgramCounter = ProgramCounter;

            // don't do anyting if we're halted.
            if (!IsHalted)
            {
                // get the next operation code
                fetch[0] = Memory.Read(ProgramCounter++);
                var operation = GetOperation(fetch[0]);

                // grab any operands for the instruction (max two so no need for a loop)
                if (operation.Opcode.Length > 1)
                {
                    // TODO: make this behave properly if we're executing an instruction at end of address space
                    fetch[1] = Memory.Read(ProgramCounter++);
                }

                if (operation.Opcode.Length > 2)
                {
                    // TODO: make this behave properly if we're executing an instruction at end of address space
                    fetch[2] = Memory.Read(ProgramCounter++);
                }

                // execute the operation
                cycles = ExecuteOperation(operation, fetch);

                // (potentially) log the operation
                LogOperation(operation.Opcode, originalProgramCounter);
            }

            return cycles;
        }

        protected int ExecuteOperation(Operation<TOpcode> operation, byte[] instruction)
        {
            // execute the operation & update our cycle count
            return operation.Opcode.Duration + operation.Execute(operation.Opcode, instruction);
        }

        protected void BuildOperations()
        {
            var methodInfos = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (var methodInfo in methodInfos)
            {
                var opcodeAttributes = methodInfo.GetCustomAttributes<TOpcode>();
                foreach (var opcodeAttribute in opcodeAttributes)
                {
                    var execute = (Func<TOpcode, byte[], int>)Delegate.CreateDelegate(typeof(Func<TOpcode, byte[], int>), this, methodInfo);

                    var operation = new Operation<TOpcode>
                    {
                        Execute = execute,
                        Opcode = opcodeAttribute
                    };

                    if (operations.ContainsKey(opcodeAttribute.Instruction))
                    {
                        throw new ApplicationException(string.Format("Duplicate opcode: 0x{0:x2}", opcodeAttribute.Instruction));
                    }

                    operations[opcodeAttribute.Instruction] = operation;
                }
            }
        }

        protected Operation<TOpcode> GetOperation(byte opcode)
        {
            Operation<TOpcode> operation;

            if (operations.TryGetValue(opcode, out operation))
            {
                return operation;
            }

            throw new InvalidOperationException(
                string.Format(
                    "Unknown instruction 0x{0:x2}",
                    opcode));
        }

        protected abstract object GetLogMessage(TOpcode opcode, ushort originalProgramCounter);

        internal void LogOperation(TOpcode opcode, ushort originalProgramCounter)
        {
            if (WriteLogMessage != null)
            {
                WriteLogMessage(this, new WriteMessageEventArgs { Message = GetLogMessage(opcode, originalProgramCounter) });
            }
        }

        internal void Log(string message)
        {
            if (WriteLogMessage != null)
            {
                WriteLogMessage(this, new WriteMessageEventArgs { Message = message });
            }
        }
    }
}
