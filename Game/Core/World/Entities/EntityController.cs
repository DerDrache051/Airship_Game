using Godot;
using System;

public partial class EntityController : Node
{
	[Export] public EntityBehavior[] Behaviors;

	public EntityBehavior currentBehavior;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (currentBehavior!=null && currentBehavior.isFinished())
		{
			currentBehavior = null;
		}

		if (currentBehavior == null)
		{
			for (int i = 0; i < Behaviors.Length; i++)
			{
				if (Behaviors[i].isPossible())
				{
					currentBehavior = Behaviors[i];
					break;
				}
			}
		}
		if (currentBehavior == null) return;
		currentBehavior.doBehavior();

	}
	public Vector2 getDesiredVelocity()
	{
		if (currentBehavior == null)
			return Vector2.Zero;
		return currentBehavior.getDesiredVelocity();
	}
	public bool shouldJump()
	{
		if(currentBehavior == null) return false;
		return currentBehavior.shouldJump();
	}
}
