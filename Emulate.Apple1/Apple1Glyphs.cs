namespace Crankery.Emulate.Apple1
{
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Crankery.Emulate.Common;

    public class Apple1Glyphs
        : Glyphs
    {
        private readonly static Color Foreground = Colors.White;
        private readonly static Color Background = Colors.Black;

        public Apple1Glyphs()
            : base(14, 16)
        {
            ExtractGlyphs(@"Resources\apple1.vid");
        }
        private void ExtractGlyphs(string path)
        {
            var bytes = File.ReadAllBytes(path);

            // the rom file has 5x7 glyphs inside an 8x8 grid.
            // there's a blank line on the l5eft so 6x8
            // there are 128 images in this rom file. there's only supposed to be 64 but they've been put in their ASCII positions already.
            for(int idx = 0; idx < 128; idx++)
            {
                var glyph = BitmapFactory.New(Width, Height);
                glyph.Clear(Background);

                this[(byte)idx] = glyph;

                for (var y = 0; y < 8; y++)
                {
                    var p = bytes[idx * 8 + y];
                    for (var x = 0; x < 5; x++)
                    {
                        if ((p & (1 << (x + 1))) != 0)
                        {
                            glyph.SetPixel((x * 2), y * 2, Foreground);
                            glyph.SetPixel((x * 2) + 1, y * 2, Foreground);
                            glyph.SetPixel((x * 2), y * 2 + 1, Foreground);
                            glyph.SetPixel((x * 2) + 1, y * 2 + 1, Foreground);
                        }
                    }
                }
            }
        }
    }
}
