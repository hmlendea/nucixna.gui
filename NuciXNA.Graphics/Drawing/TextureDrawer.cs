using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

namespace NuciXNA.Graphics.Drawing
{
    public class TextureDrawer
    {
        private static SpriteSortMode currentSpriteSortMode = SpriteSortMode.Deferred;
        private static SamplerState currentSamplerState = SamplerState.LinearClamp;
        
        public static void Draw(
            SpriteBatch spriteBatch,
            Texture2D texture,
            Point2D location,
            Rectangle2D rectangle)
        {
            Draw(
                spriteBatch,
                texture,
                location,
                rectangle,
                Colour.White);
        }

        public static void Draw(
            SpriteBatch spriteBatch,
            Texture2D texture,
            Point2D location,
            Rectangle2D rectangle,
            Colour tint)
        {
            Draw(
                spriteBatch,
                texture,
                location,
                rectangle,
                tint,
                currentSpriteSortMode,
                currentSamplerState);
        }

        public static void Draw(
            SpriteBatch spriteBatch,
            Texture2D texture,
            Point2D location,
            Rectangle2D rectangle,
            Colour tint,
            SpriteSortMode spriteSortMode,
            SamplerState samplerState)
        {
            Draw(
                spriteBatch,
                texture,
                location,
                rectangle,
                tint,
                0.0f,
                Point2D.Empty,
                Scale2D.One,
                spriteSortMode,
                samplerState);
        }

        public static void Draw(
            SpriteBatch spriteBatch,
            Texture2D texture,
            Point2D location,
            Rectangle2D rectangle,
            Colour tint,
            float rotation,
            Point2D origin,
            Scale2D scale)
        {
            Draw(
                spriteBatch,
                texture,
                location,
                rectangle,
                tint,
                rotation,
                origin,
                scale,
                SpriteSortMode.Deferred,
                SamplerState.LinearClamp);
        }

        public static void Draw(
            SpriteBatch spriteBatch,
            Texture2D texture,
            Point2D location,
            Rectangle2D rectangle,
            Colour tint,
            float rotation,
            Point2D origin,
            Scale2D scale,
            SpriteSortMode spriteSortMode,
            SamplerState samplerState)
        {
            bool beginBatchAgain = false;

            if (spriteSortMode != currentSpriteSortMode)
            {
                currentSpriteSortMode = spriteSortMode;
                beginBatchAgain = true;
            }

            if (samplerState != currentSamplerState)
            {
                currentSamplerState = samplerState;
                beginBatchAgain = true;
            }

            if (beginBatchAgain)
            {
                // TODO: Is it ok to End and Begin again? Does it affect performance? It most probably does.
                spriteBatch.End();
                spriteBatch.Begin(spriteSortMode, null, samplerState, null, null);
            }

            spriteBatch.Draw(
                texture,
                new Vector2(location.X, location.Y),
                rectangle.ToXnaRectangle(),
                tint.ToXnaColor(),
                rotation,
                new Vector2(origin.X, origin.Y),
                scale.ToXnaVector2(),
                Microsoft.Xna.Framework.Graphics.SpriteEffects.None,
                0.0f);
        }
    }
}
