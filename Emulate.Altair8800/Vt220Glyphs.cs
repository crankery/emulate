namespace Crankery.Emulate.Altair8800
{
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Crankery.Emulate.Common;

    /// <summary>
    /// Terminal glyphs.
    /// </summary>
    public class Vt220Glyphs : Glyphs
    {
        private readonly static Color Foreground = Colors.White;
        private readonly static Color Background = Colors.Black;

        /// <summary>
        /// Initializes a new instance of the <see cref="Glyphs"/> class.
        /// </summary>
        public Vt220Glyphs()
            : base(10, 20)
        {
            // via http://www.vt100.net/dec/vt220/glyphs
            // thanks Paul Williams for explaining this. :)
            ExtractGlyphs(@"Resources\vt220-rom-separated.png");
        }

        private void ExtractGlyphs(string imagePath)
        {
            using (var imageStreamSource = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                var bitmapSource = decoder.Frames[0];

                // the image is 4bpp just to be a pain really, covert to 8bpp.
                var converted = new FormatConvertedBitmap(bitmapSource, PixelFormats.Gray8, BitmapPalettes.Gray4, 1);
                var frame = BitmapFrame.Create(converted);

                int stride = (frame.PixelWidth * frame.Format.BitsPerPixel + 7) / 8;
                int size = frame.PixelHeight * stride;
                var buf = new byte[size];

                frame.CopyPixels(buf, stride, 0);

                // the glyphs are drawn on a 16x16 array of cells
                // there are two blank lines before each row of bitmaps
                // there is a blank line before and after each column of bitmaps.

                // the source's bitmaps are 10 high x 8 wide
                for (int col = 0; col < 16; col++)
                {
                    for (int row = 0; row < 16; row++)
                    {
                        // the render bitmaps are 10x20 (double high, scan lines)
                        // any pixels set in col[7] are repeated in cols 8 & 9 for line drawing.

                        var glyph = BitmapFactory.New(Width, Height);
                        glyph.Clear(Background);
                        this[(byte)(row + col * 16)] = glyph;

                        for (int y = 0; y < 10; y++)
                        {
                            for (int x = 0; x < 8; x++)
                            {
                                var offset = stride * 2 + (row * 12 + y) * stride + col * 10 + x + 1;
                                var p = buf[offset];

                                if (p == 0)
                                {
                                    glyph.SetPixel(x, y * 2, 0xff, Foreground);
                                    glyph.SetPixel(x, y * 2 + 1, 0xff, Foreground);

                                    if (x == 7)
                                    {
                                        glyph.SetPixel(8, y * 2, 0xff, Foreground);
                                        glyph.SetPixel(8, y * 2 + 1, 0xff, Foreground);
                                        glyph.SetPixel(8, y * 2, 0xff, Foreground);
                                        glyph.SetPixel(9, y * 2 + 1, 0xff, Foreground);
                                    }
                                }
                            }
                        }
                    }
                }

                // swap 0 & ' '
                // the space character is upside down question mark for some reason.
                // while the space lives happily at zero
                var space = this[(byte)' '];
                this[(byte)' '] = this[0];
                this[0] = space;
            }
        }
    }
}
