namespace Crankery.Emulate.Console
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Timers;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Sort of vt100 terminal emulation
    /// The vt100 escape codes are from Ray Burns http://stackoverflow.com/a/3223875/399148
    /// TODO: sanity checks.
    /// </summary>
    public class TerminalDisplay
    {
        private const int Width = 80;
        private const int Height = 24;
        private const int CharacterWidth = 10;
        private const int CharacterHeight = 20;
        private static readonly Color Background = Colors.Black;

        private readonly WriteableBitmap display;
        private readonly byte[] cells = new byte[Width * Height];
        private readonly Glyphs glyphs = new Glyphs();

        private int x;
        private int y;
        private string escape;
        private string escapeArgs;
        private bool showCursor;
        private bool hideCursor;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        
        public TerminalDisplay()
        {
            hideCursor = false;
            display = BitmapFactory.New(Width * CharacterWidth, Height * CharacterHeight);

            ClearRange(0, 0, Width - 1, Height - 1);
        }

        public WriteableBitmap Display
        {
            get
            {
                return display;
            }
        }

        public int X
        {
            get
            {
                return x;
            }

            private set
            {
                x = value;

                if (x >= Width)
                {
                    // move down to the start of the next line.
                    x = 0;
                    Y++;
                }

                if (x < 0)
                {
                    x = Width -1;
                    Y--;
                }
            }
        }

        public int Y
        {
            get
            {
                return y;
            }

            private set
            {
                y = value;

                if (y >= Height)
                {
                    y = Height - 1;
                    Scroll();
                }

                if (y < 0)
                {
                    y = 0;
                }
            }
        }

        public bool ShowCursor
        {
            get
            {
                return showCursor;
            }

            set
            {
                showCursor = value;
                DrawCursor();
            }
        }

        private bool HideCursor
        {
            get
            {
                return hideCursor;
            }

            set
            {
                hideCursor = value;
                DrawCursor();
            }
        }

        public void DisplayCharacter(byte c)
        {
            lock (this)
            {
                HideCursor = true;

                var ch = (char)c;
                if (escape == null)
                {
                    switch (ch)
                    {
                        case '\x1b':
                            StartEscape();
                            break;
                        case '\r':
                            X = 0;
                            break;
                        case '\n':
                            Y++;
                            break;
                        case '\b':
                            X--;
                            break;
                        default:
                            DrawCharacter(c);
                            X++;
                            break;
                    }
                }
                else
                {
                    ProcessEscape(ch);
                }

                HideCursor = false;
            }
        }

        private void DrawCursor()
        {
            if (showCursor && !hideCursor)
            {
                display.FillRectangle(
                   CharacterWidth * x,
                   CharacterHeight * (y + 1) - 3,
                   CharacterWidth * (x + 1) - 1,
                   CharacterHeight * (y + 1) - 1,
                   Colors.White);

                display.Blit(
                    new Rect(CharacterWidth * x, CharacterHeight * y, CharacterWidth, CharacterHeight),
                    glyphs[cells[y * Width + x]],
                    new Rect(0, 0, CharacterWidth, CharacterHeight),
                    WriteableBitmapExtensions.BlendMode.Additive);
            }
            else
            {
                display.Blit(
                    new Rect(CharacterWidth * x, CharacterHeight * y, CharacterWidth, CharacterHeight),
                    glyphs[cells[y * Width + x]],
                    new Rect(0, 0, CharacterWidth, CharacterHeight),
                    WriteableBitmapExtensions.BlendMode.None);
            }
        }

        private void StartEscape()
        {
            escape = string.Empty;
            escapeArgs = string.Empty;
        }

        private void ProcessEscape(char c)
        {
            if (escape.Length == 0 && "78".Contains(c))
            {
                escape += c;
            }
            else if (escape.Length > 0 && "()".Contains(escape[0]))
            {
                escape += c;
                if (escape.Length != 2)
                {
                    return;
                }
            }
            else if (escape.Length > 0 && escape[0] == 'Y')
            {
                escape += c;
                if (escape.Length != 3)
                {
                    return;
                }
            }
            else if (c == ';' || char.IsDigit(c))
            {
                escapeArgs += c;
                return;
            }
            else
            {
                escape += c;

                if ("[#?()Y".Contains(c))
                {
                    return;
                }
            }

            ProcessEscapeSequence();
            escape = null;
            escapeArgs = null;
        }

        private void ProcessEscapeSequence()
        {
            if (escape.StartsWith("Y"))
            {
                Y = (int)escape[1] - 64;
                X = (int)escape[2] - 64;

                return;
            }

            //if(_vt52Mode && (escape=="D" || escape=="H")) escape += "_";

            var args = escapeArgs.Split(';');
            int? arg0 = args.Length > 0 && args[0] != "" ? int.Parse(args[0]) : (int?)null;
            int? arg1 = args.Length > 1 && args[1] != "" ? int.Parse(args[1]) : (int?)null;

            switch (escape)
            {
                case "[A":
                case "A":
                    Y -= Math.Max(arg0 ?? 1, 1);
                    break;
                case "[B":
                case "B":
                    Y += Math.Max(arg0 ?? 1, 1);
                    break;
                case "[c":
                case "C":
                    X += Math.Max(arg0 ?? 1, 1);
                    break;
                case "[D":
                case "D":
                    X -= Math.Max(arg0 ?? 1, 1);
                    break;

                case "[f":
                case "[H":
                case "H_":
                    Y = Math.Max(arg0 ?? 1, 1) - 1;
                    X = Math.Max(arg1 ?? 1, 1) - 1;
                    break;

                //case "M": 
                //    PriorRowWithScroll(); 
                //    break;
                //case "D_": 
                //    NextRowWithScroll(); 
                //    break;
                //case "E": 
                //    NextRowWithScroll();
                //    X = 0; 
                //    break;

                //case "[r":
                //    _scrollTop = (arg0 ?? 1) - 1; 
                //    _scrollBottom = (arg0 ?? _height); 
                //    break;

                //case "H": if (!_tabStops.Contains(Column)) _tabStops.Add(Column); break;
                //case "g": if (arg0 == 3) _tabStops.Clear(); else _tabStops.Remove(Column); break;

                case "[J":
                case "J":
                    switch (arg0 ?? 0)
                    {
                        case 0:
                            ClearRange(x, y, Width, Height);
                            break;
                        case 1:
                            ClearRange(0, 0, X + 1, Y);
                            break;
                        case 2:
                            ClearRange(0, 0, Width, Height);
                            break;
                    }
                    break;
                case "[K":
                case "K":
                    switch (arg0 ?? 0)
                    {
                        case 0:
                            ClearRange(X, Y, Width, Y);
                            break;
                        case 1:
                            ClearRange(0, Y, X + 1, Y);
                            break;
                        case 2:
                            ClearRange(0, Y, Width, Y);
                            break;
                    }
                    break;

                //case "?l":
                //case "?h":
                //    var h = escape == "?h";
                //    switch (arg0)
                //    {
                //        case 2: _vt52Mode = h; break;
                //        case 3: Width = h ? 132 : 80; ResetBuffer(); break;
                //        case 7: _autoWrapMode = h; break;
                //    }
                //    break;
                //case "<": _vt52Mode = false; break;

                //case "m":
                //    if (args.Length == 0)
                //    {
                //        ResetCharacterModes();
                //    }

                //    foreach (var arg in args)
                //        switch (arg)
                //        {
                //            case "0": ResetCharacterModes(); break;
                //            case "1": _boldMode = true; break;
                //            case "2": _lowMode = true; break;
                //            case "4": _underlineMode = true; break;
                //            case "5": _blinkMode = true; break;
                //            case "7": _reverseMode = true; break;
                //            case "8": _invisibleMode = true; break;
                //        }
                //    UpdateBrushes();
                //    break;

                //case "#3":
                //case "#4":
                //case "#5":
                //case "#6":
                //    _doubleMode = (CharacterDoubling)((int)escape[1] - (int)'0');
                //    break;

                //case "[s": _saveRow = Row; _saveColumn = Column; break;
                //case "7": _saveRow = Row; _saveColumn = Column;
                //    _saveboldMode = _boldMode; _savelowMode = _lowMode;
                //    _saveunderlineMode = _underlineMode; _saveblinkMode = _blinkMode;
                //    _savereverseMode = _reverseMode; _saveinvisibleMode = _invisibleMode;
                //    break;
                //case "[u": Row = _saveRow; Column = _saveColumn; break;
                //case "8": Row = _saveRow; Column = _saveColumn;
                //    _boldMode = _saveboldMode; _lowMode = _savelowMode;
                //    _underlineMode = _saveunderlineMode; _blinkMode = _saveblinkMode;
                //    _reverseMode = _savereverseMode; _invisibleMode = _saveinvisibleMode;
                //    break;

                //case "c": Reset(); break;
            }
        }

        private void ClearRange(int left, int top, int right, int bottom)
        {
            display.FillRectangle(
                CharacterWidth * left, 
                CharacterHeight * left,
                CharacterWidth * Math.Min(right, Width) - 1,
                CharacterHeight * Math.Min(bottom, Height) - 1,
                Background);

            for (int x = left; x < Math.Min(right, Width); x++)
            {
                for (int y = top; y < Math.Min(bottom, Height); y++)
                {
                    cells[y * Width + x] = (byte)' ';
                }
            }
        }

        private void Scroll()
        {
            display.Blit(
                new Rect(0, 0, CharacterWidth * Width, CharacterHeight * (Height - 1)),
                display,
                new Rect(0, CharacterHeight, CharacterWidth * Width, CharacterHeight * (Height - 1)),
                WriteableBitmapExtensions.BlendMode.None);
            
            display.FillRectangle(0, CharacterHeight * (Height - 1), CharacterWidth * Width - 1, CharacterHeight * Height - 1, Background);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 1; y < Height; y++)
                {
                    cells[(y - 1) * Width + x] = cells[1 * Width + x];
                }
            }

            for (int x = 0; x < Width; x++)
            {
                cells[(Height-1) * Width + x] = (byte)' ';
            }
        }

        private void DrawCharacter(byte c)
        {
            cells[y * Width + x] = c;

            // the glyphs are currently 10x20 pixel bitmaps
            display.Blit(
                new Rect(CharacterWidth * x, CharacterHeight * y, CharacterWidth, CharacterHeight),
                glyphs[c], 
                new Rect(0, 0, CharacterWidth, CharacterHeight),
                WriteableBitmapExtensions.BlendMode.None);
        }
    }
}
