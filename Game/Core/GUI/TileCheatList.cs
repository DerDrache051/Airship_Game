using Godot;
using System;

public partial class TileCheatList : Godot.ItemList
{
	bool UpdateList = true;

	Item[] items;
	PackedScene[] TileScenes;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public delegate void OnItemSelected(int item);
	public event OnItemSelected onItemSelected;
	public override void _Process(double delta)
	{
		if (UpdateList)
		{
			TileScenes = new PackedScene[Registry.instance.Tiles.Length];
			items = new Item[Registry.instance.Tiles.Length];
			int itemNr = 0;
			Clear();
			for (int i = 0; i < Registry.instance.Tiles.Length; i++)
			{
				Tile tile = Registry.instance.Tiles[i].Instantiate<Tile>();
				items[itemNr] = tile.getTileItem();
				TileScenes[itemNr] = Registry.instance.Tiles[i];
				AddItem(items[itemNr].DisplayName, items[itemNr].ItemTexture);
				itemNr++;
			}

			UpdateList = false;
		}
		for(int i = 0; i < ItemCount; i++)
		{
			if (IsSelected(i))
			{
				Tile tile = Registry.instance.Tiles[i].Instantiate<Tile>();
				ClientStatics.player.inventory.AddItem(tile.getTileItem());
				Deselect(i);
			}
		}
	}




}
/*
			for (int i = 0; i < Registry.instance.Tiles.Length; i++)
			{
				
				Tile tile = (Tile)Registry.instance.Tiles[i].Instantiate<Tile>();
				if (tile == null) GD.PrintErr("Could not instantiate Tile");
				items[itemNr] = tile.getTileItem();
				AddItem(items[itemNr].DisplayName, items[itemNr].ItemTexture);
				itemNr++;
			}
*/