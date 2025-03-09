using Godot;
using Godot.Collections;
using System;

public partial class MeleWeapon : Item
{
	[Export] public float damage = 10;
	[Export] public DamageTypes damageType=DamageTypes.Impact;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public override void onPrimaryInteraction(Node node)
    {
        if (node == null) return;
		if (node is PlayerCharacter pc)
		{
			Entity ent=null;
			float distance=32f;
			Array<Node> nodes = GetTree().GetNodesInGroup("Entity");
			foreach (Node target in nodes)
			{
				if(target is Entity e&&e!=pc&&e.GlobalPosition.DistanceTo(pc.GlobalPosition)<20){
					if(e.GlobalPosition.DistanceTo(pc.CurserPosition)<distance){
						ent=e;
						distance=e.GlobalPosition.DistanceTo(pc.CurserPosition);
					}
				}
			}
			if(ent!=null)ent.dealDamage(damage,damageType,pc,pc);
		
		}	
		base.onPrimaryInteraction(node);
    }
}
