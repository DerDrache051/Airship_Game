using Godot;
using System;

public partial class PositionIndicator : Label
{
    public override void _Process(double delta)
    {
        Text = $"Position: {ClientStatics.player.Position.Snapped(1)}";
    }
}
