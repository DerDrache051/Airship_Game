using Godot;
using System;

public partial class EntityController : Node
{
	[Export] public EntityBehavior[] Behaviors;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public Vector2 getDesiredVelocity(){
		return Vector2.Zero;
	}
	public bool shouldJump(){
		return false;
	}
}
