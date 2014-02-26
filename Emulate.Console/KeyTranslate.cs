namespace Crankery.Emulate.Console
{
    using System.Windows.Input;

    /// <summary>
    /// Try to translate key scans to olde fashioned ascii key codes.
    /// TODO: find a service in Windows that actually does this for you.
    /// </summary>
    public static class KeyTranslate
    {
        public static byte? Translate(Key key)
        {
            var caps = Keyboard.IsKeyToggled(Key.CapsLock);
            var shift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            var control = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            var alt = Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);

            return Translate(key, caps, shift, control, alt);
        }

        public static byte? Translate(Key key, bool caps, bool shift, bool control, bool alt)
        {
            // this has to move to the Terminal class.
            byte? code = null;

            if (!alt)
            {
                if (key == Key.Space)
                {
                    code = (byte)' ';
                }
                else if (key == Key.Enter && !shift)
                {
                    code = (byte)'\r';
                }
                else if (key == Key.Enter && shift)
                {
                    code = (byte)'\n';
                }
                else if (key == Key.Tab)
                {
                    code = (byte)'\t';
                }
                else if (key == Key.Escape)
                {
                    code = (byte)27;
                }
                else if (key == Key.Back)
                {
                    code = (byte)8;
                }
                else if (key >= Key.A && key <= Key.Z)
                {
                    if (!control && !(shift ^ caps))
                    {
                        code = (byte)('a' + ((int)key - (int)Key.A));
                    }
                    else if (!control && (shift ^ caps))
                    {
                        code = (byte)('A' + ((int)key - (int)Key.A));
                    }
                    else
                    {
                        // control+A == ASCII 1
                        code = (byte)(1 + ((int)key - (int)Key.A));
                    }
                }
                else if (!control)
                {
                    if (shift)
                    {
                        switch (key)
                        {
                            case Key.D0:
                                code = (byte)')';
                                break;
                            case Key.D1:
                                code = (byte)'!';
                                break;
                            case Key.D2:
                                code = (byte)'@';
                                break;
                            case Key.D3:
                                code = (byte)'#';
                                break;
                            case Key.D4:
                                code = (byte)'$';
                                break;
                            case Key.D5:
                                code = (byte)'%';
                                break;
                            case Key.D6:
                                code = (byte)'^';
                                break;
                            case Key.D7:
                                code = (byte)'&';
                                break;
                            case Key.D8:
                                code = (byte)'*';
                                break;
                            case Key.D9:
                                code = (byte)'(';
                                break;
                            case Key.OemMinus:
                                code = (byte)'_';
                                break;
                            case Key.OemPlus:
                                code = (byte)'+';
                                break;
                            case Key.OemOpenBrackets:
                                code = (byte)'{';
                                break;
                            case Key.OemCloseBrackets:
                                code = (byte)'}';
                                break;
                            case Key.OemPipe:
                                code = (byte)'|';
                                break;
                            case Key.OemSemicolon:
                                code = (byte)':';
                                break;
                            case Key.OemQuotes:
                                code = (byte)'"';
                                break;
                            case Key.OemComma:
                                code = (byte)'<';
                                break;
                            case Key.OemPeriod:
                                code = (byte)'>';
                                break;
                            case Key.OemQuestion:
                                code = (byte)'?';
                                break;
                            case Key.OemTilde:
                                code = (byte)'~';
                                break;
                        }
                    }
                    else
                    {
                        if (key >= Key.D0 && key <= Key.D9)
                        {
                            code = (byte)('0' + key - Key.D0);
                        }
                        else
                        {
                            switch (key)
                            {
                                case Key.OemMinus:
                                    code = (byte)'-';
                                    break;
                                case Key.OemPlus:
                                    code = (byte)'=';
                                    break;
                                case Key.OemOpenBrackets:
                                    code = (byte)'[';
                                    break;
                                case Key.OemCloseBrackets:
                                    code = (byte)']';
                                    break;
                                case Key.OemPipe:
                                    code = (byte)'\\';
                                    break;
                                case Key.OemSemicolon:
                                    code = (byte)';';
                                    break;
                                case Key.OemQuotes:
                                    code = (byte)'\'';
                                    break;
                                case Key.OemComma:
                                    code = (byte)',';
                                    break;
                                case Key.OemPeriod:
                                    code = (byte)'.';
                                    break;
                                case Key.OemQuestion:
                                    code = (byte)'/';
                                    break;
                                case Key.OemTilde:
                                    code = (byte)'`';
                                    break;
                            }
                        }
                    }
                }
            }

            return code;
        }
    }
}
