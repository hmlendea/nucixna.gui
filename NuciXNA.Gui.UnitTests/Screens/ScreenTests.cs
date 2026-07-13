using System;

using NuciXNA.Primitives;

using NUnit.Framework;

using NuciXNA.Gui.Screens;
using NuciXNA.Gui.UnitTests.Helpers;

namespace NuciXNA.Gui.UnitTests.Screens
{
    [TestFixture]
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
        public void GivenContentAlreadyLoaded_WhenLoadContent_ThenThrowsInvalidOperationException()
            => Assert.Throws<InvalidOperationException>(loadedScreen.LoadContent);

        [Test]
        public void GivenContentNotLoaded_WhenUnloadContent_ThenThrowsInvalidOperationException()
        {
            Screen unloadedScreen = new DummyScreen();

            Assert.Throws<InvalidOperationException>(unloadedScreen.UnloadContent);
        }

        [Test]
        public void GivenFreshScreen_WhenNotDisposed_ThenIsDisposedIsFalse()
            => Assert.That(loadedScreen.IsDisposed, Is.False);

        [Test]
        public void GivenLoadedScreen_WhenSettingForegroundColour_ThenFiresForegroundColourChanged()
        {
            bool eventFired = false;

            loadedScreen.ForegroundColourChanged += delegate { eventFired = true; };
            loadedScreen.ForegroundColour = Colour.ChromeYellow;

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedScreen_WhenSettingBackgroundColour_ThenFiresBackgroundColourChanged()
        {
            bool eventFired = false;

            loadedScreen.BackgroundColourChanged += delegate { eventFired = true; };
            loadedScreen.BackgroundColour = Colour.ChromeYellow;

            Assert.That(eventFired);
        }

        [Test]
        public void GivenUnloadedScreen_WhenLoadContent_ThenIsContentLoadedIsTrue()
            => Assert.That(loadedScreen.IsContentLoaded);

        [Test]
        public void GivenLoadedScreen_WhenUnloadContent_ThenIsContentLoadedIsFalse()
        {
            loadedScreen.UnloadContent();

            Assert.That(loadedScreen.IsContentLoaded, Is.False);
        }

        [Test]
        public void GivenUnloadedScreen_WhenLoadContent_ThenFiresContentLoading()
        {
            DummyScreen screen = new();
            bool eventFired = false;

            screen.ContentLoading += delegate { eventFired = true; };
            screen.LoadContent();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenUnloadedScreen_WhenLoadContent_ThenFiresContentLoaded()
        {
            DummyScreen screen = new();
            bool eventFired = false;

            screen.ContentLoaded += delegate { eventFired = true; };
            screen.LoadContent();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedScreen_WhenUnloadContent_ThenFiresContentUnloading()
        {
            bool eventFired = false;

            loadedScreen.ContentUnloading += delegate { eventFired = true; };
            loadedScreen.UnloadContent();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedScreen_WhenUnloadContent_ThenFiresContentUnloaded()
        {
            bool eventFired = false;

            loadedScreen.ContentUnloaded += delegate { eventFired = true; };
            loadedScreen.UnloadContent();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedScreen_WhenDispose_ThenIsDisposedIsTrue()
        {
            loadedScreen.Dispose();

            Assert.That(loadedScreen.IsDisposed);
        }

        [Test]
        public void GivenLoadedScreen_WhenDispose_ThenIsContentLoadedIsFalse()
        {
            loadedScreen.Dispose();

            Assert.That(loadedScreen.IsContentLoaded, Is.False);
        }

        // ── Id ────────────────────────────────────────────────────────────────

        [Test]
        public void GivenFreshScreen_WhenGettingId_ThenIdIsNotNullOrEmpty()
        {
            DummyScreen screen = new();

            Assert.That(screen.Id, Is.Not.Null.And.Not.Empty);
        }

        // ── Default property values ────────────────────────────────────────────

        [Test]
        public void GivenFreshScreen_WhenGettingBackgroundColour_ThenIsBlack()
        {
            DummyScreen screen = new();

            Assert.That(screen.BackgroundColour, Is.EqualTo(Colour.Black));
        }

        // ── Property setters ──────────────────────────────────────────────────

        [Test]
        public void GivenFreshScreen_WhenSettingBackgroundColour_ThenBackgroundColourIsUpdated()
        {
            DummyScreen screen = new();

            screen.BackgroundColour = Colour.ChromeYellow;

            Assert.That(screen.BackgroundColour, Is.EqualTo(Colour.ChromeYellow));
        }

        [Test]
        public void GivenFreshScreen_WhenSettingForegroundColour_ThenForegroundColourIsUpdated()
        {
            DummyScreen screen = new();

            screen.ForegroundColour = Colour.ChromeYellow;

            Assert.That(screen.ForegroundColour, Is.EqualTo(Colour.ChromeYellow));
        }

        // ── Dispose ────────────────────────────────────────────────────────────

        [Test]
        public void GivenLoadedScreen_WhenDispose_ThenFiresDisposing()
        {
            bool eventFired = false;

            loadedScreen.Disposing += delegate { eventFired = true; };
            loadedScreen.Dispose();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedScreen_WhenDispose_ThenFiresDisposed()
        {
            bool eventFired = false;

            loadedScreen.Disposed += delegate { eventFired = true; };
            loadedScreen.Dispose();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenDisposedScreen_WhenDisposedAgain_ThenDoesNotThrow()
        {
            loadedScreen.Dispose();

            Assert.DoesNotThrow(loadedScreen.Dispose);
        }
    }
}
