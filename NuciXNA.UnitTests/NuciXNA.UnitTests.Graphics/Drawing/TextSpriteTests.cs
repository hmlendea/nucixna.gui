using System;

using NUnit.Framework;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Primitives;

namespace NuciXNA.UnitTests.Graphics.Drawing
{
    public class TextSpriteTests
    {
        [Test]
        public void LoadContent_ContentAlreadyLoaded_ThrowsInvalidOperationException()
        {
            TextSprite textSprite = new TextSprite();
            textSprite.LoadContent();

            Assert.Throws<InvalidOperationException>(() => textSprite.LoadContent());
        }

        [Test]
        public void LoadContent_FiresContentLoading()
        {
            bool eventFired = false;

            TextSprite textSprite = new TextSprite();
            textSprite.ContentLoading += delegate { eventFired = true; };

            textSprite.LoadContent();

            Assert.IsTrue(eventFired);
        }

        [Test]
        public void LoadContent_FiresContentLoaded()
        {
            bool eventFired = false;

            TextSprite textSprite = new TextSprite();
            textSprite.ContentLoaded += delegate { eventFired = true; };

            textSprite.LoadContent();

            Assert.IsTrue(eventFired);
        }

        [Test]
        public void LoadContent_FiresContentLoadingBeforeContentLoaded()
        {
            DateTime firstEventTime = DateTime.Now;
            DateTime lastEventTime = DateTime.Now;

            TextSprite textSprite = new TextSprite();
            textSprite.ContentLoading += delegate { firstEventTime = DateTime.Now; };
            textSprite.ContentLoading += delegate { lastEventTime = DateTime.Now; };

            textSprite.LoadContent();

            Assert.IsTrue(firstEventTime < lastEventTime);
        }

        [Test]
        public void UnloadContent_ContentNotLoaded_ThrowsInvalidOperationException()
        {
            TextSprite textSprite = new TextSprite();
            
            Assert.Throws<InvalidOperationException>(() => textSprite.UnloadContent());
        }

        [Test]
        public void UnloadContent_FiresContentUnloading()
        {
            bool eventFired = false;

            TextSprite textSprite = new TextSprite();
            textSprite.ContentUnloading += delegate { eventFired = true; };

            textSprite.LoadContent();
            textSprite.UnloadContent();

            Assert.IsTrue(eventFired);
        }

        [Test]
        public void UnloadContent_FiresContentUnloaded()
        {
            bool eventFired = false;

            TextSprite textSprite = new TextSprite();
            textSprite.ContentUnloaded += delegate { eventFired = true; };

            textSprite.LoadContent();
            textSprite.UnloadContent();

            Assert.IsTrue(eventFired);
        }

        [Test]
        public void UnloadContent_FiresContentUnloadingBeforeContentUnloaded()
        {
            DateTime firstEventTime = DateTime.Now;
            DateTime lastEventTime = DateTime.Now;

            TextSprite textSprite = new TextSprite();
            textSprite.ContentUnloading += delegate { firstEventTime = DateTime.Now; };
            textSprite.ContentUnloading += delegate { lastEventTime = DateTime.Now; };

            textSprite.LoadContent();
            textSprite.UnloadContent();

            Assert.IsTrue(firstEventTime < lastEventTime);
        }

        [Test]
        public void Update_ContentNotLoaded_ThrowsInvalidOperationException()
        {
            TextSprite textSprite = new TextSprite();
            
            Assert.Throws<InvalidOperationException>(() => textSprite.Update(null));
        }

        [Test]
        public void Update_FiresUpdating()
        {
            bool eventFired = false;

            TextSprite textSprite = new TextSprite();
            textSprite.Updating += delegate { eventFired = true; };

            textSprite.LoadContent();
            textSprite.Update(null);

            Assert.IsTrue(eventFired);
        }

        [Test]
        public void Update_FiresUpdated()
        {
            bool eventFired = false;

            TextSprite textSprite = new TextSprite();
            textSprite.Updated += delegate { eventFired = true; };

            textSprite.LoadContent();
            textSprite.Update(null);

            Assert.IsTrue(eventFired);
        }

        [Test]
        public void Update_FiresUpdatingBeforeUpdated()
        {
            DateTime firstEventTime = DateTime.Now;
            DateTime lastEventTime = DateTime.Now;

            TextSprite textSprite = new TextSprite();
            textSprite.Updating += delegate { firstEventTime = DateTime.Now; };
            textSprite.Updating += delegate { lastEventTime = DateTime.Now; };

            textSprite.LoadContent();
            textSprite.Update(null);

            Assert.IsTrue(firstEventTime < lastEventTime);
        }

        [Test]
        public void Draw_ContentNotLoaded_ThrowsInvalidOperationException()
        {
            TextSprite textSprite = new TextSprite();
            
            Assert.Throws<InvalidOperationException>(() => textSprite.Draw(null));
        }

        [Test]
        public void Draw_FiresDrawing()
        {
            bool eventFired = false;

            TextSprite textSprite = new TextSprite();
            textSprite.Drawing += delegate { eventFired = true; };

            textSprite.LoadContent();
            textSprite.Draw(null);

            Assert.IsTrue(eventFired);
        }

        [Test]
        public void Draw_FiresDrawn()
        {
            bool eventFired = false;

            TextSprite textSprite = new TextSprite();
            textSprite.Drawn += delegate { eventFired = true; };

            textSprite.LoadContent();
            textSprite.Draw(null);

            Assert.IsTrue(eventFired);
        }

        [Test]
        public void Draw_FiresDrawingBeforeDrawn()
        {
            DateTime firstEventTime = DateTime.Now;
            DateTime lastEventTime = DateTime.Now;

            TextSprite textSprite = new TextSprite();
            textSprite.Drawing += delegate { firstEventTime = DateTime.Now; };
            textSprite.Drawing += delegate { lastEventTime = DateTime.Now; };

            textSprite.LoadContent();
            textSprite.Draw(null);

            Assert.IsTrue(firstEventTime < lastEventTime);
        }
        
        [Test]
        public void GetIsDisposed_NotDisposed_ReturnsFalse()
        {
            TextSprite textSprite = new TextSprite();
            textSprite.LoadContent();

            Assert.IsFalse(textSprite.IsDisposed);
        }
    }
}
