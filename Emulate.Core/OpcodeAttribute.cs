using System;

namespace Crankery.Emulate.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class OpcodeAttribute : Attribute
    {
        public byte Instruction { get; set; }

        public string Mnemonic { get; set; }

        public int Length { get; set; }

        public int Duration { get; set; }

        public OpcodeAttribute()
        {
        }
    }
}
