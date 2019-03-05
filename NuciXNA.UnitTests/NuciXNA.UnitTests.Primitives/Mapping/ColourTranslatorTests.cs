using System;

using NUnit.Framework;

using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

namespace NuciXNA.UnitTests.Primitives
{
    public class ColourTranslatorTests
    {
        [Test]
        public void ToHexadecimal_CalledWithColour_ReturnsCorrectValue()
        {
            Colour colour = Colour.ChromeYellow;
            string expected = "#FCD116";
            string actual = ColourTranslator.ToHexadecimal(colour);

            Assert.AreEqual(expected, actual);
        }
            
        [Test]
        public void FromHexadecimal_ValidHexLongWithHashWithAlpha_ReturnsCorrectColour()
        {
            Colour expected = new Colour(255, 0, 255, 255);
            Colour actual = ColourTranslator.FromHexadecimal("#FFFF00FF");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FromHexadecimal_ValidHexLongWithHashWithoutAlpha_ReturnsCorrectColour()
        {
            Colour expected = new Colour(255, 0, 255);
            Colour actual = ColourTranslator.FromHexadecimal("#FF00FF");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FromHexadecimal_ValidHexLongWithoutHashWithAlpha_ReturnsCorrectColour()
        {
            Colour expected = new Colour(0, 255, 255, 255);
            Colour actual = ColourTranslator.FromHexadecimal("FF00FFFF");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FromHexadecimal_ValidHexLongWithoutHashWithoutAlpha_ReturnsCorrectColour()
        {
            Colour expected = new Colour(0, 255, 255);
            Colour actual = ColourTranslator.FromHexadecimal("00FFFF");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FromHexadecimal_ValidHexShortWithHashWithAlpha_ReturnsCorrectColour()
        {
            Colour expected = new Colour(255, 0, 255, 255);
            Colour actual = ColourTranslator.FromHexadecimal("#FF0F");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FromHexadecimal_ValidHexShortWithHashWithoutAlpha_ReturnsCorrectColour()
        {
            Colour expected = new Colour(255, 0, 255);
            Colour actual = ColourTranslator.FromHexadecimal("#F0F");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FromHexadecimal_ValidHexShortWithoutHashWithAlpha_ReturnsCorrectColour()
        {
            Colour expected = new Colour(0, 255, 255, 255);
            Colour actual = ColourTranslator.FromHexadecimal("F0FF");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FromHexadecimal_ValidHexShortWithoutHashWithoutAlpha_ReturnsCorrectColour()
        {
            Colour expected = new Colour(0, 255, 255);
            Colour actual = ColourTranslator.FromHexadecimal("0FF");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FromHexadecimal_InvalidHexTwoHashes_FormatException()
        {
            Assert.Throws<FormatException>(() => ColourTranslator.FromHexadecimal("##0FF"));
        }

        [Test]
        public void FromHexadecimal_InvalidHexDigitsOutsideHexRange__FormatException()
        {
            Assert.Throws<FormatException>(() => ColourTranslator.FromHexadecimal("#FZZ"));
        }

        [Test]
        public void FromHexadecimal_InvalidHexTooFewHexes_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ColourTranslator.FromHexadecimal("#FF"));
        }

        [Test]
        public void FromHexadecimal_InvalidHexTooManyHexes_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ColourTranslator.FromHexadecimal("#FF00FF00FF"));
        }

        [Test]
        public void ToArgb_CalledWithColour_ReturnsCorrectValue()
        {
            int expected = 67174915;
            Colour colour = ColourTranslator.FromArgb(expected);

            int actual = ColourTranslator.ToArgb(colour);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToArgb_CalledWithRGB_ReturnsCorrectValue()
        {
            int expected = -16711165;
            int actual = ColourTranslator.ToArgb(1, 2, 3);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToArgb_CalledWithRGBA_ReturnsCorrectValue()
        {
            int expected = -16711165;
            int actual = ColourTranslator.ToArgb(1, 2, 3, 255);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FromArgb_ValidInteger_ReturnsCorrectColour()
        {
            Colour expected = new Colour(1, 2, 3, 4);
            Colour actual = ColourTranslator.FromArgb(67174915);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FromArgb_ValidRGB_ReturnsCorrectColour()
        {
            Colour expected = new Colour(1, 2, 3);
            Colour actual = ColourTranslator.FromArgb(1, 2, 3);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FromArgb_ValidARGB_ReturnsCorrectColour()
        {
            Colour expected = new Colour(1, 2, 3, 0);
            Colour actual = ColourTranslator.FromArgb(0, 1, 2, 3);

            Assert.AreEqual(expected, actual);
        }
    }
}
