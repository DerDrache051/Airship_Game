using Godot;
using System;

public partial class LivingEntity : AnimatedEntity
{
	[Export] public float Speed;
	[Export] public bool canFly;
	[Export] public float JumpVelocity;
	[Export] public EntityController EntityController;

	float gravity;

	public override void _Ready()
	{
		gravity=ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		if(EntityController==null)return;
		if(canFly){
			Velocity=EntityController.getDesiredVelocity();
		}
		else{
			if(IsOnFloor()){
				float y=0;
				if(EntityController.shouldJump())y=JumpVelocity;
				Velocity=new(EntityController.getDesiredVelocity().X,y);
			}
			else Velocity=new(EntityController.getDesiredVelocity().X,Velocity.Y+gravity);
		}
		base._PhysicsProcess(delta);
	}
}
