using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace NuciXNA.Gui
{
    public sealed class Cursor
    {
        private ButtonState state;

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Point2D Location { get; set; }

        public Point2D LocationOffset { get; set; }

        public ButtonState State
        {
            get
            {
                if (state is not null)
                {
                    return state;
                }

                return ButtonState.Released;
            }
            set => state = value;
        }

        public int Frames { get; set; }

        public Size2D SpriteSize { get; set; }

        public Scale2D Scale { get; set; }

        /// <summary>
        /// Gets or sets a single content file to use instead of the default idle/click pair.
        /// When set, a single sprite is loaded from this path and no click-state switching occurs.
        /// When not set (the default), the cursor uses <c>Cursors/idle</c> and <c>Cursors/click</c>.
        /// Changing this property after content is loaded immediately swaps the spritesheet.
        /// </summary>
        public string ContentFile
        {
            get => contentFile;
            set
            {
                if (string.Equals(contentFile, value))
                {
                    return;
                }

                contentFile = value;

                if (isContentLoaded)
                {
                    ReloadIdleSprite();
                }
            }
        }

        private string contentFile;
        private bool isContentLoaded;

        private TextureSprite idleSprite;
        private TextureSprite clickSprite;

        public Cursor()
        {
            Frames = 1;
            Scale = Scale2D.One;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            if (string.IsNullOrEmpty(ContentFile))
            {
                idleSprite = new TextureSprite { ContentFile = "Cursors/idle" };
                clickSprite = new TextureSprite { ContentFile = "Cursors/click" };

                SetChildrenProperties();

                idleSprite.LoadContent();
                clickSprite.LoadContent();

                InputManager.Instance.MouseButtonPressed += OnInputManagerMouseButtonPressed;
                InputManager.Instance.MouseButtonReleased += OnInputManagerMouseButtonReleased;
            }
            else
            {
                idleSprite = new TextureSprite { ContentFile = ContentFile };

                SetChildrenProperties();

                idleSprite.LoadContent();
            }

            isContentLoaded = true;
            InputManager.Instance.MouseMoved += OnInputManagerMouseMoved;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            isContentLoaded = false;
            idleSprite.UnloadContent();
            clickSprite?.UnloadContent();

            if (string.IsNullOrEmpty(ContentFile))
            {
                InputManager.Instance.MouseButtonPressed -= OnInputManagerMouseButtonPressed;
                InputManager.Instance.MouseButtonReleased -= OnInputManagerMouseButtonReleased;
            }

            InputManager.Instance.MouseMoved -= OnInputManagerMouseMoved;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            SetChildrenProperties();

            idleSprite.Update(gameTime);
            clickSprite?.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (clickSprite is not null && object.Equals(State, ButtonState.Pressed))
            {
                clickSprite.Draw(spriteBatch);
            }
            else
            {
                idleSprite.Draw(spriteBatch);
            }
        }

        private void SetChildrenProperties()
        {
            idleSprite.Location = Location + LocationOffset;

            clickSprite?.Location = Location + LocationOffset;

            if (!object.Equals(SpriteSize, Size2D.Empty))
            {
                idleSprite.SpriteSize = SpriteSize;

                clickSprite?.SpriteSize = SpriteSize;
            }

            idleSprite.Scale = Scale;

            clickSprite?.Scale = Scale;
        }

        private void OnInputManagerMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (object.Equals(e.Button, MouseButton.Left))
            {
                State = ButtonState.Pressed;
            }
        }

        private void OnInputManagerMouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (object.Equals(e.Button, MouseButton.Left))
            {
                State = ButtonState.Released;
            }
        }

        private void OnInputManagerMouseMoved(object sender, MouseEventArgs e) => Location = e.Location;

        private void ReloadIdleSprite()
        {
            idleSprite.UnloadContent();
            idleSprite = new TextureSprite { ContentFile = contentFile };
            SetChildrenProperties();
            idleSprite.LoadContent();
        }
    }
}
