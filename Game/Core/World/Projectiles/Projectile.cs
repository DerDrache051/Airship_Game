using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Projectile : CharacterBody2D
{
    [ExportGroup("ProjectileProperties")]
    [Export] public Texture2D ProjectileTexture;
    [Export] public float ProjectileSpeed = 100;
    [Export] public float ProjectileRadius = 1;
    [Export] public float Gravity = 1;
    [Export] public float Shakyness = 1;
    [Export] public String DisplayName;
    [Export] public String ID;

    [ExportGroup("Damage")]
    [Export] public float ImpactDamage = 3;
    [Export] public float PiercingDamage = 0;
    [Export] public float ExplosionDamage = 0;
    [Export] public float FireDamage = 0;
    [Export] public float ElectricDamage = 0;
    [Export] public float MagicDamage = 0;
    [Export] public float PoisonDamage = 0;

    public Vector2 direction;
    public Node2D owner;
    public Node team;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");
        sprite.Texture = ProjectileTexture;
        sprite.Scale = new Vector2(ProjectileRadius, ProjectileRadius) / sprite.Texture.GetSize();
        CollisionShape2D collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        CircleShape2D shape = new ();
        shape.Radius = ProjectileRadius;
        collisionShape.Shape = shape;
        AddCollisionExceptionWith(owner);
    }
    public override void _PhysicsProcess(double delta)
    {
        Velocity += new Vector2(0, Gravity);
        if (Velocity.Length() > ProjectileSpeed) Velocity = Velocity.Normalized() * ProjectileSpeed;
        KinematicCollision2D collision = MoveAndCollide(Velocity);
        if (collision != null){
            if(collision.GetCollider() is IDamageable damageable){
                damageable.dealDamage(ImpactDamage, DamageTypes.Impact, owner, this);
                damageable.dealDamage(PiercingDamage, DamageTypes.Piercing, owner, this);
                damageable.dealDamage(ExplosionDamage, DamageTypes.Explosion, owner, this);
                damageable.dealDamage(FireDamage, DamageTypes.Fire, owner, this);
                damageable.dealDamage(ElectricDamage, DamageTypes.Electric, owner, this);
                damageable.dealDamage(MagicDamage, DamageTypes.Magic, owner, this);
                damageable.dealDamage(PoisonDamage, DamageTypes.Poison, owner, this);
                
            }
            QueueFree();
        }
    }
    public static void SpawnProjectile(PackedScene scene, Node2D owner, Vector2 position, Vector2 direction){
        Projectile projectile = (Projectile)scene.Instantiate();
        projectile.owner = owner;
        projectile.direction = direction;
        projectile.GlobalPosition = position;
        projectile.Velocity = direction * projectile.ProjectileSpeed;
        GameWorld.Instance.AddChild(projectile);
    }
}
