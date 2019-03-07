using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;

namespace NuciXNA.DataAccess.Extensions
{
    /// <summary>
    /// <see cref="ContentManager"> extensions.
    /// </summary>
    internal static class ContentManagerExtensions
    {
        public static TContent TryLoad<TContent>(this ContentManager contentManager, string contentPath) where TContent : class
        {
            try
            {
                return contentManager.Load<TContent>(contentPath);
            }
            catch
            {
                return null;
            }
        }
    }
}
