using System;

using NUnit.Framework;

using NuciXNA.Primitives;

namespace NuciXNA.UnitTests.Primitives
{
    public class ColourTests
    {
        [Test]
        public void Constructor_RGB_PropertiesSet()
        {
            Colour colour = new Colour(1, 2, 3);

            Assert.AreEqual(1, colour.R);
            Assert.AreEqual(2, colour.G);
            Assert.AreEqual(3, colour.B);
            Assert.AreEqual(byte.MaxValue, colour.A);
        }

        [Test]
        public void Constructor_RGBA_PropertiesSet()
        {
            Colour colour = new Colour(1, 2, 3, 4);

            Assert.AreEqual(1, colour.R);
            Assert.AreEqual(2, colour.G);
            Assert.AreEqual(3, colour.B);
            Assert.AreEqual(4, colour.A);
        }

        [Test]
        public void ToMonochromeAverage_CorrectColourReturned()
        {
            Colour colour = new Colour(8, 16, 24);
            Colour expected = new Colour(16, 16, 16);
            Colour actual = colour.ToMonochromeAverage();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToMonochromeDark_CorrectColourReturned()
        {
            Colour colour = new Colour(8, 16, 24);
            Colour expected = new Colour(8, 8, 8);
            Colour actual = colour.ToMonochromeDark();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToMonochromeLight_CorrectColourReturned()
        {
            Colour colour = new Colour(8, 16, 24);
            Colour expected = new Colour(24, 24, 24);
            Colour actual = colour.ToMonochromeLight();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsSimilarTo_CorrectValueReturned()
        {
            Colour colour1 = new Colour(8, 16, 24);
            Colour colour2 = new Colour(10, 18, 26);
            bool isSimilar = colour1.IsSimilarTo(colour2, 2);

            Assert.AreEqual(true, isSimilar);
        }

        [Test]
        public void Multiply_CalledWithValidFactor_CorrectColourReturned()
        {
            Colour colour = new Colour(8, 8, 8, 8);
            Colour expected = new Colour(16, 16, 16, 16);
            Colour actual = Colour.Multiply(colour, 2);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Multiply_CalledWithHugeFactor_MaxColourReturned()
        {
            Colour colour = new Colour(8, 8, 8, 8);
            Colour expected = new Colour(255, 255, 255, 255);
            Colour actual = Colour.Multiply(colour, 100);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Multiply_CalledWithNegativeFactor_MinColourReturned()
        {
            Colour colour = new Colour(8, 8, 8, 8);
            Colour expected = new Colour(0, 0, 0, 0);
            Colour actual = Colour.Multiply(colour, -100);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProduceOperator_CorrectColourReturned()
        {
            Colour colour = new Colour(8, 8, 8, 8);
            Colour expected = new Colour(16, 16, 16, 16);
            Colour actual = colour * 2;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProduceOperator_MultiplyWithHugeFactor_MaxColourReturned()
        {
            Colour colour = new Colour(8, 8, 8, 8);
            Colour expected = new Colour(255, 255, 255, 255);
            Colour actual = colour * 100;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProduceOperator_MultiplyWithNegativeFactor_MinColourReturned()
        {
            Colour colour = new Colour(8, 8, 8, 8);
            Colour expected = new Colour(0, 0, 0, 0);
            Colour actual = colour * -1;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_HexStringReturned()
        {
            Colour colour = new Colour(255, 0, 0);
            string expected = "#FF0000";
            string actual = colour.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Equals_CalledWithColourWithSameRGB_ReturnsTrue()
        {
            Colour colour1 = new Colour(1, 2, 3);
            Colour colour2 = new Colour(1, 2, 3);

            Assert.AreEqual(colour1, colour2);
        }

        [Test]
        public void Equals_CalledWithColourWithDifferentRGB_ReturnsFalse()
        {
            Colour colour1 = new Colour(1, 2, 3);
            Colour colour2 = new Colour(1, 2, 4);

            Assert.AreNotEqual(colour1, colour2);
        }

        [Test]
        public void Equals_CalledWithColourWithSameRGBA_ReturnsTrue()
        {
            Colour colour1 = new Colour(1, 2, 3, 4);
            Colour colour2 = new Colour(1, 2, 3, 4);

            Assert.AreEqual(colour1, colour2);
        }

        [Test]
        public void Equals_CalledWithColourWithDifferentRGBA_ReturnsFalse()
        {
            Colour colour1 = new Colour(1, 2, 3, 4);
            Colour colour2 = new Colour(1, 2, 3, 5);

            Assert.AreNotEqual(colour1, colour2);
        }

        [Test]
        public void Equals_CalledWithSameRGB_ReturnsTrue()
        {
            Colour colour1 = new Colour(1, 2, 3);
            Colour colour2 = new Colour(1, 2, 3);

            Assert.AreEqual(colour1, colour2);
        }

        [Test]
        public void Equals_CalledWithDifferentRGB_ReturnsFalse()
        {
            Colour colour1 = new Colour(1, 2, 3);
            Colour colour2 = new Colour(1, 2, 4);

            Assert.AreNotEqual(colour1, colour2);
        }

        [Test]
        public void Equals_CalledWithSameRGBA_ReturnsTrue()
        {
            Colour colour1 = new Colour(1, 2, 3, 4);
            Colour colour2 = new Colour(1, 2, 3, 4);

            Assert.AreEqual(colour1, colour2);
        }

        [Test]
        public void Equals_CalledWithDifferentRGBA_ReturnsFalse()
        {
            Colour colour1 = new Colour(1, 2, 3, 4);
            Colour colour2 = new Colour(1, 2, 3, 5);

            Assert.AreNotEqual(colour1, colour2);
        }

        [Test]
        public void Equals_CalledWithValidHexString_ReturnsTrue()
        {
            Colour colour = new Colour(255, 0, 255);

            Assert.IsTrue(colour.Equals("#FF00FF"));
        }

        [Test]
        public void Equals_CalledWithUnrelatedObject_Returnsfalse()
        {
            Colour colour = new Colour(255, 0, 255);

            Assert.IsFalse(colour.Equals(DateTime.Now));
        }
    }
}