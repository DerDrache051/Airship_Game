using Godot;
using System;
using System.Runtime.Serialization;

public partial class CraftingRecipeList : ItemList
{
	// Called when the node enters the scene tree for the first time.
	Recipe[] Recipes;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		for(int i = 0; i < ItemCount; i++)
		{
			if (IsSelected(i))
			{
				Recipes[i].useRecipe(ClientStatics.UI_Selector.GUI_Inventory);
				Deselect(i);
			}
		}
		LockUncraftableRecipes();
	}
	public void WriteList(String RecipeType){
		RecipeType rtype=RecipeSystem.instance.getRecipeType(RecipeType);
		if(rtype==null){
			GD.PrintErr("Unknown recipe type: "+RecipeType);
			return;
		}
		Recipes=rtype.getAllRecipes();
		if(Recipes.Length==0){
			GD.PrintErr("No recipes found for type: "+RecipeType);
			return;
		}
		Clear();
		for(int i=0;i<Recipes.Length;i++){
			AddItem(Recipes[i].RecipeName,Recipes[i].RecipeTexture);
		}
		LockUncraftableRecipes();
	}
	public void LockUncraftableRecipes(){
		for(int i=0;i<Recipes.Length;i++){
			SetItemDisabled(i,!Recipes[i].isRecipePossible(ClientStatics.UI_Selector.GUI_Inventory));
		}
	}

}
