using Godot;
using System;

public partial class QuitToMenuButton : Button
{
    public override void _Pressed()
    {
        GameWorld.Instance.QueueFree();
        ClientStatics.UI_Selector.ShownGUI_ID=5;

    }
}
