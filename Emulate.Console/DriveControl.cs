namespace Crankery.Emulate.Console
{
    using System;

    [Flags]
    public enum DriveControl
    {
        TrackIn = 1 << 0,
        TrackOut = 1 << 1,
        LoadHead = 1 << 2,
        UnloadHead = 1 << 3,
        EnableInterrupts = 1 << 4,
        DisableInterrupts = 1 << 5,
        LowerHeadCurrent = 1 << 6,
        StartWriteSequence = 1 << 7
    }
}
