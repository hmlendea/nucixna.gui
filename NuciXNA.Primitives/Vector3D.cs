using System;

using Microsoft.Xna.Framework;

namespace NuciXNA.Primitives
{
    /// <summary>
    /// 3D vector structure.
    /// </summary>
    public struct Vector3D : IEquatable<Vector3D>
    {
        /// <summary>
        /// Gets or sets the X-axis coordinate.
        /// </summary>
        /// <value>The X-axis coordinate.</value>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the Y-axis coordinate.
        /// </summary>
        /// <value>The Y-axis coordinate.</value>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the Z-axis coordinate.
        /// </summary>
        /// <value>The Z-axis coordinate.</value>
        public float Z { get; set; }

        /// <summary>
        /// Gets a value indicating whether the coordinates of this <see cref="Vector3D"/> are zero.
        /// </summary>
        /// <value><c>true</c> if the values are zero; otherwise, <c>false</c>.</value>
        public bool IsEmpty => X == 0 && Y == 0;

        public static Vector3D Zero => new Vector3D(0, 0, 0);
        public static Vector3D One => new Vector3D(1, 1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3D"/> structure.
        /// </summary>
        /// <param name="x">The X-axis coordinate.</param>
        /// <param name="y">The Y-axis coordinate.</param>
        /// <param name="z">The Z-axis coordinate.</param>
        public Vector3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Vector3D"/> is equal to the current <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="other">The <see cref="Vector3D"/> to compare with the current <see cref="Vector3D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Vector3D"/> is equal to the current <see cref="Vector3D"/>;
        /// otherwise, <c>false</c>.</returns>
        public bool Equals(Vector3D other)
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
                   Equals(Y, other.Y) &&
                   Equals(Z, other.Z);
        }

        public bool Equals(float x, float y, float z)
        {
            return X == x && Y == y && Z == z;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Vector3D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Vector3D"/>; otherwise, <c>false</c>.</returns>
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

            return Equals((Vector3D)obj);
        }

        /// <summary>
        /// Adds the values of a <see cref="Vector3D"/> to those of another <see cref="Vector3D"/>,
        /// yielding a new <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Vector3D"/> to add.</param>
        /// <param name="other">The second <see cref="Vector3D"/> to add.</param>
        /// <returns>The <see cref="Vector3D"/> whose values are the sum of the values of <c>source</c> and <c>other</c>.</returns>
        public static Vector3D operator +(Vector3D source, Vector3D other)
        => new Vector3D(source.X + other.X,
                       source.Y + other.Y,
                       source.Z + other.Z);

        /// <summary>
        /// Subtracts the values of a <see cref="Vector3D"/> from those of another <see cref="Vector3D"/>,
        /// yielding a new <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Vector3D"/> to subtract.</param>
        /// <param name="other">The second <see cref="Vector3D"/> to subtract.</param>
        /// <returns>The <see cref="Vector3D"/> whose values are the sum of the values of <c>source</c> and <c>other</c>.</returns>
        public static Vector3D operator -(Vector3D source, Vector3D other)
        => new Vector3D(source.X - other.X,
                       source.Y - other.Y,
                       source.Z - other.Z);

        /// <summary>
        /// Multiples the values of a <see cref="Vector3D"/> from those of another <see cref="Vector3D"/>,
        /// yielding a new <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Vector3D"/> to multiply.</param>
        /// <param name="other">The second <see cref="Vector3D"/> to multiply.</param>
        /// <returns>The <see cref="Vector3D"/> whose values are the produce of the values of <c>source</c> and <c>other</c>.</returns>
        public static Vector3D operator *(Vector3D source, Vector3D other)
        => new Vector3D(source.X * other.X,
                        source.Y * other.Y,
                        source.Z * other.Z);

        /// <summary>
        /// Divides the values of a <see cref="Vector3D"/> from those of another <see cref="Vector3D"/>,
        /// yielding a new <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Vector3D"/> to divide.</param>
        /// <param name="other">The second <see cref="Vector3D"/> to divide.</param>
        /// <returns>The <see cref="Vector3D"/> whose values are the division of the values of <c>source</c> and <c>other</c>.</returns>
        public static Vector3D operator /(Vector3D source, Vector3D other)
        => new Vector3D(source.X / other.X,
                        source.Y / other.Y,
                        source.Z / other.Z);

        /// <summary>
        /// Determines whether a specified instance of <see cref="Vector3D"/> is equal to another specified <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Vector3D"/> to compare.</param>
        /// <param name="other">The second <see cref="Vector3D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector3D source, Vector3D other)
        => source.Equals(other);

        /// <summary>
        /// Determines whether a specified instance of <see cref="Vector3D"/> is not equal to
        /// another specified <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="Vector3D"/> to compare.</param>
        /// <param name="other">The second <see cref="Vector3D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector3D source, Vector3D other)
        => !(source == other);

        /// <summary>
        /// Serves as a hash function for a <see cref="Vector3D"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^
                        Y.GetHashCode() ^
                        Z.GetHashCode();
            }
        }

        public static implicit operator Vector3(Vector3D source)
        {
            return new Vector3(source.X, source.Y, source.Z);
        }

        public static implicit operator Vector3D(Vector3 source)
        {
            return new Vector3D(source.X, source.Y, source.Z);
        }
    }
}
