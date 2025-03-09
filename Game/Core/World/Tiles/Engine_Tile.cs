using Godot;
using System;

public partial class Engine_Tile : Tile
{
	[Export] int EnginePower = 400;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	public override void onTileAdd()
	{
		base.onTileAdd();
		if (ParentGrid is Ship ParentShip)
		{
			ParentShip.EnginePower += EnginePower;
		}


	}
	public override void onTileRemove()
	{
		if (ParentGrid is Ship ParentShip)
		{
			ParentShip.EnginePower -= EnginePower;
		}
		base.onTileRemove();
	}

}
