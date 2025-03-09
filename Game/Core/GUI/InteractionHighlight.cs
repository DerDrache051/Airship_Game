using Godot;
using System;

public partial class InteractionHighlight : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Node2D nearest=null;
		float nearestDistance=32;
		Godot.Collections.Array<Node> nodes = GetTree().GetNodesInGroup("Interaction");
		foreach (Node node in nodes)
		{
			if (node is Node2D)
			{
				Node2D node2D = (Node2D)node;
				if (node2D.GlobalPosition.DistanceTo(ClientStatics.player.GlobalPosition) < nearestDistance)
				{
					nearestDistance = node2D.GlobalPosition.DistanceTo(ClientStatics.player.GlobalPosition);
					nearest = node2D;
				}
			}
		}
		if (nearest != null)
		{
			GlobalPosition = nearest.GlobalPosition;
		}
		Visible = nearest != null;
	}




}
