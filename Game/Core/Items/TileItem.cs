using Godot;
using System;

public partial class TileItem : Item
{

	[Export] public Tile tile;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void onSecondaryInteraction(Node node)
	{
		if (node is PlayerCharacter)
		{
			PlayerCharacter pc = (PlayerCharacter)node;
			if (pc.lastCollidedGrid == null) return;
			Vector2 placementPos = pc.CurserPosition - pc.lastCollidedGrid.Position;
			int[] tilepos = pc.lastCollidedGrid.getTilePos(placementPos);
			Grid grid = pc.lastCollidedGrid;
			//GD.Print(pc.lastCollidedGrid);
			if (grid.hasNeighbourTile(tile, tilepos[0], tilepos[1]))
			{
				//GD.Print(tilepos[0]+","+tilepos[1]);
				if (grid.addTile(tile.Clone(), tilepos[0], tilepos[1])) pc.inventory.ShrinkStack(pc.inventory.SelectedItem);
			}
		}
		base.onSecondaryInteraction(node);
	}
	public override bool isSame(Item item)
	{
		if(item is TileItem)
		{
			TileItem tileItem = (TileItem)item;
			if(!tile.isSame(tileItem.tile))return false;
		}
		return base.isSame(item);
	}

}
