using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using NuciXNA.Primitives;

namespace NuciXNA.DataAccess.IO
{
    /// <summary>
    /// Fast bitmap manipulator.
    /// </summary>
    public class BitmapFile : IDisposable
    {
        readonly Bitmap sourceBitmap;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData;
        bool bitsLocked;

        /// <summary>
        /// Gets or sets the pixels.
        /// </summary>
        /// <value>The pixels.</value>
        public byte[] Pixels { get; set; }

        /// <summary>
        /// Gets the depth.
        /// </summary>
        /// <value>The depth.</value>
        public int Depth { get; private set; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public Size2D Size { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapFile"/> class.
        /// </summary>
        /// <param name="sourceBitmap">Source bitmap.</param>
        public BitmapFile(Bitmap sourceBitmap)
        {
            this.sourceBitmap = sourceBitmap;
            LockBits();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapFile"/> class.
        /// </summary>
        /// <param name="sourceImage">Source image.</param>
        public BitmapFile(Image sourceImage)
        {
            sourceBitmap = (Bitmap)sourceImage;
            LockBits();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapFile"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public BitmapFile(string fileName)
        {
            sourceBitmap = new Bitmap(fileName);
            LockBits();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapFile"/> class.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="resource">Resource.</param>
        public BitmapFile(Type type, string resource)
        {
            sourceBitmap = new Bitmap(type, resource);
            LockBits();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapFile"/> class.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public BitmapFile(int width, int height)
        {
            sourceBitmap = new Bitmap(width, height);
            LockBits();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapFile"/> class.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="format">Format.</param>
        public BitmapFile(int width, int height, PixelFormat format)
        {
            sourceBitmap = new Bitmap(width, height, format);
            LockBits();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapFile"/> class.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="stride">Stride.</param>
        /// <param name="format">Format.</param>
        /// <param name="scan0">Scan0.</param>
        public BitmapFile(int width, int height, int stride, PixelFormat format, IntPtr scan0)
        {
            sourceBitmap = new Bitmap(width, height, stride, format, scan0);
            LockBits();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapFile"/> class.
        /// </summary>
        /// <param name="size">Size.</param>
        public BitmapFile(Size2D size)
            : this(size.Width, size.Height) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapFile"/> class.
        /// </summary>
        /// <param name="size">Size.</param>
        /// <param name="format">Format.</param>
        public BitmapFile(Size2D size, PixelFormat format)
            : this(size.Width, size.Height, format) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapFile"/> class.
        /// </summary>
        /// <param name="size">Size.</param>
        /// <param name="stride">Stride.</param>
        /// <param name="format">Format.</param>
        /// <param name="scan0">Scan0.</param>
        public BitmapFile(Size2D size, int stride, PixelFormat format, IntPtr scan0)
            : this(size.Width, size.Height, stride, format, scan0) { }

        /// <summary>
        /// Locks the bitmap data
        /// </summary>
        public void LockBits()
        {
            if (bitsLocked)
            {
                return;
            }

            bitsLocked = true;

            Size = new Size2D(sourceBitmap.Width, sourceBitmap.Height);
            
            Rectangle rect = new Rectangle(0, 0, Size.Width, Size.Height);

            Depth = Image.GetPixelFormatSize(sourceBitmap.PixelFormat);

            if (Depth != 8 && Depth != 24 && Depth != 32)
            {
                throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
            }

            bitmapData = sourceBitmap.LockBits(rect, ImageLockMode.ReadWrite, sourceBitmap.PixelFormat);

            int step = Depth / 8;
            Pixels = new byte[(int)Size.Area * step];
            Iptr = bitmapData.Scan0;

            Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
        }

        /// <summary>
        /// Unlocks the bitmap data
        /// </summary>
        public void UnlockBits()
        {
            if (!bitsLocked)
            {
                return;
            }

            Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);
            sourceBitmap.UnlockBits(bitmapData);
        }

        /// <summary>
        /// Gets the colour of the specified pixel.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <returns>Pixel colou.r</returns>
        public Colour GetPixel(int x, int y)
        {
            Colour colour = new Colour();
            byte a, r, g, b;
            int colorComponentsCount = Depth / 8;
            int index = ((y * Size.Width) + x) * colorComponentsCount;

            if (index > Pixels.Length - colorComponentsCount)
            {
                throw new IndexOutOfRangeException();
            }

            switch (Depth)
            {
                case 32:
                    b = Pixels[index];
                    g = Pixels[index + 1];
                    r = Pixels[index + 2];
                    a = Pixels[index + 3];
                    colour = Colour.FromArgb(a, r, g, b);
                    break;

                case 24:
                    b = Pixels[index];
                    g = Pixels[index + 1];
                    r = Pixels[index + 2];
                    colour = Colour.FromArgb(r, g, b);
                    break;

                case 8:
                    b = Pixels[index];
                    colour = Colour.FromArgb(b, b, b);
                    break;
            }

            return colour;
        }

        /// <summary>
        /// Gets the colour of the specified pixel.
        /// </summary>
        /// <param name="location">The location of the pixel.</param>
        /// <returns>Pixel colour.</returns>
        public Colour GetPixel(Point2D location)
        {
            return GetPixel(location.X, location.Y);
        }

        /// <summary>
        /// Sets the colour of the specified pixel.
        /// </summary>
        /// <param name="x">The X coordinate of the pixel.</param>
        /// <param name="y">The Y coordinate of the pixel.</param>
        /// <param name="colour">Pixel colour.</param>
        public void SetPixel(int x, int y, Colour colour)
        {
            int colorComponentsCount = Depth / 8;
            int index = ((y * Size.Width) + x) * colorComponentsCount;

            switch (Depth)
            {
                case 32:
                    Pixels[index] = colour.B;
                    Pixels[index + 1] = colour.G;
                    Pixels[index + 2] = colour.R;
                    Pixels[index + 3] = colour.A;
                    break;

                case 24:
                    Pixels[index] = colour.B;
                    Pixels[index + 1] = colour.G;
                    Pixels[index + 2] = colour.R;
                    break;

                case 8:
                    Pixels[index] = colour.B;
                    break;
            }
        }

        /// <summary>
        /// Sets the colour of the specified pixel.
        /// </summary>
        /// <param name="location">The location of the pixel.</param>
        /// <param name="colour">Pixel colour.</param>
        public void SetPixel(Point2D location, Colour colour)
        {
            SetPixel(location.X, location.Y, colour);
        }

        /// <summary>
        /// Releases all resource used by the <see cref="BitmapFile"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="BitmapFile"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="BitmapFile"/> in an unusable state. After
        /// calling <see cref="Dispose"/>, you must release all references to the <see cref="BitmapFile"/>
        /// so the garbage collector can reclaim the memory that the <see cref="BitmapFile"/> was occupying.</remarks>
        public void Dispose()
        {
            UnlockBits();
            sourceBitmap.Dispose();
        }

        /// <param name="source">Source <see cref="BitmapFile"/>.</param>
        public static implicit operator Bitmap(BitmapFile source)
        {
            return source.sourceBitmap;
        }

        /// <param name="source">Source <see cref="Bitmap"/>.</param>
        public static implicit operator BitmapFile(Bitmap source)
        {
            return new BitmapFile(source);
        }

        /// <param name="source"><see cref="BitmapFile"/>.</param>
        public static implicit operator Image(BitmapFile source)
        {
            return source.sourceBitmap;
        }

        /// <param name="source">Source <see cref="Image"/>.</param>
        public static implicit operator BitmapFile(Image source)
        {
            return new BitmapFile(source);
        }
    }
}
