using System;

using NUnit.Framework;

using NuciXNA.Gui.Screens;
using NuciXNA.Gui.UnitTests.Helpers;
using NuciXNA.Primitives;

namespace NuciXNA.Gui.UnitTests.Screens
{
    public class ScreenTests
    {
        Screen loadedScreen;

        [SetUp]
        public void SetUp()
        {
            loadedScreen = new DummyScreen();
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
            Screen unloadedScreen = new DummyScreen();
            
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
