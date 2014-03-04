namespace Crankery.Emulate.Core
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class CpuAttribute : Attribute
    {
        public Endian Endian { get; set; }
    }
}
