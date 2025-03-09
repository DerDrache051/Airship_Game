using Godot;
using Godot.Collections;
using System;

public partial class SelectPlayerAsTarget : EntityBehavior
{
	// Called when the node enters the scene tree for the first time.

    public override Vector2 getDesiredVelocity()
    {
        return Vector2.Zero;
    }

    public override bool shouldJump()
    {
        return false;
    }

    public override bool isFinished()
    {
        return true;
    }

    public override bool isPossible()
    {
		Array<Node> nodes = GetTree().GetNodesInGroup("Player");
        float dist=120;
		foreach (Node node in nodes)
		{
			if (node is PlayerCharacter)
			{
				PlayerCharacter player = (PlayerCharacter)node;
				if (player.GlobalPosition.DistanceTo(controlledEntity.GlobalPosition) < dist && !player.isUntargetable)
				{
					dist = player.GlobalPosition.DistanceTo(controlledEntity.GlobalPosition);
				}
			}
		}
		return dist<120;
    }

    public override void doBehavior()
    {
		Array<Node> nodes = GetTree().GetNodesInGroup("Player");
        float dist=120;
		foreach (Node node in nodes)
		{
			if (node is PlayerCharacter)
			{
				PlayerCharacter player = (PlayerCharacter)node;
				if (player.GlobalPosition.DistanceTo(controlledEntity.GlobalPosition) < dist && !player.isUntargetable)
				{
					controlledEntity.TargetEntity = player;
					dist = player.GlobalPosition.DistanceTo(controlledEntity.GlobalPosition);
				}
			}
		}
    }

}
