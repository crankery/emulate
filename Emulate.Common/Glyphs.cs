namespace Crankery.Emulate.Common
{
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Terminal glyphs.
    /// </summary>
    public abstract class Glyphs
    {
        private readonly WriteableBitmap[] glyphs = new WriteableBitmap[256];

        /// <summary>
        /// Initializes a new instance of the <see cref="Glyphs"/> class.
        /// </summary>
        public Glyphs(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="WriteableBitmap"/> for the specified character.
        /// </summary>
        /// <value>
        /// The <see cref="WriteableBitmap"/>.
        /// </value>
        /// <param name="c">The character (more or less ASCII).</param>
        /// <returns>The glyph.</returns>
        public WriteableBitmap this[byte c]
        {
            get
            {
                return glyphs[c];
            }

            protected set
            {
                glyphs[c] = value;
            }
        }
    }
}
