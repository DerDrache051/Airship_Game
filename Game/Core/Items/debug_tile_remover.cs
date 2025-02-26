using Godot;
using System;

public partial class debug_tile_remover : Item
{
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
		if(node is PlayerCharacter){
			PlayerCharacter pc=(PlayerCharacter)node;
			if (pc.lastCollidedGrid==null)return;
			Vector2 placementPos=pc.CurserPosition-pc.lastCollidedGrid.Position;
			int[] tilepos=pc.lastCollidedGrid.getTilePos(placementPos);
			Grid grid=pc.lastCollidedGrid;
			GD.Print(pc.lastCollidedGrid);

				GD.Print(tilepos[0]+","+tilepos[1]);
				grid.onTileDestroyed(grid.getTileAt(tilepos[0],tilepos[1],GridLayer.CollisionLayer));
				StackSize-=1;
		}
        base.onSecondaryInteraction(node);
    }
}
