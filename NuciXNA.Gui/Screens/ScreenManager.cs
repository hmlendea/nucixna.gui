using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Graphics;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Primitives;

namespace NuciXNA.Gui.Screens
{
    /// <summary>
    /// Screen manager.
    /// </summary>
    public class ScreenManager
    {
        static volatile ScreenManager instance;
        static object syncRoot = new object();

        Screen currentScreen, newScreen;
        TextureSprite transitionSprite;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ScreenManager();
                        }
                    }
                }

                return instance;
            }
        }

        public Type StartingScreenType { get; set; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public Size2D Size { get; private set; }

        /// <summary>
        /// Gets or sets the sprite batch.
        /// </summary>
        /// <value>The sprite batch.</value>
        public SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current screen is transitioning.
        /// </summary>
        /// <value><c>true</c> if transitioning; otherwise, <c>false</c>.</value>
        public bool Transitioning { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenManager"/> class.
        /// </summary>
        public ScreenManager()
        {
            Size = GraphicsManager.Instance.BackBufferSize;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            currentScreen = (Screen)Activator.CreateInstance(StartingScreenType);

            transitionSprite = new TextureSprite
            {
                ContentFile = "ScreenManager/FillImage",
                Tint = Colour.Black,
                OpacityEffect = new FadeEffect
                {
                    Speed = 3,
                    CurrentMultiplier = 0.0f,
                    MinimumMultiplier = 0.0f,
                    MaximumMultiplier = 2.0f
                },
                TextureLayout = TextureLayout.Tile
            };

            currentScreen.LoadContent();
            transitionSprite.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            currentScreen.UnloadContent();
            transitionSprite.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);

            if (Transitioning)
            {
                Transition(gameTime);
                return;
            }

            Size = GraphicsManager.Instance.BackBufferSize;

            transitionSprite.Scale = new Scale2D(Size);
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);

            if (Transitioning)
            {
                transitionSprite.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Changes the screen.
        /// </summary>
        /// <param name="screenType">Screen type.</param>
        public void ChangeScreens(Type screenType)
        {
            ChangeScreens(screenType, null);
        }

        /// <summary>
        /// Changes the screen.
        /// </summary>
        /// <param name="screenType">Screen type.</param>
        /// <param name="screenArgs">Screen arguments.</param>
        public void ChangeScreens(Type screenType, params object[] screenArgs)
        {
            if (Transitioning)
            {
                return;
            }
            
            newScreen = (Screen)Activator.CreateInstance(screenType, screenArgs);

            if (currentScreen == null)
            {
                currentScreen = newScreen;
                return;
            }

            transitionSprite.OpacityEffect.CurrentMultiplier = 0.0f;
            transitionSprite.OpacityEffect.IsIncreasing = true;
            transitionSprite.OpacityEffect.Activate();
            transitionSprite.Active = true;

            Transitioning = true;
        }

        /// <summary>
        /// Transitions to the new screen.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        void Transition(GameTime gameTime)
        {
            transitionSprite.Update(gameTime);

            if (transitionSprite.ClientOpacity >= 1.0f)
            {
                currentScreen.UnloadContent();
                currentScreen = newScreen;
                currentScreen.LoadContent();

                transitionSprite.Active = false;
                Transitioning = false;
            }
        }
    }
}
