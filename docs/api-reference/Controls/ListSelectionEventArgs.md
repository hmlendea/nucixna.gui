# ListSelectionEventArgs

**Type:** `NuciXNA.Gui.Controls.ListSelectionEventArgs`
**Base:** `EventArgs`
**Source:** [`NuciXNA.Gui/Controls/ListSelectionEventArgs.cs`](../../../NuciXNA.Gui/Controls/ListSelectionEventArgs.cs)

## Constructor And Properties

The primary constructor accepts `(int index, string key, string value)` and initialises three publicly mutable properties:

| Property | Type | Meaning |
|----------|------|---------|
| `SelectedIndex` | `int` | Current dictionary-enumeration index |
| `SelectedKey` | `string` | Current key |
| `SelectedValue` | `string` | Current value |

`GuiMenuListSelector.SelectedItemChanged` is declared as `EventHandler`, then invokes handlers with a `ListSelectionEventArgs` instance. Subscribers that need the fields must cast `EventArgs`.

The payload records only the new selection; it contains no previous index/key/value. Its mutable setters permit one subscriber to change values observed by later subscribers in the same synchronous event invocation.

## Tests And Change Impact

[`ListSelectionEventArgsTests.cs`](../../../NuciXNA.Gui.UnitTests/Controls/ListSelectionEventArgsTests.cs) covers construction/property values. Changing mutability, constructor order, or event delegate type is a public compatibility change.

## Related Documentation

- [GuiMenuListSelector](./GuiMenuListSelector.md)
- [Data Model](../../data-model.md)
