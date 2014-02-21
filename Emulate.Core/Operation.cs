namespace Crankery.Emulate.Core
{
    using System;

    public class Operation
    {
        public Func<byte[], int> Execute { get; set; }
       
        public OpcodeAttribute Opcode { get; set; }
    }
}
