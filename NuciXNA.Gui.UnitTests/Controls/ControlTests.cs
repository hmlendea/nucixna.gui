using System;

using NuciXNA.Primitives;

using NUnit.Framework;

using NuciXNA.Gui.Controls;
using NuciXNA.Gui.UnitTests.Helpers;

namespace NuciXNA.Gui.UnitTests.Controls
{
    [TestFixture]
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
        public void GivenContentAlreadyLoaded_WhenLoadContent_ThenThrowsInvalidOperationException()
            => Assert.Throws<InvalidOperationException>(loadedControl.LoadContent);

        [Test]
        public void GivenContentNotLoaded_WhenUnloadContent_ThenThrowsInvalidOperationException()
        {
            IGuiControl unloadedControl = new DummyControl();

            Assert.Throws<InvalidOperationException>(unloadedControl.UnloadContent);
        }

        [Test]
        public void GivenFreshControl_WhenNotDisposed_ThenIsDisposedIsFalse()
        {
            Assert.That(loadedControl.IsDisposed, Is.False);
        }

        [Test]
        public void GivenLoadedControl_WhenShow_ThenIsVisibleIsTrue()
        {
            loadedControl.Show();

            Assert.That(loadedControl.IsVisible);
        }

        [Test]
        public void GivenLoadedControl_WhenShow_ThenFiresShown()
        {
            bool eventFired = false;

            loadedControl.Shown += delegate { eventFired = true; };
            loadedControl.Show();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedControl_WhenHide_ThenIsVisibleIsFalse()
        {
            loadedControl.Hide();

            Assert.That(loadedControl.IsVisible, Is.False);
        }

        [Test]
        public void GivenLoadedControl_WhenHide_ThenFiresHidden()
        {
            bool eventFired = false;

            loadedControl.Hidden += delegate { eventFired = true; };
            loadedControl.Hide();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedControl_WhenSettingForegroundColour_ThenFiresForegroundColourChanged()
        {
            bool eventFired = false;

            loadedControl.ForegroundColourChanged += delegate { eventFired = true; };
            loadedControl.ForegroundColour = Colour.ChromeYellow;

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedControl_WhenSettingBackgroundColour_ThenFiresBackgroundColourChanged()
        {
            bool eventFired = false;

            loadedControl.BackgroundColourChanged += delegate { eventFired = true; };
            loadedControl.BackgroundColour = Colour.ChromeYellow;

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedControl_WhenSettingOpacity_ThenFiresOpacityChanged()
        {
            bool eventFired = false;

            loadedControl.OpacityChanged += delegate { eventFired = true; };
            loadedControl.Opacity = 0.12f;

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedControl_WhenSettingFontName_ThenFiresFontNameChanged()
        {
            bool eventFired = false;

            loadedControl.FontNameChanged += delegate { eventFired = true; };
            loadedControl.FontName = "TestFont";

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedControl_WhenSettingLocation_ThenFiresLocationChanged()
        {
            bool eventFired = false;

            loadedControl.LocationChanged += delegate { eventFired = true; };
            loadedControl.Location = Point2D.Empty;

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedControl_WhenSettingSize_ThenFiresSizeChanged()
        {
            bool eventFired = false;

            loadedControl.SizeChanged += delegate { eventFired = true; };
            loadedControl.Size = Size2D.Empty;

            Assert.That(eventFired);
        }

        [Test]
        public void GivenDisabledControl_WhenEnable_ThenIsEnabledIsTrue()
        {
            loadedControl.Disable();
            loadedControl.Enable();

            Assert.That(loadedControl.IsEnabled);
        }

        [Test]
        public void GivenLoadedControl_WhenEnable_ThenFiresEnabled()
        {
            bool eventFired = false;

            loadedControl.Enabled += delegate { eventFired = true; };
            loadedControl.Enable();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenEnabledControl_WhenDisable_ThenIsEnabledIsFalse()
        {
            loadedControl.Disable();

            Assert.That(loadedControl.IsEnabled, Is.False);
        }

        [Test]
        public void GivenLoadedControl_WhenDisable_ThenFiresDisabled()
        {
            bool eventFired = false;

            loadedControl.Disabled += delegate { eventFired = true; };
            loadedControl.Disable();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenUnfocusedControl_WhenFocus_ThenIsFocusedIsTrue()
        {
            loadedControl.Focus();

            Assert.That(loadedControl.IsFocused);
        }

        [Test]
        public void GivenLoadedControl_WhenFocus_ThenFiresFocused()
        {
            bool eventFired = false;

            loadedControl.Focused += delegate { eventFired = true; };
            loadedControl.Focus();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenFocusedControl_WhenUnfocus_ThenIsFocusedIsFalse()
        {
            loadedControl.Focus();
            loadedControl.Unfocus();

            Assert.That(loadedControl.IsFocused, Is.False);
        }

        [Test]
        public void GivenLoadedControl_WhenUnfocus_ThenFiresUnfocused()
        {
            bool eventFired = false;

            loadedControl.Unfocused += delegate { eventFired = true; };
            loadedControl.Unfocus();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenUnloadedControl_WhenLoadContent_ThenIsContentLoadedIsTrue()
            => Assert.That(loadedControl.IsContentLoaded);

        [Test]
        public void GivenLoadedControl_WhenUnloadContent_ThenIsContentLoadedIsFalse()
        {
            loadedControl.UnloadContent();

            Assert.That(loadedControl.IsContentLoaded, Is.False);
        }

        [Test]
        public void GivenUnloadedControl_WhenLoadContent_ThenFiresContentLoading()
        {
            DummyControl control = new();
            bool eventFired = false;

            control.ContentLoading += delegate { eventFired = true; };
            control.LoadContent();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenUnloadedControl_WhenLoadContent_ThenFiresContentLoaded()
        {
            DummyControl control = new();
            bool eventFired = false;

            control.ContentLoaded += delegate { eventFired = true; };
            control.LoadContent();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedControl_WhenUnloadContent_ThenFiresContentUnloading()
        {
            bool eventFired = false;

            loadedControl.ContentUnloading += delegate { eventFired = true; };
            loadedControl.UnloadContent();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedControl_WhenUnloadContent_ThenFiresContentUnloaded()
        {
            bool eventFired = false;

            loadedControl.ContentUnloaded += delegate { eventFired = true; };
            loadedControl.UnloadContent();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedControl_WhenDispose_ThenIsDisposedIsTrue()
        {
            loadedControl.Dispose();

            Assert.That(loadedControl.IsDisposed);
        }

        [Test]
        public void GivenNoOpacitySet_WhenGettingOpacity_ThenDefaultIsOne()
        {
            DummyControl control = new();

            Assert.That(control.Opacity, Is.EqualTo(1.0f));
        }

        [Test]
        public void GivenOpacityAboveOne_WhenSet_ThenOpacityIsClampedToOne()
        {
            loadedControl.Opacity = 2.5f;

            Assert.That(loadedControl.Opacity, Is.EqualTo(1.0f));
        }

        [Test]
        public void GivenOpacityBelowZero_WhenSet_ThenOpacityIsClampedToZero()
        {
            loadedControl.Opacity = -0.5f;

            Assert.That(loadedControl.Opacity, Is.EqualTo(0.0f));
        }

        [Test]
        public void GivenParentWithOpacity_WhenChildHasNoOpacity_ThenChildInheritsParentOpacity()
        {
            DummyControl parent = new();
            DummyControl child = new();

            parent.Opacity = 0.42f;
            child.Parent = parent;

            Assert.That(child.Opacity, Is.EqualTo(0.42f));
        }

        [Test]
        public void GivenNoLocationSet_WhenGettingLocation_ThenReturnsEmpty()
        {
            DummyControl control = new();

            Assert.That(control.Location, Is.EqualTo(Point2D.Empty));
        }

        [Test]
        public void GivenNoParent_WhenGettingScreenLocation_ThenEqualsLocation()
        {
            loadedControl.Location = new Point2D(10, 20);

            Assert.That(loadedControl.ScreenLocation.X, Is.EqualTo(10));
            Assert.That(loadedControl.ScreenLocation.Y, Is.EqualTo(20));
        }

        [Test]
        public void GivenParentWithLocation_WhenGettingChildScreenLocation_ThenEqualsParentPlusChildLocation()
        {
            DummyControl parent = new();
            DummyControl child = new();

            parent.Location = new Point2D(10, 20);
            child.Location = new Point2D(5, 15);
            child.Parent = parent;

            Assert.That(child.ScreenLocation.X, Is.EqualTo(15));
            Assert.That(child.ScreenLocation.Y, Is.EqualTo(35));
        }

        [Test]
        public void GivenNoSizeSet_WhenGettingSize_ThenReturnsEmpty()
        {
            DummyControl control = new();

            Assert.That(control.Size, Is.EqualTo(Size2D.Empty));
        }

        [Test]
        public void GivenParentWithSize_WhenChildHasNoSize_ThenChildInheritsParentSize()
        {
            DummyControl parent = new();
            DummyControl child = new();

            parent.Size = new Size2D(100, 200);
            child.Parent = parent;

            Assert.That(child.Size.Width, Is.EqualTo(100));
            Assert.That(child.Size.Height, Is.EqualTo(200));
        }

        [Test]
        public void GivenParentHidden_WhenChildIsVisible_ThenIsVisibleIsFalse()
        {
            DummyControl parent = new();
            DummyControl child = new()
            {
                Parent = parent
            };
            child.Show();
            parent.Hide();

            Assert.That(child.IsVisible, Is.False);
        }

        [Test]
        public void GivenParentDisabled_WhenChildIsEnabled_ThenIsEnabledIsFalse()
        {
            DummyControl parent = new();
            DummyControl child = new()
            {
                Parent = parent
            };
            child.Enable();
            parent.Disable();

            Assert.That(child.IsEnabled, Is.False);
        }

        [Test]
        public void GivenLocationAndSize_WhenGettingClientRectangle_ThenMatchesLocationAndSize()
        {
            DummyControl control = new()
            {
                Location = new Point2D(10, 20),
                Size = new Size2D(100, 50)
            };

            Rectangle2D expected = new(control.Location, control.Size);

            Assert.That(control.ClientRectangle, Is.EqualTo(expected));
        }

        [Test]
        public void GivenLocationAndSize_WhenGettingDisplayRectangle_ThenMatchesScreenLocationAndSize()
        {
            DummyControl control = new()
            {
                Location = new Point2D(30, 40),
                Size = new Size2D(200, 100)
            };

            Rectangle2D expected = new(control.ScreenLocation, control.Size);

            Assert.That(control.DisplayRectangle, Is.EqualTo(expected));
        }

        [Test]
        public void GivenParentWithLocation_WhenGettingChildDisplayRectangle_ThenUsesScreenLocation()
        {
            DummyControl parent = new();
            DummyControl child = new();
            parent.Location = new Point2D(10, 10);
            child.Location = new Point2D(5, 5);
            child.Size = new Size2D(50, 25);
            child.Parent = parent;

            Rectangle2D expected = new(child.ScreenLocation, child.Size);

            Assert.That(child.DisplayRectangle, Is.EqualTo(expected));
        }

        // ── Id ────────────────────────────────────────────────────────────────

        [Test]
        public void GivenFreshControl_WhenGettingId_ThenIdIsNotNullOrEmpty()
        {
            DummyControl control = new();

            Assert.That(control.Id, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void GivenTwoNewControls_WhenGettingIds_ThenIdsAreDifferent()
        {
            DummyControl first = new();
            DummyControl second = new();

            Assert.That(first.Id, Is.Not.EqualTo(second.Id));
        }

        // ── Default property values ────────────────────────────────────────────

        [Test]
        public void GivenFreshControl_WhenGettingIsEnabled_ThenIsTrue()
        {
            DummyControl control = new();

            Assert.That(control.IsEnabled);
        }

        [Test]
        public void GivenFreshControl_WhenGettingIsVisible_ThenIsTrue()
        {
            DummyControl control = new();

            Assert.That(control.IsVisible);
        }

        [Test]
        public void GivenFreshControl_WhenGettingIsFocused_ThenIsFalse()
        {
            DummyControl control = new();

            Assert.That(control.IsFocused, Is.False);
        }

        [Test]
        public void GivenFreshControl_WhenGettingIsHovered_ThenIsFalse()
        {
            DummyControl control = new();

            Assert.That(control.IsHovered, Is.False);
        }

        [Test]
        public void GivenFreshControl_WhenGettingIsContentLoaded_ThenIsFalse()
        {
            DummyControl control = new();

            Assert.That(control.IsContentLoaded, Is.False);
        }

        // ── Colour and font inheritance ────────────────────────────────────────

        [Test]
        public void GivenParentWithBackgroundColour_WhenChildHasNoBackgroundColour_ThenChildInheritsParentBackgroundColour()
        {
            DummyControl parent = new();
            DummyControl child = new();

            parent.BackgroundColour = Colour.ChromeYellow;
            child.Parent = parent;

            Assert.That(child.BackgroundColour, Is.EqualTo(Colour.ChromeYellow));
        }

        [Test]
        public void GivenChildWithBackgroundColour_WhenParentHasDifferentBackgroundColour_ThenChildBackgroundColourTakesPrecedence()
        {
            DummyControl parent = new();
            DummyControl child = new();

            parent.BackgroundColour = Colour.Gold;
            child.BackgroundColour = Colour.ChromeYellow;
            child.Parent = parent;

            Assert.That(child.BackgroundColour, Is.EqualTo(Colour.ChromeYellow));
        }

        [Test]
        public void GivenParentWithForegroundColour_WhenChildHasNoForegroundColour_ThenChildInheritsParentForegroundColour()
        {
            DummyControl parent = new();
            DummyControl child = new();

            parent.ForegroundColour = Colour.ChromeYellow;
            child.Parent = parent;

            Assert.That(child.ForegroundColour, Is.EqualTo(Colour.ChromeYellow));
        }

        [Test]
        public void GivenParentWithFontName_WhenChildHasNoFontName_ThenChildInheritsParentFontName()
        {
            DummyControl parent = new();
            DummyControl child = new();

            parent.FontName = "TestFont";
            child.Parent = parent;

            Assert.That(child.FontName, Is.EqualTo("TestFont"));
        }

        // ── Dispose ────────────────────────────────────────────────────────────

        [Test]
        public void GivenLoadedControl_WhenDispose_ThenIsContentLoadedIsFalse()
        {
            loadedControl.Dispose();

            Assert.That(loadedControl.IsContentLoaded, Is.False);
        }
    }
}
