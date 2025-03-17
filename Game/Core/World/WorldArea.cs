using Godot;
using System;

public partial class WorldArea : Area2D
{
    public void removeBody(Node2D body)
    {
        body.QueueFree();
    }
}
