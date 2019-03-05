using System;
using System.Linq;

using NuciXNA.Primitives.Mapping;

namespace NuciXNA.Primitives
{
    /// <summary>
    /// Colour.
    /// </summary>
    public sealed class Colour : IEquatable<Colour>
    {
        /// <summary>
        /// Gets the alpha value.
        /// </summary>
        /// <value>The alpha value</value>
        public byte A { get; set; }

        /// <summary>
        /// Gets or sets the red value.
        /// </summary>
        /// <value>The red value.</value>
        public byte R { get; set; }

        /// <summary>
        /// Gets or sets the green value.
        /// </summary>
        /// <value>The green value.</value>
        public byte G { get; set; }

        /// <summary>
        /// Gets or sets the blue value.
        /// </summary>
        /// <value>The blue value.</value>
        public byte B { get; set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Colour"/> class.
        /// </summary>
        public Colour()
        {
            A = 255;
            R = 0;
            G = 0;
            B = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Colour"/> class.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        public Colour(byte r, byte g, byte b)
            : this()
        {
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Colour"/> class.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        public Colour(int r, int g, int b)
            : this()
        {
            R = (byte)r;
            G = (byte)g;
            B = (byte)b;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Colour"/> class.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        public Colour(byte r, byte g, byte b, byte a)
            : this(r, g, b)
        {
            A = a;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Colour"/> class.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        public Colour(int r, int g, int b, int a)
            : this(r, g, b)
        {
            A = (byte)a;
        }

        #endregion

        #region Predefined colours
        public static Colour Transparent => new Colour(0, 0, 0, 0);

        #region XNA colours

        public static Colour AliceBlue => new Colour(240, 248, 255, 255);
        public static Colour AntiqueWhite => new Colour(250, 235, 215, 255);
        public static Colour Aqua => new Colour(0, 255, 255, 255);
        public static Colour Aquamarine => new Colour(127, 255, 212, 255);
        public static Colour Bisque => new Colour(255, 228, 196, 255);
        public static Colour Black => new Colour(0, 0, 0, 255);
        public static Colour Blue => new Colour(0, 0, 255, 255);
        public static Colour Chocolate => new Colour(210, 105, 30, 255);
        public static Colour CornflowerBlue => new Colour(100, 149, 237, 255);
        public static Colour Cornsilk => new Colour(255, 248, 220, 255);
        public static Colour Crimson => new Colour(220, 20, 60, 255);
        public static Colour DarkGreen => new Colour(0, 100, 0, 255);
        public static Colour DarkRed => new Colour(139, 0, 0, 255);
        public static Colour DimGray => new Colour(105, 105, 105, 255);
        public static Colour DodgerBlue => new Colour(30, 144, 255, 255);
        public static Colour Gold => new Colour(255, 215, 0, 255);
        public static Colour Green => new Colour(0, 255, 0, 255);
        public static Colour Olive => new Colour(128, 128, 0, 255);
        public static Colour OliveDrab => new Colour(107, 142, 45, 255);
        public static Colour Orange => new Colour(255, 165, 0, 255);
        public static Colour Red => new Colour(255, 0, 0, 255);
        public static Colour RoyalBlue => new Colour(65, 105, 255, 255);
        public static Colour Salmon => new Colour(250, 128, 114, 255);
        public static Colour SeaGreen => new Colour(46, 139, 87, 255);
        public static Colour SkyBlue => new Colour(135, 206, 235, 255);
        public static Colour Snow => new Colour(255, 250, 250, 255);
        public static Colour Tan => new Colour(210, 180, 140, 255);
        public static Colour Teal => new Colour(0, 128, 128, 255);
        public static Colour Thistle => new Colour(216, 191, 216, 255);
        public static Colour Tomato => new Colour(255, 99, 71, 255);
        public static Colour Turqoise => new Colour(64, 224, 208, 255);
        public static Colour Violet => new Colour(238, 130, 238, 255);
        public static Colour Wheat => new Colour(245, 222, 179, 255);
        public static Colour White => new Colour(255, 255, 255, 255);
        public static Colour WhiteSmoke => new Colour(245, 245, 245, 255);
        public static Colour Yellow => new Colour(255, 255, 0, 255);
        public static Colour YellowGreen => new Colour(154, 205, 50, 255);

        #endregion
        #region NuciXNA colours

        /// <summary>
        /// Cobal blue, as defined on the romanian flag.
        /// </summary>
        public static Colour CobaltBlue => new Colour(0, 43, 127, 255);

        /// <summary>
        /// Chrome yellow, as defined on the romanian flag.
        /// </summary>
        public static Colour ChromeYellow => new Colour(252, 209, 22, 255);

        public static Colour PersianRed => new Colour(200, 29, 17, 255);

        public static Colour Vermilion => new Colour(227, 66, 52, 255);

        /// <summary>
        /// Vermilion red, as defined on the romanian flag.
        /// </summary>
        public static Colour VermilionRed => new Colour(206, 17, 38, 255);

        #endregion
        #region Pantone colours

        /// <summary>
        /// PANTONE PQ-15-0343TCX Greenery.
        /// </summary>
        public static Colour Greenery => new Colour(136, 176, 75, 255);

        /// <summary>
        /// PANTONE PQ-18-2120TCX Honeysuckle.
        /// </summary>
        public static Colour Honeysuckle => new Colour(217, 79, 112, 255);

        /// <summary>
        /// PANTONE PQ-15-1247TCX Tangerine.
        /// </summary>
        public static Colour Tangerine => new Colour(248, 143, 88, 255);

        /// <summary>
        /// PANTONE PQ-17-1463TCX Tangerine Tango.
        /// </summary>
        public static Colour TangerineTango => new Colour(221, 65, 36, 255);

        /// <summary>
        /// PANTONE PQ-18-3838TCX Ultra Violet.
        /// </summary>
        public static Colour UltraViolet => new Colour(95, 75, 139, 255);

        #endregion
        #region Material colours

        public static Colour MaterialRed => new Colour(244, 67, 54, 255);
        public static Colour MaterialPink => new Colour(233, 30, 99, 255);
        public static Colour MaterialPurple => new Colour(103, 58, 183, 255);
        public static Colour MaterialIndigo => new Colour(63, 81, 181, 255);
        public static Colour MaterialBlue => new Colour(33, 150, 243, 255);
        public static Colour MaterialLightBlue => new Colour(3, 169, 244, 255);
        public static Colour MaterialCyan => new Colour(0, 188, 212, 255);
        public static Colour MaterialTeal => new Colour(0, 150, 136, 255);
        public static Colour MaterialGreen => new Colour(76, 175, 80, 255);
        public static Colour MaterialLightGreen => new Colour(139, 195, 74, 255);
        public static Colour MaterialLime => new Colour(205, 220, 57, 255);
        public static Colour MaterialYellow => new Colour(255, 235, 59, 255);
        public static Colour MaterialAmber => new Colour(255, 193, 7, 255);
        public static Colour MaterialOrange => new Colour(255, 152, 0, 255);
        public static Colour MaterialDeepOrange => new Colour(255, 87, 34, 255);
        public static Colour MaterialBrown => new Colour(121, 85, 72, 255);
        public static Colour MaterialGrey => new Colour(158, 158, 158, 255);
        public static Colour MaterialBlueGrey => new Colour(96, 125, 139, 255);

        #endregion
        #endregion

        #region Mappings

        /// <summary>
        /// Creates a colour from an ARGB integer.
        /// </summary>
        /// <returns>The colour.</returns>
        /// <param name="argb">ARGB integer.</param>
        public static Colour FromArgb(int argb) => ColourTranslator.FromArgb(argb);

        /// <summary>
        /// Creates a colour from RGB values.
        /// </summary>
        /// <returns>The colour.</returns>
        /// <param name="r">Red value.</param>
        /// <param name="g">Green value.</param>
        /// <param name="b">Blue value.</param>
        public static Colour FromArgb(byte r, byte g, byte b) => ColourTranslator.FromArgb(r, g, b);

        /// <summary>
        /// Creates a colour from RGB values.
        /// </summary>
        /// <returns>The colour.</returns>
        /// <param name="r">Red value.</param>
        /// <param name="g">Green value.</param>
        /// <param name="b">Blue value.</param>
        public static Colour FromArgb(int r, int g, int b) => ColourTranslator.FromArgb(r, g, b);

        /// <summary>
        /// Creates a colour from ARGB values.
        /// </summary>
        /// <returns>The colour.</returns>
        /// <param name="a">Alpha value.</param>
        /// <param name="r">Red value.</param>
        /// <param name="g">Green value.</param>
        /// <param name="b">Blue value.</param>
        public static Colour FromArgb(byte a, byte r, byte g, byte b) => ColourTranslator.FromArgb(a, r, g, b);

        /// <summary>
        /// Creates a colour from ARGB values.
        /// </summary>
        /// <returns>The colour.</returns>
        /// <param name="a">Alpha value.</param>
        /// <param name="r">Red value.</param>
        /// <param name="g">Green value.</param>
        /// <param name="b">Blue value.</param>
        public static Colour FromArgb(int a, int r, int g, int b) => ColourTranslator.FromArgb(a, r, g, b);

        /// <summary>
        /// Creates a colour from a hexadecimal code.
        /// </summary>
        /// <returns>The colour.</returns>
        /// <param name="hexa">Hexadecimal code.</param>
        public static Colour FromHexadecimal(string hexa) => ColourTranslator.FromHexadecimal(hexa);

        /// <summary>
        /// Converts the colour to a 32 bit integer.
        /// </summary>
        /// <returns>The ARGB integer value.</returns>
        public int ToArgb() => ColourTranslator.ToArgb(this);

        /// <summary>
        /// Converts the colour to a hexadecimal string.
        /// </summary>
        /// <returns>A string representing the hexadecimal code of the colour.</returns>
        public string ToHexadecimal() => ColourTranslator.ToHexadecimal(this);

        public Colour ToMonochromeAverage()
        {
            int average = (R + G + B) / 3;

            return new Colour(average, average, average, A);
        }

        public Colour ToMonochromeLight()
        {
            int lightest = new int[] { R, G, B }.Max();

            return new Colour(lightest, lightest, lightest, A);
        }

        public Colour ToMonochromeDark()
        {
            int darkest = new int[] { R, G, B }.Min();

            return new Colour(darkest, darkest, darkest, A);
        }

        #endregion

        /// <summary>
        /// Checks wether the current colour is similar to another.
        /// </summary>
        /// <returns><c>true</c>, if it is similar, <c>false</c> otherwise.</returns>
        /// <param name="colour">Colour to compare.</param>
        /// <param name="tolerance">Tolerance.</param>
        public bool IsSimilarTo(Colour colour, int tolerance)
        {
            return Math.Abs(R - colour.R) <= tolerance &&
                   Math.Abs(G - colour.G) <= tolerance &&
                   Math.Abs(B - colour.B) <= tolerance;
        }

        /// <summary>
        /// Multiplies a specified colour by a factor.
        /// </summary>
        /// <returns>The multiply.</returns>
        /// <param name="colour">Colour.</param>
        /// <param name="factor">Factor.</param>
        public static Colour Multiply(Colour colour, float factor)
        {
            byte newA = (byte)(Math.Max(0, Math.Min(255, colour.A * factor)));
            byte newR = (byte)(Math.Max(0, Math.Min(255, colour.R * factor)));
            byte newG = (byte)(Math.Max(0, Math.Min(255, colour.G * factor)));
            byte newB = (byte)(Math.Max(0, Math.Min(255, colour.B * factor)));

            return new Colour(newR, newG, newB, newA);
        }
        
        public override string ToString()
            => ToHexadecimal();

        #region IEquatable and Equals
        
        /// <summary>
        /// Determines whether the specified <see cref="Colour"/> is equal to the current <see cref="Colour"/>.
        /// </summary>
        /// <param name="other">The <see cref="Colour"/> to compare with the current <see cref="Colour"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Colour"/> is equal to the current
        /// <see cref="Colour"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Colour other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return
                A == other.A &&
                R == other.R &&
                G == other.G &&
                B == other.B;
        }

        public bool Equals(int a, int r, int g, int b)
        {
            return A == a && R == r && G == g && B == b;
        }

        public bool Equals(int r, int g, int b)
        {
            return Equals(255, r, g, b);
        }

        public bool Equals(string hexa)
        {
            return Equals(FromHexadecimal(hexa));
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Colour"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Colour"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Colour"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Colour)obj);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Colour"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return
                    A.GetHashCode() ^
                    R.GetHashCode() ^
                    G.GetHashCode() ^
                    B.GetHashCode();
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Multiplies a specified colour by a factor.
        /// </summary>
        /// <returns>The multiply.</returns>
        /// <param name="colour">Colour.</param>
        /// <param name="factor">Factor.</param>
        public static Colour operator *(Colour colour, float factor)
        {
            return Multiply(colour, factor);
        }

        public static bool operator ==(Colour me, Colour other)
        {
            if (me is null)
            {
                return other is null;
            }

            return me.Equals(other);
        }

        public static bool operator !=(Colour me, Colour other)
            => !(me == other);

        public static explicit operator int(Colour me)
            => me.ToArgb();

        public static explicit operator string(Colour me)
            => me.ToString();
        
        public static explicit operator Colour(int argb)
            => Colour.FromArgb(argb);

        public static explicit operator Colour(string hexadecimal)
            => Colour.FromHexadecimal(hexadecimal);

        #endregion
    }
}
