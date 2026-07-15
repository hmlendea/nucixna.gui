using System;

using NUnit.Framework;

using NuciXNA.Gui.Controls;
using NuciXNA.Gui.UnitTests.Helpers;

namespace NuciXNA.Gui.UnitTests.Controls
{
    [TestFixture]
    public class GuiControlLifecycleTests
    {
        // ── LoadContent ────────────────────────────────────────────────────────

        [Test]
        public void GivenFreshControl_WhenLoadContent_ThenIsContentLoadedIsTrue()
        {
            DummyControl control = new();
            control.LoadContent();

            Assert.That(control.IsContentLoaded);
        }

        [Test]
        public void GivenFreshControl_WhenLoadContent_ThenIsDisposedIsFalse()
        {
            DummyControl control = new();
            control.LoadContent();

            Assert.That(control.IsDisposed, Is.False);
        }

        [Test]
        public void GivenFreshControl_WhenLoadContent_ThenFiresContentLoading()
        {
            DummyControl control = new();
            bool eventFired = false;
            control.ContentLoading += delegate { eventFired = true; };

            control.LoadContent();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenFreshControl_WhenLoadContent_ThenFiresContentLoaded()
        {
            DummyControl control = new();
            bool eventFired = false;
            control.ContentLoaded += delegate { eventFired = true; };

            control.LoadContent();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenFreshControl_WhenLoadContent_ThenContentLoadingFiresBeforeContentLoaded()
        {
            DummyControl control = new();
            int loadingOrder = 0;
            int loadedOrder = 0;
            int sequence = 0;

            control.ContentLoading += delegate { loadingOrder = ++sequence; };
            control.ContentLoaded += delegate { loadedOrder = ++sequence; };

            control.LoadContent();

            Assert.That(loadingOrder, Is.LessThan(loadedOrder));
        }

        [Test]
        public void GivenLoadedControl_WhenLoadContentAgain_ThenThrowsInvalidOperationException()
        {
            DummyControl control = new();
            control.LoadContent();

            Assert.Throws<InvalidOperationException>(control.LoadContent);
        }

        // ── UnloadContent ──────────────────────────────────────────────────────

        [Test]
        public void GivenLoadedControl_WhenUnloadContent_ThenIsContentLoadedIsFalse()
        {
            DummyControl control = new();
            control.LoadContent();
            control.UnloadContent();

            Assert.That(control.IsContentLoaded, Is.False);
        }

        [Test]
        public void GivenFreshControl_WhenUnloadContent_ThenThrowsInvalidOperationException()
        {
            DummyControl control = new();

            Assert.Throws<InvalidOperationException>(control.UnloadContent);
        }

        [Test]
        public void GivenLoadedControl_WhenUnloadContent_ThenFiresContentUnloading()
        {
            DummyControl control = new();
            control.LoadContent();
            bool eventFired = false;
            control.ContentUnloading += delegate { eventFired = true; };

            control.UnloadContent();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedControl_WhenUnloadContent_ThenFiresContentUnloaded()
        {
            DummyControl control = new();
            control.LoadContent();
            bool eventFired = false;
            control.ContentUnloaded += delegate { eventFired = true; };

            control.UnloadContent();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedControl_WhenUnloadContent_ThenContentUnloadingFiresBeforeContentUnloaded()
        {
            DummyControl control = new();
            control.LoadContent();
            int unloadingOrder = 0;
            int unloadedOrder = 0;
            int sequence = 0;

            control.ContentUnloading += delegate { unloadingOrder = ++sequence; };
            control.ContentUnloaded += delegate { unloadedOrder = ++sequence; };

            control.UnloadContent();

            Assert.That(unloadingOrder, Is.LessThan(unloadedOrder));
        }

        [Test]
        public void GivenLoadedControl_WhenUnloadThenLoadAgain_ThenIsContentLoadedIsTrue()
        {
            DummyControl control = new();
            control.LoadContent();
            control.UnloadContent();
            control.LoadContent();

            Assert.That(control.IsContentLoaded);
        }

        [Test]
        public void GivenLoadedControl_WhenUnloadContent_ThenClearsParentReference()
        {
            DummyControl parent = new();
            DummyControl child = new();
            child.Parent = parent;
            child.LoadContent();

            child.UnloadContent();

            Assert.That(child.Parent, Is.Null);
        }

        // ── Dispose ────────────────────────────────────────────────────────────

        [Test]
        public void GivenLoadedControl_WhenDispose_ThenIsDisposedIsTrue()
        {
            DummyControl control = new();
            control.LoadContent();

            control.Dispose();

            Assert.That(control.IsDisposed);
        }

        [Test]
        public void GivenLoadedControl_WhenDispose_ThenIsContentLoadedIsFalse()
        {
            DummyControl control = new();
            control.LoadContent();

            control.Dispose();

            Assert.That(control.IsContentLoaded, Is.False);
        }

        [Test]
        public void GivenLoadedControl_WhenDispose_ThenFiresDisposing()
        {
            DummyControl control = new();
            control.LoadContent();
            bool eventFired = false;
            control.Disposing += delegate { eventFired = true; };

            control.Dispose();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedControl_WhenDispose_ThenFiresDisposed()
        {
            DummyControl control = new();
            control.LoadContent();
            bool eventFired = false;
            control.Disposed += delegate { eventFired = true; };

            control.Dispose();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenLoadedControl_WhenDispose_ThenDisposingFiresBeforeDisposed()
        {
            DummyControl control = new();
            control.LoadContent();
            int disposingOrder = 0;
            int disposedOrder = 0;
            int sequence = 0;

            control.Disposing += delegate { disposingOrder = ++sequence; };
            control.Disposed += delegate { disposedOrder = ++sequence; };

            control.Dispose();

            Assert.That(disposingOrder, Is.LessThan(disposedOrder));
        }

        [Test]
        public void GivenUnloadedControl_WhenDispose_ThenIsDisposedIsTrue()
        {
            DummyControl control = new();

            control.Dispose();

            Assert.That(control.IsDisposed);
        }

        [Test]
        public void GivenDisposedControl_WhenDisposeAgain_ThenDoesNotThrow()
        {
            DummyControl control = new();
            control.LoadContent();
            control.Dispose();

            Assert.DoesNotThrow(control.Dispose);
        }

        [Test]
        public void GivenDisposedControl_WhenDisposeAgain_ThenDisposedEventFiresOnlyOnce()
        {
            DummyControl control = new();
            control.LoadContent();
            int fireCount = 0;
            control.Disposed += delegate { fireCount++; };

            control.Dispose();
            control.Dispose();

            Assert.That(fireCount, Is.EqualTo(1));
        }

        // ── Focus / Unfocus ────────────────────────────────────────────────────

        [Test]
        public void GivenFreshControl_WhenFocus_ThenIsFocusedIsTrue()
        {
            DummyControl control = new();

            control.Focus();

            Assert.That(control.IsFocused);
        }

        [Test]
        public void GivenFocusedControl_WhenUnfocus_ThenIsFocusedIsFalse()
        {
            DummyControl control = new();
            control.Focus();

            control.Unfocus();

            Assert.That(control.IsFocused, Is.False);
        }

        [Test]
        public void GivenFreshControl_WhenFocus_ThenFiresFocused()
        {
            DummyControl control = new();
            bool eventFired = false;
            control.Focused += delegate { eventFired = true; };

            control.Focus();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenFreshControl_WhenUnfocus_ThenFiresUnfocused()
        {
            DummyControl control = new();
            bool eventFired = false;
            control.Unfocused += delegate { eventFired = true; };

            control.Unfocus();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenFocusedControlFocusedTwice_WhenCheckingFocusedFireCount_ThenFiresTwice()
        {
            DummyControl control = new();
            int fireCount = 0;
            control.Focused += delegate { fireCount++; };

            control.Focus();
            control.Focus();

            Assert.That(fireCount, Is.EqualTo(2));
        }

        // ── Enable / Disable ───────────────────────────────────────────────────

        [Test]
        public void GivenFreshControl_WhenGettingIsEnabled_ThenIsTrue()
        {
            DummyControl control = new();

            Assert.That(control.IsEnabled);
        }

        [Test]
        public void GivenEnabledControl_WhenDisable_ThenIsEnabledIsFalse()
        {
            DummyControl control = new();
            control.LoadContent();

            control.Disable();

            Assert.That(control.IsEnabled, Is.False);
        }

        [Test]
        public void GivenDisabledControl_WhenEnable_ThenIsEnabledIsTrue()
        {
            DummyControl control = new();
            control.LoadContent();
            control.Disable();

            control.Enable();

            Assert.That(control.IsEnabled);
        }

        [Test]
        public void GivenEnabledControl_WhenDisable_ThenFiresDisabled()
        {
            DummyControl control = new();
            control.LoadContent();
            bool eventFired = false;
            control.Disabled += delegate { eventFired = true; };

            control.Disable();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenDisabledControl_WhenEnable_ThenFiresEnabled()
        {
            DummyControl control = new();
            control.LoadContent();
            control.Disable();
            bool eventFired = false;
            control.Enabled += delegate { eventFired = true; };

            control.Enable();

            Assert.That(eventFired);
        }

        // ── Show / Hide ────────────────────────────────────────────────────────

        [Test]
        public void GivenFreshControl_WhenGettingIsVisible_ThenIsTrue()
        {
            DummyControl control = new();

            Assert.That(control.IsVisible);
        }

        [Test]
        public void GivenVisibleControl_WhenHide_ThenIsVisibleIsFalse()
        {
            DummyControl control = new();
            control.LoadContent();

            control.Hide();

            Assert.That(control.IsVisible, Is.False);
        }

        [Test]
        public void GivenHiddenControl_WhenShow_ThenIsVisibleIsTrue()
        {
            DummyControl control = new();
            control.LoadContent();
            control.Hide();

            control.Show();

            Assert.That(control.IsVisible);
        }

        [Test]
        public void GivenVisibleControl_WhenHide_ThenFiresHidden()
        {
            DummyControl control = new();
            control.LoadContent();
            bool eventFired = false;
            control.Hidden += delegate { eventFired = true; };

            control.Hide();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenHiddenControl_WhenShow_ThenFiresShown()
        {
            DummyControl control = new();
            control.LoadContent();
            control.Hide();
            bool eventFired = false;
            control.Shown += delegate { eventFired = true; };

            control.Show();

            Assert.That(eventFired);
        }

        // ── Id ─────────────────────────────────────────────────────────────────

        [Test]
        public void GivenFreshControl_WhenGettingId_ThenIdIsNotNullOrEmpty()
        {
            DummyControl control = new();

            Assert.That(control.Id, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void GivenTwoFreshControls_WhenGettingIds_ThenIdsAreDifferent()
        {
            DummyControl first = new();
            DummyControl second = new();

            Assert.That(first.Id, Is.Not.EqualTo(second.Id));
        }

        [Test]
        public void GivenManyFreshControls_WhenGettingIds_ThenAllIdsAreUnique()
        {
            System.Collections.Generic.HashSet<string> identifiers = [];

            for (int iteration = 0; iteration < 50; iteration += 1)
            {
                identifiers.Add(new DummyControl().Id);
            }

            Assert.That(identifiers.Count, Is.EqualTo(50));
        }

        [Test]
        public void GivenFreshControl_WhenSettingId_ThenIdIsUpdated()
        {
            DummyControl control = new();

            control.Id = "SolairOfAstora";

            Assert.That(control.Id, Is.EqualTo("SolairOfAstora"));
        }

        // ── Child disposal ─────────────────────────────────────────────────────

        [Test]
        public void GivenLoadedParentWithLoadedChild_WhenParentDisposed_ThenChildIsDisposed()
        {
            DummyControl parent = new();
            DummyControl child = new();
            parent.AddChild(child);
            parent.LoadContent();

            parent.Dispose();

            Assert.That(child.IsDisposed);
        }

        [Test]
        public void GivenLoadedParentWithMultipleChildren_WhenParentDisposed_ThenAllChildrenAreDisposed()
        {
            DummyControl parent = new();
            DummyControl firstChild = new();
            DummyControl secondChild = new();
            DummyControl thirdChild = new();
            parent.AddChild(firstChild);
            parent.AddChild(secondChild);
            parent.AddChild(thirdChild);
            parent.LoadContent();

            parent.Dispose();

            Assert.That(firstChild.IsDisposed);
            Assert.That(secondChild.IsDisposed);
            Assert.That(thirdChild.IsDisposed);
        }

        [Test]
        public void GivenLoadedParentWithLoadedChild_WhenParentDisposed_ThenChildDisposedEventFires()
        {
            DummyControl parent = new();
            DummyControl child = new();
            parent.AddChild(child);
            parent.LoadContent();
            bool childDisposedFired = false;
            child.Disposed += delegate { childDisposedFired = true; };

            parent.Dispose();

            Assert.That(childDisposedFired);
        }

        [Test]
        public void GivenUnloadedParentWithChild_WhenParentDisposed_ThenChildIsDisposed()
        {
            DummyControl parent = new();
            DummyControl child = new();
            parent.AddChild(child);

            parent.Dispose();

            Assert.That(child.IsDisposed);
        }
    }
}
