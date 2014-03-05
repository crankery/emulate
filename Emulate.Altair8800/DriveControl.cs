namespace Crankery.Emulate.Altair8800
{
    using System;

    /// <summary>
    /// Drive control byte.
    /// </summary>
    [Flags]
    public enum DriveControl
    {
        /// <summary>
        /// The track in flag.
        /// </summary>
        TrackIn = 1 << 0,

        /// <summary>
        /// The track out flag.
        /// </summary>
        TrackOut = 1 << 1,

        /// <summary>
        /// The load head flag.
        /// </summary>
        LoadHead = 1 << 2,

        /// <summary>
        /// The unload head flag.
        /// </summary>
        UnloadHead = 1 << 3,

        /// <summary>
        /// The enable interrupts flag.
        /// </summary>
        EnableInterrupts = 1 << 4,

        /// <summary>
        /// The disable interrupts flag.
        /// </summary>
        DisableInterrupts = 1 << 5,

        /// <summary>
        /// The lower head current flag.
        /// </summary>
        LowerHeadCurrent = 1 << 6,

        /// <summary>
        /// The start write sequence flag.
        /// </summary>
        StartWriteSequence = 1 << 7
    }
}
