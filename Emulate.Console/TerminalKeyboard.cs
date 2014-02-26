namespace Crankery.Emulate.Console
{
    public class TerminalKeyboard
    {
        public delegate void KeyPressedEvent(object sendter, byte code);

        public event KeyPressedEvent KeyPressed = delegate { };

        public void OnKeyPressed(byte code)
        {
            lock (this)
            {
                KeyPressed(this, code);
            }
        }
    }
}
