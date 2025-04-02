using Godot;
using System;

public partial class QuitButton2 : Button
{
    public override void _Pressed()
    {
        Registry.instance.GetTree().Quit(0);
    }
}
