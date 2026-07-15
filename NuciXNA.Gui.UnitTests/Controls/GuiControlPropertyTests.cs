using NuciXNA.Primitives;

using NUnit.Framework;

using NuciXNA.Gui.Controls;
using NuciXNA.Gui.UnitTests.Helpers;

namespace NuciXNA.Gui.UnitTests.Controls
{
    [TestFixture]
    public class GuiControlPropertyTests
    {
        // ── Opacity ────────────────────────────────────────────────────────────

        [Test]
        public void GivenNoOpacitySet_WhenGettingOpacity_ThenDefaultIsOne()
        {
            DummyControl control = new();

            Assert.That(control.Opacity, Is.EqualTo(1.0f));
        }

        [Test]
        public void GivenOpacitySetToZero_WhenGettingOpacity_ThenIsZero()
        {
            DummyControl control = new();
            control.Opacity = 0.0f;

            Assert.That(control.Opacity, Is.EqualTo(0.0f));
        }

        [Test]
        public void GivenOpacitySetToOne_WhenGettingOpacity_ThenIsOne()
        {
            DummyControl control = new();
            control.Opacity = 1.0f;

            Assert.That(control.Opacity, Is.EqualTo(1.0f));
        }

        [Test]
        public void GivenOpacitySetToHalf_WhenGettingOpacity_ThenIsHalf()
        {
            DummyControl control = new();
            control.Opacity = 0.5f;

            Assert.That(control.Opacity, Is.EqualTo(0.5f));
        }

        [Test]
        public void GivenOpacityAboveOne_WhenSet_ThenOpacityIsClampedToOne()
        {
            DummyControl control = new();
            control.Opacity = 6.13f;

            Assert.That(control.Opacity, Is.EqualTo(1.0f));
        }

        [Test]
        public void GivenOpacityBelowZero_WhenSet_ThenOpacityIsClampedToZero()
        {
            DummyControl control = new();
            control.Opacity = -8.73f;

            Assert.That(control.Opacity, Is.EqualTo(0.0f));
        }

        [Test]
        public void GivenOpacityJustAboveOne_WhenSet_ThenOpacityIsClampedToOne()
        {
            DummyControl control = new();
            control.Opacity = 1.0001f;

            Assert.That(control.Opacity, Is.EqualTo(1.0f));
        }

        [Test]
        public void GivenOpacityJustBelowZero_WhenSet_ThenOpacityIsClampedToZero()
        {
            DummyControl control = new();
            control.Opacity = -0.0001f;

            Assert.That(control.Opacity, Is.EqualTo(0.0f));
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
        public void GivenChildWithOwnOpacity_WhenParentHasDifferentOpacity_ThenChildOpacityTakesPrecedence()
        {
            DummyControl parent = new();
            DummyControl child = new();
            parent.Opacity = 0.3f;
            child.Opacity = 0.8f;
            child.Parent = parent;

            Assert.That(child.Opacity, Is.EqualTo(0.8f));
        }

        [Test]
        public void GivenThreeLevelParentChain_WhenGrandchildHasNoOpacity_ThenInheritsGrandparentOpacity()
        {
            DummyControl grandparent = new();
            DummyControl parent = new();
            DummyControl child = new();

            grandparent.Opacity = 0.6f;
            parent.Parent = grandparent;
            child.Parent = parent;

            Assert.That(child.Opacity, Is.EqualTo(0.6f));
        }

        [Test]
        public void GivenOpacityChanged_WhenFires_ThenFiresOpacityChangedEvent()
        {
            DummyControl control = new();
            bool eventFired = false;
            control.OpacityChanged += delegate { eventFired = true; };

            control.Opacity = 0.5f;

            Assert.That(eventFired);
        }

        [Test]
        public void GivenOpacitySetMultipleTimes_WhenChangedEachTime_ThenFiresEachTime()
        {
            DummyControl control = new();
            int fireCount = 0;
            control.OpacityChanged += delegate { fireCount++; };

            control.Opacity = 0.1f;
            control.Opacity = 0.5f;
            control.Opacity = 0.9f;

            Assert.That(fireCount, Is.EqualTo(3));
        }

        // ── Location ───────────────────────────────────────────────────────────

        [Test]
        public void GivenNoLocationSet_WhenGettingLocation_ThenReturnsEmpty()
        {
            DummyControl control = new();

            Assert.That(control.Location, Is.EqualTo(Point2D.Empty));
        }

        [Test]
        public void GivenLocationSet_WhenGettingLocation_ThenReturnsSetValue()
        {
            DummyControl control = new();
            control.Location = new Point2D(613, 873);

            Assert.That(control.Location, Is.EqualTo(new Point2D(613, 873)));
        }

        [Test]
        public void GivenLocationSet_WhenSettingLocation_ThenFiresLocationChanged()
        {
            DummyControl control = new();
            bool eventFired = false;
            control.LocationChanged += delegate { eventFired = true; };

            control.Location = new Point2D(100, 200);

            Assert.That(eventFired);
        }

        [Test]
        public void GivenNoParent_WhenGettingScreenLocation_ThenEqualsLocation()
        {
            DummyControl control = new();
            control.Location = new Point2D(613, 873);

            Assert.That(control.ScreenLocation, Is.EqualTo(new Point2D(613, 873)));
        }

        [Test]
        public void GivenParentWithLocation_WhenGettingChildScreenLocation_ThenEqualsSum()
        {
            DummyControl parent = new();
            DummyControl child = new();
            parent.Location = new Point2D(100, 200);
            child.Location = new Point2D(13, 73);
            child.Parent = parent;

            Assert.That(child.ScreenLocation.X, Is.EqualTo(113));
            Assert.That(child.ScreenLocation.Y, Is.EqualTo(273));
        }

        [Test]
        public void GivenThreeLevelParentChain_WhenGettingScreenLocation_ThenSumsAllLocations()
        {
            DummyControl grandparent = new();
            DummyControl parent = new();
            DummyControl child = new();

            grandparent.Location = new Point2D(10, 20);
            parent.Location = new Point2D(5, 15);
            child.Location = new Point2D(1, 2);

            parent.Parent = grandparent;
            child.Parent = parent;

            Assert.That(child.ScreenLocation.X, Is.EqualTo(16));
            Assert.That(child.ScreenLocation.Y, Is.EqualTo(37));
        }

        [Test]
        public void GivenControlAtOrigin_WhenGettingScreenLocation_ThenIsZeroZero()
        {
            DummyControl control = new();
            control.Location = Point2D.Empty;

            Assert.That(control.ScreenLocation.X, Is.EqualTo(0));
            Assert.That(control.ScreenLocation.Y, Is.EqualTo(0));
        }

        // ── Size ───────────────────────────────────────────────────────────────

        [Test]
        public void GivenNoSizeSet_WhenGettingSize_ThenReturnsEmpty()
        {
            DummyControl control = new();

            Assert.That(control.Size, Is.EqualTo(Size2D.Empty));
        }

        [Test]
        public void GivenSizeSet_WhenGettingSize_ThenReturnsSetValue()
        {
            DummyControl control = new();
            control.Size = new Size2D(613, 873);

            Assert.That(control.Size, Is.EqualTo(new Size2D(613, 873)));
        }

        [Test]
        public void GivenSizeSet_WhenSettingSize_ThenFiresSizeChanged()
        {
            DummyControl control = new();
            bool eventFired = false;
            control.SizeChanged += delegate { eventFired = true; };

            control.Size = new Size2D(100, 200);

            Assert.That(eventFired);
        }

        [Test]
        public void GivenParentWithSize_WhenChildHasNoSize_ThenChildInheritsParentSize()
        {
            DummyControl parent = new();
            DummyControl child = new();
            parent.Size = new Size2D(613, 873);
            child.Parent = parent;

            Assert.That(child.Size, Is.EqualTo(new Size2D(613, 873)));
        }

        [Test]
        public void GivenChildWithOwnSize_WhenParentHasDifferentSize_ThenChildSizeTakesPrecedence()
        {
            DummyControl parent = new();
            DummyControl child = new();
            parent.Size = new Size2D(1000, 2000);
            child.Size = new Size2D(613, 873);
            child.Parent = parent;

            Assert.That(child.Size, Is.EqualTo(new Size2D(613, 873)));
        }

        // ── ClientRectangle / DisplayRectangle ────────────────────────────────

        [Test]
        public void GivenLocationAndSize_WhenGettingClientRectangle_ThenMatchesLocationAndSize()
        {
            DummyControl control = new()
            {
                Location = new Point2D(10, 20),
                Size = new Size2D(100, 50)
            };

            Assert.That(control.ClientRectangle.Location, Is.EqualTo(new Point2D(10, 20)));
            Assert.That(control.ClientRectangle.Size, Is.EqualTo(new Size2D(100, 50)));
        }

        [Test]
        public void GivenLocationAndSize_WhenGettingDisplayRectangle_ThenMatchesScreenLocationAndSize()
        {
            DummyControl control = new()
            {
                Location = new Point2D(30, 40),
                Size = new Size2D(200, 100)
            };

            Assert.That(control.DisplayRectangle.Location, Is.EqualTo(control.ScreenLocation));
            Assert.That(control.DisplayRectangle.Size, Is.EqualTo(new Size2D(200, 100)));
        }

        [Test]
        public void GivenControlWithParent_WhenGettingDisplayRectangle_ThenUsesScreenLocation()
        {
            DummyControl parent = new();
            DummyControl child = new();
            parent.Location = new Point2D(50, 60);
            child.Location = new Point2D(10, 10);
            child.Size = new Size2D(80, 40);
            child.Parent = parent;

            Assert.That(child.DisplayRectangle.Location.X, Is.EqualTo(60));
            Assert.That(child.DisplayRectangle.Location.Y, Is.EqualTo(70));
        }

        // ── BackgroundColour inheritance ───────────────────────────────────────

        [Test]
        public void GivenBackgroundColourSet_WhenGettingBackgroundColour_ThenReturnsSetValue()
        {
            DummyControl control = new();
            control.BackgroundColour = Colour.ChromeYellow;

            Assert.That(control.BackgroundColour, Is.EqualTo(Colour.ChromeYellow));
        }

        [Test]
        public void GivenBackgroundColourSet_WhenSettingBackgroundColour_ThenFiresBackgroundColourChanged()
        {
            DummyControl control = new();
            bool eventFired = false;
            control.BackgroundColourChanged += delegate { eventFired = true; };

            control.BackgroundColour = Colour.ChromeYellow;

            Assert.That(eventFired);
        }

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
        public void GivenChildWithBackgroundColour_WhenParentHasDifferentColour_ThenChildColourTakesPrecedence()
        {
            DummyControl parent = new();
            DummyControl child = new();
            parent.BackgroundColour = Colour.Gold;
            child.BackgroundColour = Colour.ChromeYellow;
            child.Parent = parent;

            Assert.That(child.BackgroundColour, Is.EqualTo(Colour.ChromeYellow));
        }

        [Test]
        public void GivenThreeLevelParentChain_WhenOnlyGrandparentHasBackgroundColour_ThenGrandchildInheritsIt()
        {
            DummyControl grandparent = new();
            DummyControl parent = new();
            DummyControl child = new();
            grandparent.BackgroundColour = Colour.Red;
            parent.Parent = grandparent;
            child.Parent = parent;

            Assert.That(child.BackgroundColour, Is.EqualTo(Colour.Red));
        }

        // ── ForegroundColour inheritance ───────────────────────────────────────

        [Test]
        public void GivenForegroundColourSet_WhenGettingForegroundColour_ThenReturnsSetValue()
        {
            DummyControl control = new();
            control.ForegroundColour = Colour.Red;

            Assert.That(control.ForegroundColour, Is.EqualTo(Colour.Red));
        }

        [Test]
        public void GivenForegroundColourSet_WhenSettingForegroundColour_ThenFiresForegroundColourChanged()
        {
            DummyControl control = new();
            bool eventFired = false;
            control.ForegroundColourChanged += delegate { eventFired = true; };

            control.ForegroundColour = Colour.Gold;

            Assert.That(eventFired);
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
        public void GivenChildWithForegroundColour_WhenParentHasDifferentColour_ThenChildColourTakesPrecedence()
        {
            DummyControl parent = new();
            DummyControl child = new();
            parent.ForegroundColour = Colour.Gold;
            child.ForegroundColour = Colour.Red;
            child.Parent = parent;

            Assert.That(child.ForegroundColour, Is.EqualTo(Colour.Red));
        }

        // ── FontName inheritance ───────────────────────────────────────────────

        [Test]
        public void GivenFontNameSet_WhenGettingFontName_ThenReturnsSetValue()
        {
            DummyControl control = new();
            control.FontName = "MenuFont";

            Assert.That(control.FontName, Is.EqualTo("MenuFont"));
        }

        [Test]
        public void GivenFontNameSet_WhenSettingFontName_ThenFiresFontNameChanged()
        {
            DummyControl control = new();
            bool eventFired = false;
            control.FontNameChanged += delegate { eventFired = true; };

            control.FontName = "MenuFont";

            Assert.That(eventFired);
        }

        [Test]
        public void GivenParentWithFontName_WhenChildHasNoFontName_ThenChildInheritsParentFontName()
        {
            DummyControl parent = new();
            DummyControl child = new();
            parent.FontName = "MenuFont";
            child.Parent = parent;

            Assert.That(child.FontName, Is.EqualTo("MenuFont"));
        }

        [Test]
        public void GivenChildWithFontName_WhenParentHasDifferentFontName_ThenChildFontNameTakesPrecedence()
        {
            DummyControl parent = new();
            DummyControl child = new();
            parent.FontName = "ParentFont";
            child.FontName = "ChildFont";
            child.Parent = parent;

            Assert.That(child.FontName, Is.EqualTo("ChildFont"));
        }

        [Test]
        public void GivenThreeLevelParentChain_WhenOnlyGrandparentHasFontName_ThenGrandchildInheritsIt()
        {
            DummyControl grandparent = new();
            DummyControl parent = new();
            DummyControl child = new();
            grandparent.FontName = "GrandparentFont";
            parent.Parent = grandparent;
            child.Parent = parent;

            Assert.That(child.FontName, Is.EqualTo("GrandparentFont"));
        }

        // ── Parent visibility/enabled propagation ──────────────────────────────

        [Test]
        public void GivenParentHidden_WhenChildIsVisible_ThenChildIsVisibleIsFalse()
        {
            DummyControl parent = new();
            DummyControl child = new();
            child.Parent = parent;
            parent.Hide();

            Assert.That(child.IsVisible, Is.False);
        }

        [Test]
        public void GivenParentShown_WhenChildIsVisible_ThenChildIsVisibleIsTrue()
        {
            DummyControl parent = new();
            DummyControl child = new();
            child.Parent = parent;

            Assert.That(child.IsVisible);
        }

        [Test]
        public void GivenParentDisabled_WhenChildIsEnabled_ThenChildIsEnabledIsFalse()
        {
            DummyControl parent = new();
            DummyControl child = new();
            child.Parent = parent;
            parent.Disable();

            Assert.That(child.IsEnabled, Is.False);
        }

        [Test]
        public void GivenParentEnabled_WhenChildIsEnabled_ThenChildIsEnabledIsTrue()
        {
            DummyControl parent = new();
            DummyControl child = new();
            child.Parent = parent;

            Assert.That(child.IsEnabled);
        }

        [Test]
        public void GivenGrandparentHidden_WhenGrandchildIsVisible_ThenGrandchildIsVisibleIsFalse()
        {
            DummyControl grandparent = new();
            DummyControl parent = new();
            DummyControl child = new();
            parent.Parent = grandparent;
            child.Parent = parent;
            grandparent.Hide();

            Assert.That(child.IsVisible, Is.False);
        }

        [Test]
        public void GivenGrandparentDisabled_WhenGrandchildIsEnabled_ThenGrandchildIsEnabledIsFalse()
        {
            DummyControl grandparent = new();
            DummyControl parent = new();
            DummyControl child = new();
            parent.Parent = grandparent;
            child.Parent = parent;
            grandparent.Disable();

            Assert.That(child.IsEnabled, Is.False);
        }

        // ── Parent reference ───────────────────────────────────────────────────

        [Test]
        public void GivenFreshControl_WhenGettingParent_ThenIsNull()
        {
            DummyControl control = new();

            Assert.That(control.Parent, Is.Null);
        }

        [Test]
        public void GivenChildWithParent_WhenGettingParent_ThenReturnsParent()
        {
            DummyControl parent = new();
            DummyControl child = new();
            child.Parent = parent;

            Assert.That(child.Parent, Is.EqualTo(parent));
        }

        [Test]
        public void GivenChildWithParent_WhenParentIsReplaced_ThenNewParentIsUsed()
        {
            DummyControl firstParent = new();
            DummyControl secondParent = new();
            DummyControl child = new();

            child.Parent = firstParent;
            child.Parent = secondParent;

            Assert.That(child.Parent, Is.EqualTo(secondParent));
        }
    }
}
