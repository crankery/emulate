namespace Crankery.Emulate.Altair8800
{
    /// <summary>
    /// The Atair 8800 front panel
    /// TODO: add all the buttons and stuff.
    /// </summary>
    public class FrontPanel
    {
        public FrontPanel(Devices devices)
            : this(devices, 0xff)
        {
        }

        public FrontPanel(Devices devices, byte inputPort)
        {
            devices.RegisterInputPort(
                inputPort,
                () =>
                {
                    // return the upper 8 bits of the address switches from the front panel
                    return 0x10;
                });
        }
    }
}
