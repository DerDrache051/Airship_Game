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
		DisplayName=tile.Tilematerial.DisplayName;
		SceneFilePath="res://Game/Core/Items/tile_item.tscn";
		ID=tile.Tilematerial.ID;
		MaxStackSize=16;
		StackSize=1;
		if(tile.Tilematerial.itemTexture!=null)
			ItemTexture=tile.Tilematerial.itemTexture;
		else if(tile.Tilematerial.ForegroundDecorationLayer!=null)
			ItemTexture=tile.Tilematerial.ForegroundDecorationLayer;
		else if(tile.Tilematerial.CollisionLayer!=null)
			ItemTexture=tile.Tilematerial.CollisionLayer;
		else if(tile.Tilematerial.InBetweenDecorationLayer!=null)
			ItemTexture=tile.Tilematerial.InBetweenDecorationLayer;
		else if(tile.Tilematerial.InteractionLayer!=null)
			ItemTexture=tile.Tilematerial.InteractionLayer;
		else if(tile.Tilematerial.ConnectionLayer!=null)
			ItemTexture=tile.Tilematerial.ConnectionLayer;
		else if(tile.Tilematerial.BackgroundDecorationLayer!=null)
			ItemTexture=tile.Tilematerial.BackgroundDecorationLayer;
		else if(tile.Tilematerial.BackgroundLayer!=null)
			ItemTexture=tile.Tilematerial.BackgroundLayer;
		else if(tile.Tilematerial.BehindShipMapDecorationLayer!=null)
			ItemTexture=tile.Tilematerial.BehindShipMapDecorationLayer;
		else if(tile.Tilematerial.BehindShipMapLayer!=null)
			ItemTexture=tile.Tilematerial.BehindShipMapLayer;
		
		cursorState=CursorStates.Build;

    }
	public override Dictionary<string, string> SerializeComponents(Dictionary<string, string> dict)
	{
		dict.Add("TileScene",tile.SceneFilePath);
		base.SerializeComponents(dict);
		return dict;
	}

}
