﻿namespace Crankery.Emulate.Core
{
    using System;
    using System.Collections.Generic;

    public static class HexLoad
    {
        public static void Load(this IMemory memory, IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                if (line.StartsWith(":") && line.Length > 9)
                {
                    var recordType = GetByte(3, line);

                    // TODO: add error checking, etc.
                    if (recordType == 0x00)
                    {
                        var address = ReadAddress(line);
                        var count = GetByte(0, line);

                        for (ushort i = 0; i < count; i++)
                        {
                            memory.Write((ushort)(address + i), GetByte(4 + i, line));
                        }
                    }

                    if (recordType == 0x01)
                    {
                        break;
                    }
                }
            }
        }

        private static ushort ReadAddress(string line)
        {
            var v = line.Substring(3, 4);

            return Convert.ToUInt16(v, 16);
        }

        private static byte GetByte(int idx, string line)
        {
            var v = line.Substring(1 + 2 * idx, 2);

            return Convert.ToByte(v, 16);
        }
    }
}
