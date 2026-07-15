using System;
using System.Collections.Generic;

using NuciXNA.Primitives;

using NUnit.Framework;

using NuciXNA.Gui.Screens;
using NuciXNA.Gui.UnitTests.Helpers;

namespace NuciXNA.Gui.UnitTests.Screens
{
    [TestFixture]
    public class ScreenExtendedTests
    {
        // ── Id uniqueness ──────────────────────────────────────────────────────

        [Test]
        public void GivenTwoFreshScreens_WhenGettingIds_ThenIdsAreDifferent()
        {
            DummyScreen first = new();
            DummyScreen second = new();

            Assert.That(first.Id, Is.Not.EqualTo(second.Id));
        }

        [Test]
        public void GivenManyFreshScreens_WhenGettingIds_ThenAllIdsAreUnique()
        {
            HashSet<string> identifiers = [];

            for (int iteration = 0; iteration < 50; iteration += 1)
            {
                identifiers.Add(new DummyScreen().Id);
            }

            Assert.That(identifiers.Count, Is.EqualTo(50));
        }

        [Test]
        public void GivenFreshScreen_WhenSettingId_ThenIdIsUpdated()
        {
            DummyScreen screen = new();
            screen.Id = "SolairOfAstora";

            Assert.That(screen.Id, Is.EqualTo("SolairOfAstora"));
        }

        // ── Default property values ────────────────────────────────────────────

        [Test]
        public void GivenFreshScreen_WhenGettingIsContentLoaded_ThenIsFalse()
        {
            DummyScreen screen = new();

            Assert.That(screen.IsContentLoaded, Is.False);
        }

        [Test]
        public void GivenFreshScreen_WhenGettingIsDisposed_ThenIsFalse()
        {
            DummyScreen screen = new();

            Assert.That(screen.IsDisposed, Is.False);
        }

        // ── LoadContent event ordering ─────────────────────────────────────────

        [Test]
        public void GivenFreshScreen_WhenLoadContent_ThenContentLoadingFiresBeforeContentLoaded()
        {
            DummyScreen screen = new();
            int loadingOrder = 0;
            int loadedOrder = 0;
            int sequence = 0;

            screen.ContentLoading += delegate { loadingOrder = ++sequence; };
            screen.ContentLoaded += delegate { loadedOrder = ++sequence; };

            screen.LoadContent();

            Assert.That(loadingOrder, Is.LessThan(loadedOrder));
        }

        // ── UnloadContent ──────────────────────────────────────────────────────

        [Test]
        public void GivenLoadedScreen_WhenUnloadContent_ThenContentUnloadingFiresBeforeContentUnloaded()
        {
            DummyScreen screen = new();
            screen.LoadContent();
            int unloadingOrder = 0;
            int unloadedOrder = 0;
            int sequence = 0;

            screen.ContentUnloading += delegate { unloadingOrder = ++sequence; };
            screen.ContentUnloaded += delegate { unloadedOrder = ++sequence; };

            screen.UnloadContent();

            Assert.That(unloadingOrder, Is.LessThan(unloadedOrder));
        }

        [Test]
        public void GivenLoadedScreen_WhenUnloadThenLoadAgain_ThenIsContentLoadedIsTrue()
        {
            DummyScreen screen = new();
            screen.LoadContent();
            screen.UnloadContent();

            screen.LoadContent();

            Assert.That(screen.IsContentLoaded);
        }

        // ── Dispose ────────────────────────────────────────────────────────────

        [Test]
        public void GivenLoadedScreen_WhenDispose_ThenDisposingFiresBeforeDisposed()
        {
            DummyScreen screen = new();
            screen.LoadContent();
            int disposingOrder = 0;
            int disposedOrder = 0;
            int sequence = 0;

            screen.Disposing += delegate { disposingOrder = ++sequence; };
            screen.Disposed += delegate { disposedOrder = ++sequence; };

            screen.Dispose();

            Assert.That(disposingOrder, Is.LessThan(disposedOrder));
        }

        [Test]
        public void GivenUnloadedScreen_WhenDispose_ThenIsDisposedIsTrue()
        {
            DummyScreen screen = new();

            screen.Dispose();

            Assert.That(screen.IsDisposed);
        }

        [Test]
        public void GivenDisposedScreen_WhenDisposeAgain_ThenDisposedEventFiresOnlyOnce()
        {
            DummyScreen screen = new();
            screen.LoadContent();
            int fireCount = 0;
            screen.Disposed += delegate { fireCount++; };

            screen.Dispose();
            screen.Dispose();

            Assert.That(fireCount, Is.EqualTo(1));
        }

        // ── BackgroundColour ───────────────────────────────────────────────────

        [Test]
        public void GivenScreen_WhenSettingBackgroundColourTwice_ThenLastValueWins()
        {
            DummyScreen screen = new();
            screen.BackgroundColour = Colour.Red;
            screen.BackgroundColour = Colour.Gold;

            Assert.That(screen.BackgroundColour, Is.EqualTo(Colour.Gold));
        }

        [Test]
        public void GivenScreen_WhenSettingBackgroundColourTwice_ThenBackgroundColourChangedFiresTwice()
        {
            DummyScreen screen = new();
            int fireCount = 0;
            screen.BackgroundColourChanged += delegate { fireCount++; };

            screen.BackgroundColour = Colour.Red;
            screen.BackgroundColour = Colour.Gold;

            Assert.That(fireCount, Is.EqualTo(2));
        }

        // ── ForegroundColour ───────────────────────────────────────────────────

        [Test]
        public void GivenScreen_WhenSettingForegroundColourTwice_ThenLastValueWins()
        {
            DummyScreen screen = new();
            screen.ForegroundColour = Colour.Red;
            screen.ForegroundColour = Colour.Gold;

            Assert.That(screen.ForegroundColour, Is.EqualTo(Colour.Gold));
        }

        [Test]
        public void GivenScreen_WhenSettingForegroundColourTwice_ThenForegroundColourChangedFiresTwice()
        {
            DummyScreen screen = new();
            int fireCount = 0;
            screen.ForegroundColourChanged += delegate { fireCount++; };

            screen.ForegroundColour = Colour.Red;
            screen.ForegroundColour = Colour.Gold;

            Assert.That(fireCount, Is.EqualTo(2));
        }
    }
}
