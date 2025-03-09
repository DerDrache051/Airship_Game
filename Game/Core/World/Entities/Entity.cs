using System;
using Godot;

public partial class Entity:CharacterBody2D,IDamageable{

    [Export] public int maxHealth;
    [Export] public Node Team;
    [Export] public String DisplayName;

    [Export] public float PhysicalDamageMultiplier=1;//Piercing and Impact
    [Export] public float FireDamageMultiplier=1;
    [Export] public float ExplosionDamageMultiplier=1;
    [Export] public float ElectricDamageMultiplier=1;
    [Export] public float MagicDamageMultiplier=1;
    [Export] public float PoisonDamageMultiplier=1;
    [Export] public int DamageReduction=0;
    [Export] public bool isInvulnerable=false;
    [Export] public bool isUntargetable=false;

    public bool isOnCooldown;
    public int health;
    public Grid lastCollidedGrid;
    private Vector2 previousGridPosition;
    public override void _Ready()
    {
        health=maxHealth;
        base._Ready();
    }
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
                    
                }
            }
        }

        base._PhysicsProcess(delta);
    }
    public virtual float dealDamage(float damage,DamageTypes damageTypes,Node2D source,Node2D projectile){
        if(isInvulnerable&&damageTypes!=DamageTypes.True)return 0;
        float actualDamage=damage;
        if (damageTypes == DamageTypes.Explosion)actualDamage *= ExplosionDamageMultiplier;
        if (damageTypes == DamageTypes.Fire)actualDamage *= FireDamageMultiplier;
        if (damageTypes == DamageTypes.Electric)actualDamage *= ElectricDamageMultiplier;
        if (damageTypes == DamageTypes.Magic)actualDamage *= MagicDamageMultiplier;
        if (damageTypes == DamageTypes.Poison)actualDamage *= PoisonDamageMultiplier;
        if (damageTypes == DamageTypes.Piercing)actualDamage *= PhysicalDamageMultiplier;
        if (damageTypes == DamageTypes.Impact)actualDamage *= PhysicalDamageMultiplier;
        health-=(int)actualDamage;
        if(health<=0){
            QueueFree();
        }
        return actualDamage;
    }
    public void resetCooldown(){
        isOnCooldown=false;
    }
    public async void setCooldown(float time){
        isOnCooldown=true;
        await ToSignal(GetTree().CreateTimer(time, false, false, false),"timeout");
        isOnCooldown=false;
    }
}
