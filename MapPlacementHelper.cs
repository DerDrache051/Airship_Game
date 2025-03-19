using Godot;
using System;

public partial class MapPlacementHelper : Node2D
{

    public PackedScene scene;
    public void _Place(){
        GD.Print("placing");
        Node node = scene.Instantiate();
        if(node is Node2D node2d)node2d.GlobalPosition = GlobalPosition;
        GameWorld.Instance.AddChild(node);
        QueueFree();
    }
}
