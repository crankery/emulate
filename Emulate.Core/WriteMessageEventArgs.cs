namespace Crankery.Emulate.Core
{
    using System;

    public class WriteMessageEventArgs : EventArgs
    {
        public object Message { get; set; }
    }
}
