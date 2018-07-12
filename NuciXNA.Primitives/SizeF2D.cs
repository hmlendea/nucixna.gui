using System;
using System.Drawing;

namespace NuciXNA.Primitives
{
    /// <summary>
    /// 2D size structure.
    /// </summary>
    public struct SizeF2D : IEquatable<SizeF2D>
    {
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public float Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public float Height { get; set; }

        /// <summary>
        /// Gets the area.
        /// </summary>
        /// <value>The area.</value>
        public double Area => Width * Height;

        public double Perimeter => Width * 2 + Height * 2;

        /// <summary>
        /// Gets a value indicating whether this <see cref="SizeF2D"/> is zero.
        /// </summary>
        /// <value><c>true</c> if is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty => Width == 0 && Height == 0;

        /// <summary>
        /// Gets the size of zero.
        /// </summary>
        /// <value>The size of zero.</value>
        public static SizeF2D Empty => new SizeF2D(0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeF2D"/> structure.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public SizeF2D(float width, float height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeF2D"/> structure.
        /// </summary>
        /// <param name="point">Point.</param>
        public SizeF2D(PointF2D point)
        {
            Width = point.X;
            Height = point.Y;
        }

        public SizeF2D(float size) : this(size, size) { }
        public SizeF2D(int size) : this(size, size) { }

        /// <summary>
        /// Determines whether the specified <see cref="SizeF2D"/> is equal to the current <see cref="SizeF2D"/>.
        /// </summary>
        /// <param name="other">The <see cref="SizeF2D"/> to compare with the current <see cref="SizeF2D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="SizeF2D"/> is equal to the current <see cref="SizeF2D"/>;
        /// otherwise, <c>false</c>.</returns>
        public bool Equals(SizeF2D other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Width, other.Width) &&
                   Equals(Height, other.Height);
        }

        public bool Equals(float width, float height)
        {
            return Width == width && Height == height;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="SizeF2D"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="SizeF2D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current <see cref="SizeF2D"/>;
        /// otherwise, <c>false</c>.</returns>
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

            return Equals((SizeF2D)obj);
        }

        /// <summary>
        /// Adds the values of a <see cref="SizeF2D"/> to those of another <see cref="SizeF2D"/>,
        /// yielding a new <see cref="SizeF2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="SizeF2D"/> to add.</param>
        /// <param name="other">The second <see cref="SizeF2D"/> to add.</param>
        /// <returns>The <see cref="SizeF2D"/> that is the sum of the values of <c>source</c> and <c>other</c>.</returns>
        public static SizeF2D operator +(SizeF2D source, SizeF2D other)
        => new SizeF2D(source.Width + other.Width,
                      source.Height + other.Height);

        /// <summary>
        /// Subtracts the values of a <see cref="SizeF2D"/> from those of another <see cref="SizeF2D"/>,
        /// yielding a new <see cref="SizeF2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="SizeF2D"/> to subtract.</param>
        /// <param name="other">The second <see cref="SizeF2D"/> to subtract.</param>
        /// <returns>The <see cref="SizeF2D"/> that is the subtraction of the values of <c>other</c> from <c>source</c>.</returns>
        public static SizeF2D operator -(SizeF2D source, SizeF2D other)
        => new SizeF2D(source.Width - other.Width,
                      source.Height - other.Height);

        public static SizeF2D operator *(SizeF2D source, SizeF2D other)
        => new SizeF2D(source.Width * other.Width,
                      source.Height * other.Height);

        public static SizeF2D operator /(SizeF2D source, SizeF2D other)
        => new SizeF2D(source.Width / other.Width,
                      source.Height / other.Height);

        public static SizeF2D operator *(SizeF2D source, float other)
        => new SizeF2D(source.Width * other,
                      source.Height * other);

        public static SizeF2D operator /(SizeF2D source, float other)
        => new SizeF2D(source.Width / other,
                      source.Height / other);

        /// <summary>
        /// Determines whether a specified instance of <see cref="SizeF2D"/> is equal to
        /// another specified <see cref="SizeF2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="SizeF2D"/> to compare.</param>
        /// <param name="other">The second <see cref="SizeF2D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(SizeF2D source, SizeF2D other)
        => source.Equals(other);

        /// <summary>
        /// Determines whether a specified instance of <see cref="SizeF2D"/> is not equal to
        /// another specified <see cref="SizeF2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="SizeF2D"/> to compare.</param>
        /// <param name="other">The second <see cref="SizeF2D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(SizeF2D source, SizeF2D other)
        => !(source == other);

        /// <summary>
        /// Serves as a hash function for a <see cref="SizeF2D"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Width.GetHashCode() * 397) ^
                        Height.GetHashCode();
            }
        }

        public static implicit operator SizeF(SizeF2D source)
        {
            return new SizeF(source.Width, source.Height);
        }

        public static implicit operator SizeF2D(SizeF source)
        {
            return new SizeF2D(source.Width, source.Height);
        }
    }
}
