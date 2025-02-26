using Godot;
using System;

public partial class Recipe : Node
{
	[Export] public String[] inputItemsIDs;
	[Export] public int[] inputItemQuantities;
	[Export] public PackedScene[] outputItemIDs;
	[Export] public int[] outputItemQuantities;
	[Export] public float Time;
	[Export] public Texture2D RecipeTexture;
	[Export] public String RecipeName;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public bool isRecipePossible(Inventory inventory){
		for(int i=0;i<inputItemsIDs.Length;i++){
			if(inventory.getItemCount(inputItemsIDs[i])<inputItemQuantities[i])return false;
		}
		return inventory.getEmptySlotCount()>=outputItemQuantities.Length;
	}
	public void useRecipe(Inventory inventory){
		for(int i=0;i<inputItemsIDs.Length;i++){
			inventory.removeItems(inputItemsIDs[i],inputItemQuantities[i]);
		}
		for(int i=0;i<outputItemQuantities.Length;i++){
			Node item = outputItemIDs[i].Instantiate();
			if(item is Item){
				((Item)item).StackSize=outputItemQuantities[i];
				inventory.AddItem((Item)item);
			}
			if(item is Tile){
				Item tileitem=((Tile)item).getTileItem();
				tileitem.StackSize=outputItemQuantities[i];
				inventory.AddItem(tileitem);
			}
		}
	}
}
