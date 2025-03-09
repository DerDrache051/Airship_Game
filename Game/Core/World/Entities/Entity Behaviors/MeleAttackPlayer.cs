using Godot;
using System;
using System.Runtime;

public partial class MeleAttackPlayer : EntityBehavior
{

	[Export] public float ImpactDamage=2;
	[Export] public float Cooldown=0.2f;


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
        return !controlledEntity.isOnCooldown;
    }

    public override bool isPossible()
    {
		if(controlledEntity.TargetEntity==null)return false;
		return controlledEntity.GlobalPosition.DistanceTo(controlledEntity.TargetEntity.GlobalPosition) <20;
    }

    public override void doBehavior()
    {
		if(controlledEntity.isOnCooldown)return;
		controlledEntity.setCooldown(Cooldown);
        controlledEntity.TargetEntity.dealDamage(ImpactDamage,DamageTypes.Impact,controlledEntity,controlledEntity);
    }

}
