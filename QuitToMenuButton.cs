using Godot;
using System;

public partial class QuitToMenuButton : Button
{
    public override void _Pressed()
    {
        GameWorld.Instance.QueueFree();
        ClientStatics.UI_Selector.ShowGUI(ClientStatics.UI_Selector.MainMenu);

    }
}
