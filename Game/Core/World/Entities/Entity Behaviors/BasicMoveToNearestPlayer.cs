using Godot;
using Godot.Collections;
using System;

public partial class BasicMoveToNearestPlayer : EntityBehavior{


    public override Vector2 getDesiredVelocity()
    {
        if(controlledEntity.TargetEntity==null)return Vector2.Zero;
        return controlledEntity.TargetEntity.GlobalPosition-controlledEntity.GlobalPosition;
    }

    public override bool shouldJump()
    {
        return controlledEntity.IsOnWall();
    }

    public override bool isFinished()
    {
        return true;
    }

    public override bool isPossible()
    {
        return controlledEntity.TargetEntity != null;
    }

    public override void doBehavior()
    {
        ;
    }

}
