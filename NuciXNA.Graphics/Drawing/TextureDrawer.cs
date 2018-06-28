using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

namespace NuciXNA.Graphics.Drawing
{
    public class TextureDrawer
    {
        private static readonly SpriteSortMode DefaultSpriteSortMode = SpriteSortMode.Deferred;
        private static readonly SamplerState DefaultSamplerState = SamplerState.LinearClamp;

        private static SpriteSortMode currentSpriteSortMode = DefaultSpriteSortMode;
        private static SamplerState currentSamplerState = DefaultSamplerState;
        
        public static void Draw(
            SpriteBatch spriteBatch,
            Texture2D texture,
            Point2D location,
            Rectangle2D sourceRectangle,
            Colour tint,
            float opacity,
            float rotation,
            Point2D origin,
            Scale2D scale,
            TextureLayout textureLayout)
        {
            Vector2 loc = new Vector2(location.X, location.Y);
            Rectangle srcRec = new Rectangle(sourceRectangle.X, sourceRectangle.Y, sourceRectangle.Width, sourceRectangle.Height);
            Color colour = tint.ToXnaColor();
            Vector2 org = new Vector2(origin.X, origin.Y);
            Vector2 scl = new Vector2(scale.Horizontal, scale.Vertical);
            float layerDepth = 0.0f;

            colour.A = (byte)(colour.A * opacity);

            if (textureLayout == TextureLayout.Tile)
            {
                SetSpriteBatchProperties(spriteBatch, SpriteSortMode.Immediate, SamplerState.LinearWrap);
            }
            else if (textureLayout == TextureLayout.Stretch)
            {
                SetSpriteBatchProperties(spriteBatch, DefaultSpriteSortMode, DefaultSamplerState);

                loc = new Vector2(
                    location.X + (int)(sourceRectangle.Width * scale.Horizontal) / 2,
                    location.Y + (int)(sourceRectangle.Height * scale.Vertical) / 2);
            }

            spriteBatch.Draw(texture, loc, srcRec, colour, rotation, org, scl, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, layerDepth);
        }

        static void SetSpriteBatchProperties(
            SpriteBatch spriteBatch,
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

            // TODO: Is it ok to End and Begin again? Does it affect performance? It most probably does.
            if (beginBatchAgain)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, null, samplerState);
            }
        }
    }
}
