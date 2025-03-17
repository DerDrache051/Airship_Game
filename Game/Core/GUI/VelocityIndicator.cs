using Godot;
using System;

public partial class VelocityIndicator : Label
{
    public override void _Process(double delta)
    {
        Text = $"Velocity: {ClientStatics.player.Velocity}";
    }
}
