using Godot;
using System;

public partial class WeightIndicator : Label
{
    public override void _Process(double delta)
    {
        Text = $"Weight: {ClientStatics.player.lastCollidedGrid.Weight}";
    }
}
