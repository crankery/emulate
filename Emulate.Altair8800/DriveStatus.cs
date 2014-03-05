namespace Crankery.Emulate.Altair8800
{
    using System;

    /// <summary>
    /// Drive status byte.
    /// </summary>
    [Flags]
    public enum DriveStatus
    {
        /// <summary>
        /// The ready to write flag.
        /// </summary>
        ReadyToWrite = 1 << 0,

        /// <summary>
        /// The head movement ok flag.
        /// </summary>
        HeadMovementOk = 1 << 1,

        /// <summary>
        /// The loaded flag.
        /// </summary>
        Loaded = 1 << 2,

        /// <summary>
        /// The interrupts enabled flag.
        /// </summary>
        InterruptsEnabled = 1 << 3,

        /// <summary>
        /// The track zero flag.
        /// </summary>
        TrackZero = 1 << 6,

        /// <summary>
        /// The ready to read flag.
        /// </summary>
        ReadyToRead = 1 << 7
    }
}
