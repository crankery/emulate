namespace Crankery.Emulate.Console
{
    using System;

    [Flags]
    public enum DriveStatus
    {
        ReadyToWrite = 1 << 0,
        HeadMovementOk = 1 << 1,
        Loaded = 1 << 2,
        InterruptsEnabled = 1 << 3,
        TrackZero = 1 << 6,
        ReadyToRead = 1 << 7
    }
}
