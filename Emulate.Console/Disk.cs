namespace Crankery.Emulate.Console
{
    using System;

    public class Disk
    {
        private const int DriveCount = 4; // apparently could be up to 16.
        private const int SectorLength = 137;
        private const int SectorCount = 32;
        private const int TrackCount = 77;

        private int selectedDrive = 0;

        private int[] track = new int[DriveCount];
        private int[] sector = new int[DriveCount];
        private int[] offset = new int[DriveCount];

        private byte[][] diskImage = new byte[DriveCount][];
        private DriveStatus[] diskStatus = new DriveStatus[DriveCount];

        public Disk(Devices devices)
            : this(devices, 0x8, 0x9, 0xa)
        {
        }

        public Disk(Devices devices, byte port0, byte port1, byte port2)
        {
            for (int i = 0; i < DriveCount; i++)
            {
                // put all the current indexes out of bounds
                track[i] = 0xff;
                sector[i] = 0xff;
                offset[i] = 0xff;
            }

            // read drive status
            devices.RegisterInputPort(
                port0,
                () =>
                {
                    // note we invert this value on the way out
                    // the logic was reversed in hardware
                    var status = (byte)~diskStatus[selectedDrive];

                    // clear the reserved bits.
                    status &= 0xe7;

                    return (byte)status;
                });

            // drive select
            devices.RegisterOutputPort(
                port0,
                (b) =>
                {
                    if (selectedDrive < DriveCount)
                    {
                        diskStatus[selectedDrive] = 0;
                    }

                    selectedDrive = b & 0xf;

                    if ((b & 0x80) == 0)
                    {
                        // enable (backwards, yes)
                        diskStatus[selectedDrive] = DriveStatus.HeadMovementOk;
                        offset[selectedDrive] = 0xff;
                        sector[selectedDrive] = 0xff;
                    }
                });

            // read sector position
            devices.RegisterInputPort(
                port1,
                () =>
                { 
                    // bit 0 => 1 = sector position valid (0 is true)
                    // bits 1-5 => sector number (0-31)
                    if (diskStatus[selectedDrive].HasFlag(DriveStatus.Loaded))
                    {
                        // advance sector when sector number is checked
                        // this allows the emulated software to wait for the right one to spin around
                        // somewhat as it normally would have.
                        sector[selectedDrive]++;
                        if (sector[selectedDrive] >= SectorCount)
                        {
                            sector[selectedDrive] = 0;
                        }

                        // ensure that the current byte on this new sector is out of bounds
                        offset[selectedDrive] = 0xff;

                        // sector position is valid, push the 5 bit sector number to the left one position.
                        // the zero bit must be clear to indicate the sector value is valid.
                        return (byte)((sector[selectedDrive] & 0x1f) << 1);
                    }
                    else
                    {
                        // sector position not valid as head is not loaded.
                        return (byte)0;
                    }
                });

            // drive control
            devices.RegisterOutputPort(
                port1,
                (b) =>
                {
                    var cmd = (DriveControl)b;
                    if (cmd.HasFlag(DriveControl.TrackIn))
                    {
                        // move in one track
                        if ((track[selectedDrive]++) >= TrackCount)
                        {
                            track[selectedDrive] = TrackCount - 1;
                        }

                        sector[selectedDrive] = 0xff;
                        offset[selectedDrive] = 0xff;
                    }

                    if (cmd.HasFlag(DriveControl.TrackOut))
                    {
                        // move out one track
                        track[selectedDrive]--;

                        if (track[selectedDrive] >= TrackCount ||
                            track[selectedDrive] < 0)
                        {
                            track[selectedDrive] = 0;
                        }

                        sector[selectedDrive] = 0xff;
                        offset[selectedDrive] = 0xff;
                    }

                    if (cmd.HasFlag(DriveControl.LoadHead))
                    {
                        // load head
                        diskStatus[selectedDrive] |= DriveStatus.Loaded;
                        diskStatus[selectedDrive] |= DriveStatus.ReadyToRead;
                    }

                    if (cmd.HasFlag(DriveControl.UnloadHead))
                    {
                        // unload head
                        diskStatus[selectedDrive] &= ~DriveStatus.Loaded;
                        diskStatus[selectedDrive] &= ~DriveStatus.ReadyToRead;

                        sector[selectedDrive] = 0xff;
                        offset[selectedDrive] = 0xff;
                    }

                    if (cmd.HasFlag(DriveControl.StartWriteSequence))
                    {
                        // start write sequence
                        diskStatus[selectedDrive] |= DriveStatus.ReadyToWrite;

                        // the write method will go to byte zero
                        offset[selectedDrive] = 0xff;
                    }

                    if (track[selectedDrive] == 0)
                    {
                        diskStatus[selectedDrive] |= DriveStatus.TrackZero;
                    }
                    else
                    {
                        diskStatus[selectedDrive] &= ~DriveStatus.TrackZero;
                    }
                });

            // read data
            devices.RegisterInputPort(
                port2,
                () =>
                {
                    if (diskImage[selectedDrive] != null)
                    {
                        offset[selectedDrive]++;
                        if (offset[selectedDrive] >= SectorLength)
                        {
                            offset[selectedDrive] = 0;
                        }

                        var index =
                            track[selectedDrive] * SectorCount * SectorLength +
                            sector[selectedDrive] * SectorLength +
                            offset[selectedDrive];

                        return diskImage[selectedDrive][index];
                    }

                    return 0xff;
                });

            // write data
            devices.RegisterOutputPort(
                port2,
                (b) =>
                {
                    // TODO: check to see that the drive is actually writing.
                    if (diskImage[selectedDrive] != null)
                    {
                        offset[selectedDrive]++;
                        if (offset[selectedDrive] >= SectorLength)
                        {
                            offset[selectedDrive] = 0;
                        }

                        var index =
                            track[selectedDrive] * SectorCount * SectorLength +
                            sector[selectedDrive] * SectorLength +
                            offset[selectedDrive];

                        diskImage[selectedDrive][index] = b;
                    }
                });
        }

        /// <summary>
        /// Load a disk image into a drive.
        /// </summary>
        /// <param name="driveNumber">Drive number.</param>
        /// <param name="image">The disk image (null to unload).</param>
        public void Load(byte driveNumber, byte[] image)
        {
            if (driveNumber > DriveCount)
            {
                throw new ArgumentOutOfRangeException("driveNumber");
            }

            if (image != null)
            {
                // make sure we have the full buffer available.
                diskImage[driveNumber] = new byte[SectorCount * SectorLength * TrackCount];
                Array.Copy(image, diskImage[driveNumber], Math.Min(image.Length, SectorCount * SectorLength * TrackCount));
            }
            else
            {
                diskImage[driveNumber] = null;
            }
        }
    }
}
