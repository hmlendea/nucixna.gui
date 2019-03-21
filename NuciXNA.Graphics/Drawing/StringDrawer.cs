using System;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

namespace NuciXNA.Graphics.Drawing
{
    public static class StringDrawer
    {
        public static void Draw(
            SpriteBatch spriteBatch,
            SpriteFont font,
            string text,
            Rectangle2D bounds,
            Colour fontColour,
            Colour outlineColour,
            float opacity,
            Alignment horizontalAlignment,
            Alignment verticalAlignment,
            FontOutline outline)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            Vector2 textOrigin = Vector2.Zero;
            Vector2 totalSize = font.MeasureString(text);
            Color clrOutline = outlineColour.ToXnaColor();
            
            string[] lines = text.Split('\n');

            if (verticalAlignment == Alignment.Middle)
            {
                textOrigin.Y = bounds.Height / 2 - totalSize.Y / 2;
            }
            else if (verticalAlignment == Alignment.End)
            {
                textOrigin.Y = bounds.Height - totalSize.Y;
            }

            foreach (string line in lines)
            {
                Vector2 lineSize = font.MeasureString(line);

                if (horizontalAlignment == Alignment.Middle)
                {
                    textOrigin.X = bounds.Width / 2 - lineSize.X / 2;
                }
                else if (horizontalAlignment == Alignment.End)
                {
                    textOrigin.X = bounds.Width - lineSize.X;
                }

                textOrigin = new Vector2(
                    (int)Math.Round(textOrigin.X),
                    (int)Math.Round(textOrigin.Y));

                if (outline == FontOutline.Around)
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            Vector2 pos = new Vector2(
                                bounds.X + dx + textOrigin.X,
                                bounds.Y + dy + textOrigin.Y);
                            
                            spriteBatch.DrawString(font, line, pos, clrOutline);
                        }
                    }
                }
                else if (outline == FontOutline.BottomRight)
                {
                    Vector2 pos = new Vector2(
                        bounds.X + 1 + textOrigin.X,
                        bounds.Y + 1 + textOrigin.Y);
                    
                    spriteBatch.DrawString(font, line, pos, clrOutline);
                }

                spriteBatch.DrawString(font, line, bounds.Location.ToXnaVector2() + textOrigin, fontColour.ToXnaColor());

                textOrigin.Y += lineSize.Y;
            }
        }
        
        /// <summary>
        /// Wraps the text on multiple lines.
        /// </summary>
        /// <returns>The text.</returns>
        /// <param name="font">Font.</param>
        /// <param name="text">Text.</param>
        /// <param name="maxLineWidth">Maximum line width.</param>
        static string WrapText(SpriteFont font, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = font.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = font.MeasureString(word);

                if (word.Contains("\r"))
                {
                    lineWidth = 0f;
                    sb.Append("\r \r");
                }

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }

                else
                {
                    if (size.X > maxLineWidth)
                    {
                        if (sb.ToString() == " ")
                        {
                            sb.Append(WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth));
                        }
                        else
                        {
                            sb.Append("\n" + WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth));
                        }
                    }
                    else
                    {
                        sb.Append("\n" + word + " ");
                        lineWidth = size.X + spaceWidth;
                    }
                }
            }

            return sb.ToString();
        }
    }
}
