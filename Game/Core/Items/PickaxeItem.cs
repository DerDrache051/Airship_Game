using Godot;
using System;

public partial class PickaxeItem : Item
{
	[Export] public float damage = 5;
	[Export] public float miningPower = 1;
	[Export] public float speed=1;
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
		if (node is PlayerCharacter)
		{
			PlayerCharacter pc = (PlayerCharacter)node;
			if (pc.lastCollidedGrid == null) return;
			Grid grid = pc.lastCollidedGrid;
			if (pc.GlobalPosition.DistanceTo(pc.CurserPosition) < 50) grid.damageTileAt(pc.CurserPosition, damage, DamageTypes.Impact, pc, pc);
		}
		base.onPrimaryInteraction(node);
	}
	public override void onSecondaryInteraction(Node node)
	{
		if (node is PlayerCharacter)
		{
			PlayerCharacter pc = (PlayerCharacter)node;
			if (pc.lastCollidedGrid == null) return;
			if (pc.GlobalPosition.DistanceTo(pc.CurserPosition) < 50)
			{
				Vector2 placementPos = pc.CurserPosition - pc.lastCollidedGrid.Position;
				int[] tilepos = pc.lastCollidedGrid.getTilePos(placementPos);
				Grid grid = pc.lastCollidedGrid;
				Tile tile=grid.getFirstTileAt(tilepos[0], tilepos[1]);
				if (tile != null){
				pc.inventory.AddItem(tile.getTileItem());
				grid.removeTile(tile);
				}
			}
		}
		base.onSecondaryInteraction(node);
	}
}
