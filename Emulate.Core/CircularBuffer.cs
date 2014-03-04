// <copyright file="CircularBuffer.cs" company="Crankery">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Dave Hamilton</author>

namespace Crankery.Emulate.Core
{
    using System.Linq;
    using System.Collections.Generic;

    public class CircularBuffer
        : CircularBuffer<object>
    {
        public CircularBuffer(int capacity)
            : base(capacity)
        {
        }

        public IEnumerable<string> Strings
        {
            get
            {
                return Values.Select(x => x.ToString());
            }
        }
    }

    public class CircularBuffer<T>
    {
        private readonly T[] buffer;
        private int next;
        private bool full;

        public CircularBuffer(int capacity)
        {
            buffer = new T[capacity];
            Clear();
        }

        public void Clear()
        {
            next = 0;
            full = false;
        }

        public void Submit(T value)
        {
            if (next >= buffer.Length)
            {
                next = 0;
                full = true;
            }

            // this can advance the 'next pointer' to beyond the end of the buffer.
            buffer[next++] = value;
        }

        public IEnumerable<T> Values
        {
            get
            {
                if (full)
                {
                    for (int i = next; i < buffer.Length; i++)
                    {
                        yield return buffer[i];
                    }
                }

                for (int i = 0; i < next; i++)
                {
                    yield return buffer[i];
                }
            }
        }
    }
}
