using System;

using Microsoft.Xna.Framework;

namespace NuciXNA.Primitives
{
    /// <summary>
    /// 2D vector structure.
    /// </summary>
    public struct Vector2D : IEquatable<Vector2D>
    {
        /// <summary>
        /// Gets or sets the X-axis value.
        /// </summary>
        /// <value>The X-axis value.</value>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the Y-axis value.
        /// </summary>
        /// <value>The Y-axis value.</value>
        public float Y { get; set; }

        /// <summary>
        /// Gets a value indicating whether the values of this <see cref="Vector2D"/> are zero.
        /// </summary>
        /// <value><c>true</c> if the coorinates are zero; otherwise, <c>false</c>.</value>
        public bool IsEmpty => X == 0 && Y == 0;

        public static Vector2D Zero => new Vector2D(0, 0);
        public static Vector2D One => new Vector2D(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2D"/> structure.
        /// </summary>
        /// <param name="x">The X-axis value.</param>
        /// <param name="y">The Y-axis value.</param>
        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2D"/> structure.
        /// </summary>
        /// <param name="size">Size.</param>
        public Vector2D(Point2D point)
        {
            X = point.X;
            Y = point.Y;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Vector2D"/> is equal to the current <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="other">The <see cref="Vector2D"/> to compare with the current <see cref="Vector2D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Vector2D"/> is equal to the current <see cref="Vector2D"/>;
        /// otherwise, <c>false</c>.</returns>
        public bool Equals(Vector2D other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(X, other.X) &&
                   Equals(Y, other.Y);
        }

        public bool Equals(float x, float y)
        {
            return X == x && Y == y;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Vector2D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Vector2D"/>; otherwise, <c>false</c>.</returns>
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

            return Equals((Vector2D)obj);
        }

        /// <summary>
        /// Adds the values of a <see cref="Vector2D"/> to those of another <see cref="Vector2D"/>,
        /// yielding a new <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Vector2D"/> to add.</param>
        /// <param name="other">The second <see cref="Vector2D"/> to add.</param>
        /// <returns>The <see cref="Vector2D"/> whose values are the sum of the values of <c>source</c> and <c>other</c>.</returns>
        public static Vector2D operator +(Vector2D source, Vector2D other)
        => new Vector2D(source.X + other.X,
                        source.Y + other.Y);

        /// <summary>
        /// Subtracts the values of a <see cref="Vector2D"/> from those of another <see cref="Vector2D"/>,
        /// yielding a new <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Vector2D"/> to subtract.</param>
        /// <param name="other">The second <see cref="Vector2D"/> to subtract.</param>
        /// <returns>The <see cref="Vector2D"/> whose values are the subtraction of the values of <c>source</c> and <c>other</c>.</returns>
        public static Vector2D operator -(Vector2D source, Vector2D other)
        => new Vector2D(source.X - other.X,
                        source.Y - other.Y);

        /// <summary>
        /// Multiples the values of a <see cref="Vector2D"/> from those of another <see cref="Vector2D"/>,
        /// yielding a new <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Vector2D"/> to multiply.</param>
        /// <param name="other">The second <see cref="Vector2D"/> to multiply.</param>
        /// <returns>The <see cref="Vector2D"/> whose values are the produce of the values of <c>source</c> and <c>other</c>.</returns>
        public static Vector2D operator *(Vector2D source, Vector2D other)
        => new Vector2D(source.X * other.X,
                        source.Y * other.Y);

        /// <summary>
        /// Divides the values of a <see cref="Vector2D"/> from those of another <see cref="Vector2D"/>,
        /// yielding a new <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Vector2D"/> to divide.</param>
        /// <param name="other">The second <see cref="Vector2D"/> to divide.</param>
        /// <returns>The <see cref="Vector2D"/> whose values are the division of the values of <c>source</c> and <c>other</c>.</returns>
        public static Vector2D operator /(Vector2D source, Vector2D other)
        => new Vector2D(source.X / other.X,
                        source.Y / other.Y);

        /// <summary>
        /// Determines whether a specified instance of <see cref="Vector2D"/> is equal to another specified <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Vector2D"/> to compare.</param>
        /// <param name="other">The second <see cref="Vector2D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector2D source, Vector2D other)
        => source.Equals(other);

        /// <summary>
        /// Determines whether a specified instance of <see cref="Vector2D"/> is not equal to
        /// another specified <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Vector2D"/> to compare.</param>
        /// <param name="other">The second <see cref="Vector2D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector2D source, Vector2D other)
        => !(source == other);

        /// <summary>
        /// Serves as a hash function for a <see cref="Vector2D"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^
                        Y.GetHashCode();
            }
        }

        public static implicit operator Vector2(Vector2D source)
        {
            return new Vector2(source.X, source.Y);
        }

        public static implicit operator Vector2D(Vector2 source)
        {
            return new Vector2D(source.X, source.Y);
        }
    }
}
