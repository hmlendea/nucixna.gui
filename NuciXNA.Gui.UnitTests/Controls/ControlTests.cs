using System;

using NUnit.Framework;

using NuciXNA.Gui.Controls;
using NuciXNA.Gui.UnitTests.Helpers;
using NuciXNA.Primitives;

namespace NuciXNA.Gui.UnitTests.Controls
{
    public class ControlTests
    {
        GuiControl loadedControl;

        [SetUp]
        public void SetUp()
        {
            loadedControl = new DummyControl();
            loadedControl.LoadContent();
        }

        [Test]
        public void LoadContent_ContentAlreadyLoaded_ThrowsInvalidOperationException()
            => Assert.Throws<InvalidOperationException>(() => loadedControl.LoadContent());

        [Test]
        public void UnloadContent_ContentNotLoaded_ThrowsInvalidOperationException()
        {
            IGuiControl unloadedControl = new DummyControl();

            Assert.Throws<InvalidOperationException>(() => unloadedControl.UnloadContent());
        }

        [Test]
        public void NotDisposed_IsDisposedIsFalse()
        {
            Assert.That(loadedControl.IsDisposed, Is.False);
        }

        [Test]
        public void Show_IsVisibleIsTrue()
        {
            loadedControl.Show();

            Assert.That(loadedControl.IsVisible);
        }

        [Test]
        public void Show_FiresShown()
        {
            bool eventFired = false;

            loadedControl.Shown += delegate { eventFired = true; };
            loadedControl.Show();

            Assert.That(eventFired);
        }

        [Test]
        public void Hide_IsVisibleIsFalse()
        {
            loadedControl.Hide();

            Assert.That(loadedControl.IsVisible, Is.False);
        }

        [Test]
        public void Hide_FiresHidden()
        {
            bool eventFired = false;

            loadedControl.Hidden += delegate { eventFired = true; };
            loadedControl.Hide();

            Assert.That(eventFired);
        }

        public void SetForegroundColour_FiresForegroundColourChanged()
        {
            bool eventFired = false;

            loadedControl.ForegroundColourChanged += delegate { eventFired = true; };
            loadedControl.ForegroundColour = Colour.ChromeYellow;

            Assert.That(eventFired);
        }

        public void SetBackgroundColour_FiresBackgroundColourChanged()
        {
            bool eventFired = false;

            loadedControl.BackgroundColourChanged += delegate { eventFired = true; };
            loadedControl.BackgroundColour = Colour.ChromeYellow;

            Assert.That(eventFired);
        }

        public void SetOpacity_FiresOpacityChanged()
        {
            bool eventFired = false;

            loadedControl.OpacityChanged += delegate { eventFired = true; };
            loadedControl.Opacity = 0.12f;

            Assert.That(eventFired);
        }

        public void SetFontName_FiresFontNameChanged()
        {
            bool eventFired = false;

            loadedControl.FontNameChanged += delegate { eventFired = true; };
            loadedControl.FontName = "TestFont";

            Assert.That(eventFired);
        }

        public void SetLocation_FiresLocationChanged()
        {
            bool eventFired = false;

            loadedControl.LocationChanged += delegate { eventFired = true; };
            loadedControl.Location = Point2D.Empty;

            Assert.That(eventFired);
        }

        public void SetSize_FiresSizeChanged()
        {
            bool eventFired = false;

            loadedControl.SizeChanged += delegate { eventFired = true; };
            loadedControl.Size = Size2D.Empty;

            Assert.That(eventFired);
        }
    }
}
