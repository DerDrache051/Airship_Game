using Godot;
using Godot.Collections;
using System;
using System.Threading;

public partial class RecipeType : Node
{
	[Export] public RecipeType extends;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public int getNumberOfPossibelRecipes(Inventory inventory){
		int count=0;
		Array<Node> Nodes =GetChildren();
		for(int i=0;i<Nodes.Count;i++){
			Recipe recipe = (Recipe)Nodes[i];
			if(recipe.isRecipePossible(inventory))count++;
		}
		if(extends!=null)count+=extends.getNumberOfPossibelRecipes(inventory);
		return count;
	}
	public Recipe[] getPossibleRecipes(Inventory inventory){
		Array<Node> Nodes =GetChildren();
		Recipe[] recipes=new Recipe[getNumberOfPossibelRecipes(inventory)];
		for(int i=0;i<Nodes.Count;i++){
			Recipe recipe = (Recipe)Nodes[i];
			if(recipe.isRecipePossible(inventory))recipes[i]=recipe;
		}
		if(extends!=null){
			extends.getPossibleRecipes(inventory);
		}
		return recipes;
	}
	public int getNumberOfRecipes(){
		Array<Node> Nodes =GetChildren();
		int count=Nodes.Count;
		if(extends!=null)count+=extends.getNumberOfRecipes();
		return count;
	}
	public Recipe[] getAllRecipes(){
		Array<Node> Nodes =GetChildren();
		Recipe[] recipes=new Recipe[getNumberOfRecipes()];
		for(int i=0;i<Nodes.Count;i++){
			Recipe recipe = (Recipe)Nodes[i];
			recipes[i]=recipe;
		}
		if(extends!=null){
			extends.getAllRecipes();
		}
		return recipes;
	}
}
