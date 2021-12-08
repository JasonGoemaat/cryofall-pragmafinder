## Trying to display as HUD

Looks like most HUD elements are in HUDLayoutControl, wouldn't want to modify that core game file...

But in BootstrapperClientGame.cs we have these:

    layoutRootChildren.Add(new HUDLayoutControl());
    layoutRootChildren.Add(new ChatPanel());

And in ClientComponentConsoleErrorsWatcher.cs


```cs
private static LogOverlayControl GetControl()
{
    if (ConsoleControl.Instance is not null
        && ConsoleControl.Instance.IsDisplayed)
    {
        // error will be logged into the console
        return null;
    }

    var instance = LogOverlayControl.Instance;
    if (instance is not null)
    {
        return instance;
    }

    instance = new LogOverlayControl();
    Api.Client.UI.LayoutRootChildren.Add(instance);
    return instance;
}
```

That is pretty nice because it is simple and has an ItemsControl...

