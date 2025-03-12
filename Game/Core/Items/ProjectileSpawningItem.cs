using Godot;
using System;

public partial class ProjectileSpawningItem : Item
{
    [Export] public PackedScene ProjectileScene;
    public override void onPrimaryInteraction (Node node)
    {
        if (node is PlayerCharacter player)
        {
            Projectile.SpawnProjectile(ProjectileScene,player,player.GlobalPosition,player.GlobalPosition.DirectionTo(player.CurserPosition));
        }
    }
}
