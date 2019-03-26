using System;

using NUnit.Framework;

using NuciXNA.Gui.Controls;
using NuciXNA.Primitives;

namespace NuciXNA.UnitTests.Gui.Controls
{
    public class ControlTests
    {
        Control loadedControl;

        [SetUp]
        public void SetUp()
        {
            loadedControl = new Control();
            loadedControl.LoadContent();
        }

        [Test]
        public void LoadContent_ContentAlreadyLoaded_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => loadedControl.LoadContent());
        }

        [Test]
        public void UnloadContent_ContentNotLoaded_ThrowsInvalidOperationException()
        {
            Control unloadedControl = new Control();
            
            Assert.Throws<InvalidOperationException>(() => unloadedControl.UnloadContent());
        }
        
        [Test]
        public void NotDisposed_IsDisposedIsFalse()
        {
            Assert.IsFalse(loadedControl.IsDisposed);
        }
        
        [Test]
        public void Show_IsEnabledIsFalse()
        {
            loadedControl.Show();

            Assert.IsTrue(loadedControl.IsEnabled);
        }
        
        [Test]
        public void Show_IsVisibleIsFalse()
        {
            loadedControl.Show();

            Assert.IsTrue(loadedControl.IsVisible);
        }
        
        [Test]
        public void Show_FiresVisibilityChanged()
        {
            bool eventFired = false;

            loadedControl.VisibilityChanged += delegate { eventFired = true; };
            loadedControl.Show();

            Assert.IsTrue(eventFired);
        }
        
        [Test]
        public void Hide_IsEnabledIsFalse()
        {
            loadedControl.Hide();

            Assert.IsFalse(loadedControl.IsEnabled);
        }
        
        [Test]
        public void Hide_IsVisibleIsFalse()
        {
            loadedControl.Hide();

            Assert.IsFalse(loadedControl.IsVisible);
        }
        
        [Test]
        public void Hide_FiresVisibilityChanged()
        {
            bool eventFired = false;

            loadedControl.VisibilityChanged += delegate { eventFired = true; };
            loadedControl.Hide();

            Assert.IsTrue(eventFired);
        }

        public void SetForegroundColour_FiresForegroundColourChanged()
        {
            bool eventFired = false;

            loadedControl.ForegroundColourChanged += delegate { eventFired = true; };
            loadedControl.ForegroundColour = Colour.ChromeYellow;

            Assert.IsTrue(eventFired);
        }

        public void SetBackgroundColour_FiresBackgroundColourChanged()
        {
            bool eventFired = false;

            loadedControl.BackgroundColourChanged += delegate { eventFired = true; };
            loadedControl.BackgroundColour = Colour.ChromeYellow;

            Assert.IsTrue(eventFired);
        }

        public void SetOpacity_FiresOpacityChanged()
        {
            bool eventFired = false;

            loadedControl.OpacityChanged += delegate { eventFired = true; };
            loadedControl.Opacity = 0.12f;

            Assert.IsTrue(eventFired);
        }

        public void SetFontName_FiresFontNameChanged()
        {
            bool eventFired = false;

            loadedControl.FontNameChanged += delegate { eventFired = true; };
            loadedControl.FontName = "TestFont";

            Assert.IsTrue(eventFired);
        }

        public void SetLocation_FiresLocationChanged()
        {
            bool eventFired = false;

            loadedControl.LocationChanged += delegate { eventFired = true; };
            loadedControl.Location = Point2D.Empty;

            Assert.IsTrue(eventFired);
        }

        public void SetSize_FiresSizeChanged()
        {
            bool eventFired = false;

            loadedControl.SizeChanged += delegate { eventFired = true; };
            loadedControl.Size = Size2D.Empty;

            Assert.IsTrue(eventFired);
        }

        public void SetIsVisible_FiresVisibilityChanged()
        {
            bool eventFired = false;

            loadedControl.VisibilityChanged += delegate { eventFired = true; };
            loadedControl.IsVisible = true;

            Assert.IsTrue(eventFired);
        }
    }
}
