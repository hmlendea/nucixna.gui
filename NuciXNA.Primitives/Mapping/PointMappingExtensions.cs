using SystemPoint = System.Drawing.Point;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using XnaPoint = Microsoft.Xna.Framework.Point;

namespace NuciXNA.Primitives.Mapping
{
    public static class PointMappingExtensions
    {
        // >>> TO NUCIXNA POINT2D

        /// <summary>
        /// Converts a <see cref="SystemPoint"/> into to a <see cref="Point2D"/>.
        /// </summary>
        /// <param name="source">Source <see cref="SystemPoint"/>.</param>
        /// <returns>The <see cref="Point2D"/>.</returns>
        public static Point2D ToPoint2D(this SystemPoint source)
        => new Point2D(source.X, source.Y);

        /// <summary>
        /// Converts a <see cref="Vector2"/> into to a <see cref="Point2D"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Vector2"/>.</param>
        /// <returns>The <see cref="Point2D"/>.</returns>
        public static Point2D ToPoint2D(this Vector2 source)
        => new Point2D((int)source.X, (int)source.Y);

        /// <summary>
        /// Converts a <see cref="XnaPoint"/> into to a <see cref="Point2D"/>.
        /// </summary>
        /// <param name="source">Source <see cref="XnaPoint"/>.</param>
        /// <returns>The <see cref="Point2D"/>.</returns>
        public static Point2D ToPoint2D(this XnaPoint source)
        => new Point2D(source.X, source.Y);

        // >>> TO SYSTEM

        /// <summary>
        /// Converts a <see cref="Point2D"/> into to a <see cref="SystemPoint"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Point2D"/>.</param>
        /// <returns>The <see cref="SystemPoint"/>.</returns>
        public static SystemPoint ToSystemPoint(this Point2D source)
        => new SystemPoint(source.X, source.Y);

        /// <summary>
        /// Converts a <see cref="Vector2"/> into to a <see cref="SystemPoint"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Vector2"/>.</param>
        /// <returns>The <see cref="SystemPoint"/>.</returns>
        public static SystemPoint ToSystemPoint(this Vector2 source)
        => new SystemPoint((int)source.X, (int)source.Y);

        /// <summary>
        /// Converts a <see cref="XnaPoint"/> into to a <see cref="SystemPoint"/>.
        /// </summary>
        /// <param name="source">Source <see cref="XnaPoint"/>.</param>
        /// <returns>The <see cref="SystemPoint"/>.</returns>
        public static SystemPoint ToSystemPoint(this XnaPoint source)
        => new SystemPoint(source.X, source.Y);

        // >>> TO XNA

        /// <summary>
        /// Converts a <see cref="SystemPoint"/> into to a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="source">Source <see cref="SystemPoint"/>.</param>
        /// <returns>The <see cref="Vector2"/>.</returns>
        public static Vector2 ToXnaVector2(this SystemPoint source)
        => new Vector2(source.X, source.Y);

        /// <summary>
        /// Converts a <see cref="Point2D"/> into to a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Point2D"/>.</param>
        /// <returns>The <see cref="Vector2"/>.</returns>
        public static Vector2 ToXnaVector2(this Point2D source)
        => new Vector2(source.X, source.Y);

        /// <summary>
        /// Converts a <see cref="Point2D"/> into to a <see cref="XnaPoint"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Point2D"/>.</param>
        /// <returns>The <see cref="XnaPoint"/>.</returns>
        public static XnaPoint ToXnaPoint(this Point2D source)
        => new XnaPoint(source.X, source.Y);

        /// <summary>
        /// Converts a <see cref="SystemPoint"/> into to a <see cref="XnaPoint"/>.
        /// </summary>
        /// <param name="source">Source <see cref="SystemPoint"/>.</param>
        /// <returns>The <see cref="XnaPoint"/>.</returns>
        public static XnaPoint ToXnaPoint(this SystemPoint source)
        => new XnaPoint(source.X, source.Y);
    }
}
