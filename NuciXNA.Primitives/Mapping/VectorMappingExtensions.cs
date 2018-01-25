using Microsoft.Xna.Framework;

namespace NuciXNA.Primitives.Mapping
{
    public static class VectorMappingExtensions
    {
        // >>> TO NUCI PRIMITIVE

        /// <summary>
        /// Converts a <see cref="Vector2"/> into to a <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Vector2"/>.</param>
        /// <returns>The <see cref="Vector2D"/>.</returns>
        public static Vector2D ToVector2D(this Vector2 source)
        => new Vector2D(source.X, source.Y);

        /// <summary>
        /// Converts a <see cref="Vector3"/> into to a <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Vector3"/>.</param>
        /// <returns>The <see cref="Vector3D"/>.</returns>
        public static Vector3D ToVector3D(this Vector3 source)
        => new Vector3D(source.X, source.Y, source.Z);

        // >>> TO XNA PRIMITIVE

        /// <summary>
        /// Converts a <see cref="Vector2D"/> into to a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Vector2D"/>.</param>
        /// <returns>The <see cref="Vector2"/>.</returns>
        public static Vector2 ToXnaVector(this Vector2D source)
        => new Vector2(source.X, source.Y);

        /// <summary>
        /// Converts a <see cref="Vector3D"/> into to a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Vector3D"/>.</param>
        /// <returns>The <see cref="Vector3"/>.</returns>
        public static Vector3 ToXnaVector(this Vector3D source)
        => new Vector3(source.X, source.Y, source.Z);
    }
}
