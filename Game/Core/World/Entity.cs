using System;
using Godot;

public partial class Entity:CharacterBody2D,IDamageable{

    [Export] public int maxHealth;
    [Export] public Node Team;
    [Export] public String DisplayName;

    [Export] public float PhysicalDamageMultiplier;//Piercing and Impact
    [Export] public float FireDamageMultiplier;
    [Export] public float ExplosionDamageMultiplier;
    [Export] public float ElectricDamageMultiplier;
    [Export] public float MagicDamageMultiplier;
    [Export] public float PoisonDamageMultiplier;
    [Export] public int DamageReduction;
    public int health;
    public Grid lastCollidedGrid;
    private Vector2 previousGridPosition;
    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();

        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            GodotObject collider = GetSlideCollision(i).GetCollider();
            if (collider is Grid grid)
            {
                if (lastCollidedGrid != grid)
                {
                    lastCollidedGrid = grid;
                    previousGridPosition = lastCollidedGrid.GlobalPosition;
                }
            }
        }

        if (lastCollidedGrid != null)
        {
            Vector2 gridMovement = lastCollidedGrid.GlobalPosition - previousGridPosition;
            GlobalPosition += gridMovement;
            previousGridPosition = lastCollidedGrid.GlobalPosition;
        }

        base._PhysicsProcess(delta);
    }
    public float dealDamage(float damage,DamageTypes damageTypes,Node2D source,Node2D projectile){
        if(damageTypes == DamageTypes.True)health-=(int)damage;
        if(health<=0){
            QueueFree();
        }
        return health;
    }

}