namespace Crankery.Emulate.Console
{
    using Crankery.Emulate.Core;
    using System;

    [Flags]
    public enum DriveStatus
    {
        ReadyToWrite = 1 << 0,
        HeadMovementOk = 1 << 1,
        ReadyForReadWrite = 1 << 2,
        InterruptsEnabled = 1 << 3,
        Track0 = 1 << 6,
        ReadyToWriteX = 1 << 7
    }

    public class Disk
    {
        private const int sectorLength = 137;
        private const int sectorCount = 32;
        private const int trackCount = 77;

        private int selectedDrive = -1;
        private int currentTrack = -1;
        private int currentSector = -1;
        private int currentByte = -1;

        public Disk(Devices devices)
            : this(devices, 0x8, 0x9, 0xa)
        {
        }

        public Disk(Devices devices, byte port0, byte port1, byte port2)
        {
            // drive status
            devices.RegisterInputPort(
                port0,
                () =>
                {

                    DriveStatus status = (DriveStatus)0;

                    // note we invert this value on the way out
                    // the logic was reversed in hardware
                    return (byte)~status;
                });

            // sector position
            devices.RegisterInputPort(
                port1,
                () =>
                {
                    // bit0 => 1 = sector position valid
                    // bit1-5 => sector number (0-31)

                    return (byte)(currentSector >= 0 ? ((currentSector << 1) | 1) : 0);
                });

            // read data
            devices.RegisterInputPort(
                port2,
                () =>
                {

                    return 0xff;
                });

            // drive select
            devices.RegisterOutputPort(
                port0,
                (b) =>
                {
                    // lower 4 bits -> disk number
                    // bit 7 -> 0 = clear selected device, 1 => select device


                });

            // drive control
            devices.RegisterOutputPort(
                port1,
                (b) =>
                {
                    // bit0 => 1 = move out on track
                    // bit1 => 1 = move out one track
                    // bit2 => 1 = load head
                    // bit3 => 1 = unload head
                    // bit4 => 1 = enable interrupts
                    // bit5 => 1 = disable interupts
                    // bit6 => 1 = lower head current (meh)
                    // bit7 => 1 = start write sequence

                });

            // write data
            devices.RegisterOutputPort(
                port2,
                (b) =>
                {
                    // pull a byte off the buffer and stick it in the next address
                });
        }

        /// <summary>
        /// Load a disk image into a drive.
        /// </summary>
        /// <param name="driveNumber">Drive number.</param>
        /// <param name="image">The disk image.</param>
        public void LoadDisk(byte driveNumber, byte[] image)
        {
        }
    }
}
