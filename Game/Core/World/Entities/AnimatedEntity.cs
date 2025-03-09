using Godot;
using System;

public partial class AnimatedEntity : Entity
{
	[ExportGroup("Animation")]

	[Export] public String animationLibrary;

	[Export] public AnimationPlayer animationPlayer;

	[Export] public Sprite2D itemInHand;

    public void PlayAnimation(String animationName){
		animationPlayer.Play(animationLibrary+"/"+animationName);
	}
	public bool isAnimationFinished(){
		return animationPlayer.CurrentAnimation==""||animationPlayer.CurrentAnimation=="walk"||animationPlayer.CurrentAnimation=="idle";
	}
    public override void _PhysicsProcess(double delta)
    {
		if(isAnimationFinished()){
			if(Velocity.X>10||Velocity.X<-10)
				PlayAnimation("walk");
			else
				PlayAnimation("idle");
		}
		if(Velocity.X<-10){
			Scale=new Vector2(1,-1);
			RotationDegrees=180;
		}
		if(Velocity.X>10){
			Scale=new Vector2(1,1);
			RotationDegrees=0;
		}
		
        base._PhysicsProcess(delta);
    }
	public override float dealDamage(float damage,DamageTypes damageTypes,Node2D source,Node2D projectile){
		PlayAnimation("hurt");
		return base.dealDamage(damage,damageTypes,source,projectile);
	}

}

