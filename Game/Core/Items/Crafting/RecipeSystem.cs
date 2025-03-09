using Godot;
using System;

public partial class RecipeSystem : Node
{
	public static RecipeSystem instance;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public RecipeType getRecipeType(String type){
		return instance.GetNode<RecipeType>(type);
	}
}
