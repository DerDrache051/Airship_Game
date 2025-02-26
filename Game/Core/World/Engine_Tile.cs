using Godot;
using System;

public partial class Engine_Tile : Tile
{
	[Export] int EnginePower=400;

	public bool isActive;
	private bool wasActive;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(ParentGrid!=null && ParentGrid is Ship){
			Ship ParentShip=(Ship)ParentGrid;
			if(isActive && !wasActive)ParentShip.EnginePower+=EnginePower;
			if(!isActive && wasActive)ParentShip.EnginePower-=EnginePower;
		}
	}
    public override void onTileAdd()
    {
		isActive=true;
		wasActive=false;
        base.onTileAdd();
    }
	public override void onTileRemove(){
		if(ParentGrid!=null && ParentGrid is Ship){
			Ship ParentShip=(Ship)ParentGrid;
			if(isActive)ParentShip.EnginePower-=EnginePower;
		}
		base.onTileRemove();
	}
}
