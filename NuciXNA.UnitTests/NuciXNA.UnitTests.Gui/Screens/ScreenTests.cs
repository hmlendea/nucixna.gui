using System;

using NUnit.Framework;

using NuciXNA.Gui.Screens;
using NuciXNA.Primitives;

namespace NuciXNA.UnitTests.Gui.Screens
{
    public class ScreenTests
    {
        Screen loadedScreen;

        [SetUp]
        public void SetUp()
        {
            loadedScreen = new Screen();
            loadedScreen.LoadContent();
        }

        [Test]
        public void LoadContent_ContentAlreadyLoaded_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => loadedScreen.LoadContent());
        }

        [Test]
        public void UnloadContent_ContentNotLoaded_ThrowsInvalidOperationException()
        {
            Screen unloadedScreen = new Screen();
            
            Assert.Throws<InvalidOperationException>(() => unloadedScreen.UnloadContent());
        }
        
        [Test]
        public void NotDisposed_IsDisposedIsFalse()
        {
            Assert.IsFalse(loadedScreen.IsDisposed);
        }
        
        public void SetForegroundColour_FiresForegroundColourChanged()
        {
            bool eventFired = false;

            loadedScreen.ForegroundColourChanged += delegate { eventFired = true; };
            loadedScreen.ForegroundColour = Colour.ChromeYellow;

            Assert.IsTrue(eventFired);
        }

        public void SetBackgroundColour_FiresBackgroundColourChanged()
        {
            bool eventFired = false;

            loadedScreen.BackgroundColourChanged += delegate { eventFired = true; };
            loadedScreen.BackgroundColour = Colour.ChromeYellow;

            Assert.IsTrue(eventFired);
        }
    }
}
