namespace Crankery.Emulate.Common
{
    using System;

    /// <summary>
    /// Send byte event args.
    /// </summary>
    public class SendByteEventArgs : EventArgs
    {
        public delegate void SendByteEvent(object sender, SendByteEventArgs e);

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public byte Value { get; set; }
    }
}
