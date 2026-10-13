# GuiMenuListSelector

**Type:** `NuciXNA.Gui.Controls.GuiMenuListSelector`
**Base:** `GuiMenuItem`
**Source:** [`NuciXNA.Gui/Controls/GuiMenuListSelector.cs`](../../../NuciXNA.Gui/Controls/GuiMenuListSelector.cs)

## Purpose And State

`GuiMenuListSelector` copies key/value items into a dictionary, tracks one selected entry by dictionary enumeration index, renders its selected value, and offers cyclic keyboard/gamepad/mouse navigation.

| Property | Type | Semantics |
|----------|------|-----------|
| `Keys` | `IEnumerable<string>` | Current internal dictionary keys view |
| `Values` | `IEnumerable<string>` | Current internal dictionary values view |
| `SelectedIndex` | `int` | Initially `-1`; private setter |
| `SelectedKey` | `string` | Null before selection |
| `SelectedValue` | `string` | Null before selection |
| `ItemsCount` | `int` | Current dictionary count |

## Events

| Event | Payload | Timing |
|-------|---------|--------|
| `ItemsChanged` | `EventArgs.Empty` | After replacing the internal dictionary, before reset/automatic selection |
| `SelectedItemChanged` | Runtime `ListSelectionEventArgs` through `EventHandler` | After selected index/key/value change |

## Selection Methods

| Method | Behaviour |
|--------|-----------|
| `SelectNextItem()` | One item: no-op; otherwise increment and wrap to zero |
| `SelectPreviousItem()` | One item: no-op; otherwise decrement and wrap to last |
| `SelectItemByIndex(int)` | Equal current index: no-op before validation; otherwise validate, enumerate keys to index, assign selection, raise event |
| `SelectItemByKey(string)` | Equal current key: no-op; otherwise find key or throw `ArgumentOutOfRangeException` |
| `SelectItemByValue(string)` | Equal current value: no-op; otherwise find first equal value or throw `ArgumentOutOfRangeException` |
| `TrySelectItem(int)` | Suppresses `IndexOutOfRangeException` from index selection |
| `TrySelectItem(string)` | Treats argument as a value and suppresses `ArgumentOutOfRangeException` |

Empty collection edge: `SelectNextItem()` reaches index `0` and throws. `SelectPreviousItem()` computes `-1`, which equals the current index, so `SelectItemByIndex` returns before validation. Direct `SelectItemByIndex(-1)` has the same no-op behaviour while selected index is already `-1`.

## Item Methods

`SetItems` accepts:
- `params string[]` or `IEnumerable<string>`: generates a GUID-string key for every value.
- `params KeyValuePair<string,string>[]` or enumerable pairs.
- `IDictionary<string,string>`.

Every overload copies into a new dictionary. The dictionary overload raises `ItemsChanged`, resets selection fields, and selects index `0` when non-empty. `GetItems()` returns another defensive dictionary copy.

Duplicate pair keys fail through `ToDictionary`. Enumeration order defines index/key/value correspondence.

## Lifecycle, Display, And Input

Load calls the base hook, attempts index `0` for non-empty items, and subscribes selection input. Update calls the base hook, normalises a non-empty out-of-range index, then displays `Text + " : " + currentSelectionValue`.

- Focused keyboard Right/`D` and gamepad D-pad/thumbstick right select next.
- Focused keyboard Left/`A` and gamepad D-pad/thumbstick left select previous.
- Pressed left mouse button inside selects next; right selects previous.

## Tests And Change Impact

[`GuiMenuListSelectorTests.cs`](../../../NuciXNA.Gui.UnitTests/Controls/GuiMenuListSelectorTests.cs) and [`GuiMenuListSelectorExtendedTests.cs`](../../../NuciXNA.Gui.UnitTests/Controls/GuiMenuListSelectorExtendedTests.cs) cover substantial selection behaviour. Preserve event order, defensive copies, GUID-key generation, index semantics, and tolerant-overload scope. Add explicit empty-next/empty-previous and input-integration tests before changing edge behaviour.

## Related Documentation

- [ListSelectionEventArgs](./ListSelectionEventArgs.md)
- [GuiMenuItem](./GuiMenuItem.md)
- [Data Model](../../data-model.md)
