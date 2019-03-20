using System;

using NUnit.Framework;

using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;

namespace NuciXNA.UnitTests.Gui.GuiElements
{
    public class GuiElementTests
    {
        GuiElement loadedElement;

        [SetUp]
        public void SetUp()
        {
            loadedElement = new GuiElement();
            loadedElement.LoadContent();
        }

        [Test]
        public void LoadContent_ContentAlreadyLoaded_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => loadedElement.LoadContent());
        }

        [Test]
        public void UnoadContent_ContentNotLoaded_ThrowsInvalidOperationException()
        {
            GuiElement unloadedElement = new GuiElement();
            
            Assert.Throws<InvalidOperationException>(() => unloadedElement.UnloadContent());
        }
        
        [Test]
        public void NotDisposed_IsDisposedIsFalse()
        {
            Assert.IsFalse(loadedElement.IsDisposed);
        }
        
        [Test]
        public void Show_IsEnabledIsFalse()
        {
            loadedElement.Show();

            Assert.IsTrue(loadedElement.IsEnabled);
        }
        
        [Test]
        public void Show_IsVisibleIsFalse()
        {
            loadedElement.Show();

            Assert.IsTrue(loadedElement.IsVisible);
        }
        
        [Test]
        public void Show_FiresVisibilityChanged()
        {
            bool eventFired = false;

            loadedElement.VisibilityChanged += delegate { eventFired = true; };
            loadedElement.Show();

            Assert.IsTrue(eventFired);
        }
        
        [Test]
        public void Hide_IsEnabledIsFalse()
        {
            loadedElement.Hide();

            Assert.IsFalse(loadedElement.IsEnabled);
        }
        
        [Test]
        public void Hide_IsVisibleIsFalse()
        {
            loadedElement.Hide();

            Assert.IsFalse(loadedElement.IsVisible);
        }
        
        [Test]
        public void Hide_FiresVisibilityChanged()
        {
            bool eventFired = false;

            loadedElement.VisibilityChanged += delegate { eventFired = true; };
            loadedElement.Hide();

            Assert.IsTrue(eventFired);
        }

        public void SetForegroundColour_FiresForegroundColourChanged()
        {
            bool eventFired = false;

            loadedElement.ForegroundColourChanged += delegate { eventFired = true; };
            loadedElement.ForegroundColour = Colour.ChromeYellow;

            Assert.IsTrue(eventFired);
        }

        public void SetBackgroundColour_FiresBackgroundColourChanged()
        {
            bool eventFired = false;

            loadedElement.BackgroundColourChanged += delegate { eventFired = true; };
            loadedElement.BackgroundColour = Colour.ChromeYellow;

            Assert.IsTrue(eventFired);
        }

        public void SetOpacity_FiresOpacityChanged()
        {
            bool eventFired = false;

            loadedElement.OpacityChanged += delegate { eventFired = true; };
            loadedElement.Opacity = 0.12f;

            Assert.IsTrue(eventFired);
        }

        public void SetFontName_FiresFontNameChanged()
        {
            bool eventFired = false;

            loadedElement.FontNameChanged += delegate { eventFired = true; };
            loadedElement.FontName = "TestFont";

            Assert.IsTrue(eventFired);
        }

        public void SetLocation_FiresLocationChanged()
        {
            bool eventFired = false;

            loadedElement.LocationChanged += delegate { eventFired = true; };
            loadedElement.Location = Point2D.Empty;

            Assert.IsTrue(eventFired);
        }

        public void SetSize_FiresSizeChanged()
        {
            bool eventFired = false;

            loadedElement.SizeChanged += delegate { eventFired = true; };
            loadedElement.Size = Size2D.Empty;

            Assert.IsTrue(eventFired);
        }

        public void SetIsVisible_FiresVisibilityChanged()
        {
            bool eventFired = false;

            loadedElement.VisibilityChanged += delegate { eventFired = true; };
            loadedElement.IsVisible = true;

            Assert.IsTrue(eventFired);
        }
    }
}
