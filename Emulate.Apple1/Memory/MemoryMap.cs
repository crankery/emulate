namespace Crankery.Emulate.Apple1
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Crankery.Emulate.Core;

    public class MemoryMap : IMemory
    {
        private readonly MemoryCell[] memory;

        public MemoryMap(Ports ports)
        {
            // 64k address space
            memory = new MemoryCell[0x10000];

            // initialize to an 8K system layout (reuse the same object instance)
            var empty = new EmptyMemoryCell();
            AddMemory(0x0000, 0xffff, () => empty);

            // the first 8KB is RAM
            AddMemory(0x0000, 0x1fff, () => new MemoryCell());

            // and there's another 4KB in exxx.
            AddMemory(0xe000, 0xefff, () => new MemoryCell());

            // pre-load the cassette based BASIC in this 4KB block.
            this.Load(File.ReadAllBytes(Path.Combine("Resources", "apple1basic.bin")), 0xe000);

            // the monitor lives in the last page
            LoadRom(0xff00, 0xffff, "apple-a1.bin", "apple-a2.bin");

            // the cassette rom lives in the c1xx page
            LoadRom(0xc100, 0xc1ff, "apple-a3.bin", "apple-a4.bin");

            // the ports are mirrored every 0x20 bytes throughout 0xdXXX (always ends with x10, x11, x12 or x13)
            for (var b = 0xd000; b < 0xe000; b += 0x20)
            {
                memory[b + 0x10] = ports.KeyboardData;
                memory[b + 0x11] = ports.KeyboardControl;
                memory[b + 0x12] = ports.DisplayData;
                memory[b + 0x13] = ports.DisplayControl;
            }
        }

        public byte Read(ushort address)
        {
            return memory[address].Value;
        }

        public ushort ReadWord(ushort address)
        {
            return Utility.MakeWord(Read((ushort)(address + 1)), Read(address));
        }

        public void Write(ushort address, byte value)
        {
            memory[address].Value = value;
        }

        public void Write(ushort address, ushort value)
        {
            Write(address, value.GetLow());
            Write((ushort)(address + 1), value.GetHigh());
        }

        private void AddMemory(ushort start, ushort end, Func<MemoryCell> create)
        {
            for (int i = start; i <= end; i++)
            {
                memory[i] = create();
            }
        }

        private void LoadRom(ushort start, ushort end, string lowNibbleFile, string highNibbleFile)
        {
            var l = File.ReadAllBytes(Path.Combine("Resources", lowNibbleFile));
            var h = File.ReadAllBytes(Path.Combine("Resources", highNibbleFile));

            for (int i = start, o = 0; i <= end; i++, o++)
            {
                memory[i] = new ReadOnlyMemoryCell((byte)((l[o] & 0xf) | ((h[o] & 0xf) << 4)));
            }
        }
    }
}
