using System;

namespace NuciXNA.Primitives
{
    /// <summary>
    /// 2D orthogonal coordinates structure.
    /// </summary>
    public struct PointF3D : IEquatable<PointF3D>
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
        /// Gets a value indicating whether the coordinates of this <see cref="PointF3D"/> are zero.
        /// </summary>
        /// <value><c>true</c> if the coorinates are zero; otherwise, <c>false</c>.</value>
        public bool IsEmpty => X == 0 && Y == 0;

        /// <summary>
        /// Gets a <see cref="PointF3D"/> with the coordinates of zero.
        /// </summary>
        /// <value>The orthogonal centre point.</value>
        public static PointF3D Empty => new PointF3D(0, 0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="PointF3D"/> structure.
        /// </summary>
        /// <param name="x">The X-axis coordinate.</param>
        /// <param name="y">The Y-axis coordinate.</param>
        /// <param name="z">The Z-axis coordinate.</param>
        public PointF3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public PointF3D(Point3D point3d) : this(point3d.X, point3d.Y, point3d.Z) { }
        public PointF3D(PointF2D point2d, int z) : this(point2d.X, point2d.Y, z) { }
        public PointF3D(PointF2D point2d, float z) : this(point2d.X, point2d.Y, z) { }
        public PointF3D(Point2D point2d, int z) : this(point2d.X, point2d.Y, z) { }
        public PointF3D(Point2D point2d, float z)  : this(point2d.X, point2d.Y, z) { }

        /// <summary>
        /// Determines whether the specified <see cref="PointF3D"/> is equal to the current <see cref="PointF3D"/>.
        /// </summary>
        /// <param name="other">The <see cref="PointF3D"/> to compare with the current <see cref="PointF3D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="PointF3D"/> is equal to the current <see cref="PointF3D"/>;
        /// otherwise, <c>false</c>.</returns>
        public bool Equals(PointF3D other)
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
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="PointF3D"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="PointF3D"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="PointF3D"/>; otherwise, <c>false</c>.</returns>
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

            return Equals((PointF3D)obj);
        }

        /// <summary>
        /// Adds the coordinates of a <see cref="PointF3D"/> to those of another <see cref="PointF3D"/>,
        /// yielding a new <see cref="PointF3D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="PointF3D"/> to add.</param>
        /// <param name="other">The second <see cref="PointF3D"/> to add.</param>
        /// <returns>The <see cref="PointF3D"/> whose coordinates are the sum of the coordinates of <c>source</c> and <c>other</c>.</returns>
        public static PointF3D operator +(PointF3D source, PointF3D other)
        => new PointF3D(source.X + other.X,
                       source.Y + other.Y,
                       source.Z + other.Z);

        /// <summary>
        /// Subtracts the coordinates of a <see cref="PointF3D"/> from those of another <see cref="PointF3D"/>,
        /// yielding a new <see cref="PointF3D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="PointF3D"/> to subtract.</param>
        /// <param name="other">The second <see cref="PointF3D"/> to subtract.</param>
        /// <returns>The <see cref="PointF3D"/> whose coordinates are the sum of the coordinates of <c>source</c> and <c>other</c>.</returns>
        public static PointF3D operator -(PointF3D source, PointF3D other)
        => new PointF3D(source.X - other.X,
                       source.Y - other.Y,
                       source.Z - other.Z);

        /// <summary>
        /// Multiples the values of a <see cref="PointF3D"/> from those of another <see cref="PointF3D"/>,
        /// yielding a new <see cref="PointF3D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="PointF3D"/> to multiply.</param>
        /// <param name="other">The second <see cref="PointF3D"/> to multiply.</param>
        /// <returns>The <see cref="PointF3D"/> whose values are the produce of the values of <c>source</c> and <c>other</c>.</returns>
        public static PointF3D operator *(PointF3D source, PointF3D other)
        => new PointF3D(source.X * other.X,
                        source.Y * other.Y,
                        source.Z * other.Z);

        /// <summary>
        /// Divides the values of a <see cref="PointF3D"/> from those of another <see cref="PointF3D"/>,
        /// yielding a new <see cref="PointF3D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="PointF3D"/> to divide.</param>
        /// <param name="other">The second <see cref="PointF3D"/> to divide.</param>
        /// <returns>The <see cref="PointF3D"/> whose values are the division of the values of <c>source</c> and <c>other</c>.</returns>
        public static PointF3D operator /(PointF3D source, PointF3D other)
        => new PointF3D(source.X / other.X,
                        source.Y / other.Y,
                        source.Z / other.Z);

        public static PointF3D operator *(PointF3D source, float other)
        => new PointF3D(source.X * other, source.Y * other, source.Y * other);

        public static PointF3D operator /(PointF3D source, float other)
        => new PointF3D(source.X / other, source.Y / other, source.Y * other);

        /// <summary>
        /// Determines whether a specified instance of <see cref="PointF3D"/> is equal to another specified <see cref="PointF3D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="PointF3D"/> to compare.</param>
        /// <param name="other">The second <see cref="PointF3D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(PointF3D source, PointF3D other)
        => source.Equals(other);

        /// <summary>
        /// Determines whether a specified instance of <see cref="PointF3D"/> is not equal to
        /// another specified <see cref="PointF3D"/>.
        /// </summary>
        /// <param name="source">The first <see cref="PointF3D"/> to compare.</param>
        /// <param name="other">The second <see cref="PointF3D"/> to compare.</param>
        /// <returns><c>true</c> if <c>source</c> and <c>other</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(PointF3D source, PointF3D other)
        => !(source == other);

        /// <summary>
        /// Serves as a hash function for a <see cref="PointF3D"/> object.
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
    }
}
