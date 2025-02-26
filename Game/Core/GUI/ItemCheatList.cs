using Godot;
using System;

public partial class ItemCheatList : Godot.ItemList
{
	bool UpdateList = true;

	Item[] items;
	PackedScene[] itemScenes;
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
			itemScenes = new PackedScene[Registry.instance.Items.Length];
			items = new Item[Registry.instance.Items.Length];
			int itemNr = 0;
			Clear();
			for (int i = 0; i < Registry.instance.Items.Length; i++)
			{
				
				items[itemNr] = (Item)Registry.instance.Items[i].Instantiate<Item>();
				itemScenes[itemNr] = Registry.instance.Items[i];
				AddItem(items[itemNr].DisplayName, items[itemNr].ItemTexture);
				itemNr++;
			}

			UpdateList = false;
		}
		for(int i = 0; i < ItemCount; i++)
		{
			if (IsSelected(i))
			{
				ClientStatics.player.inventory.AddItem(itemScenes[i].Instantiate<Item>());
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