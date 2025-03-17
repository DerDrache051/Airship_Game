using Godot;
using Godot.Collections;
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
    public override void DeserializeComponents(Dictionary<string, string> dict)
    {
		base.DeserializeComponents(dict);
		PackedScene scene=GD.Load<PackedScene>(dict["TileScene"]);
		tile=scene.Instantiate<Tile>();
		DisplayName=tile.DisplayName;
		SceneFilePath="res://Game/Core/Items/tile_item.tscn";
		ID=tile.ID;
		MaxStackSize=16;
		StackSize=1;
		if(tile.itemTexture!=null)
			ItemTexture=tile.itemTexture;
		else if(tile.ForegroundDecorationLayer!=null)
			ItemTexture=tile.ForegroundDecorationLayer;
		else if(tile.CollisionLayer!=null)
			ItemTexture=tile.CollisionLayer;
		else if(tile.InBetweenDecorationLayer!=null)
			ItemTexture=tile.InBetweenDecorationLayer;
		else if(tile.InteractionLayer!=null)
			ItemTexture=tile.InteractionLayer;
		else if(tile.ConnectionLayer!=null)
			ItemTexture=tile.ConnectionLayer;
		else if(tile.BackgroundDecorationLayer!=null)
			ItemTexture=tile.BackgroundDecorationLayer;
		else if(tile.BackgroundLayer!=null)
			ItemTexture=tile.BackgroundLayer;
		else if(tile.BehindShipMapDecorationLayer!=null)
			ItemTexture=tile.BehindShipMapDecorationLayer;
		else if(tile.BehindShipMapLayer!=null)
			ItemTexture=tile.BehindShipMapLayer;
		
		cursorState=CursorStates.Build;

    }
	public override Dictionary<string, string> SerializeComponents(Dictionary<string, string> dict)
	{
		dict.Add("TileScene",tile.SceneFilePath);
		base.SerializeComponents(dict);
		return dict;
	}

}
