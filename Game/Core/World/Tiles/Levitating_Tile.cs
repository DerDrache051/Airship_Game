using Godot;


public partial class Levitating_Tile : Tile
{
	
	[Export] int LevitationPower=400;

	public bool isActive;
	private bool wasActive;

    public override void _Ready()
    {
        base._Ready();
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if(ParentGrid!=null && ParentGrid is Ship){
			Ship ParentShip=(Ship)ParentGrid;
			if(isActive && !wasActive)ParentShip.LevitationPower+=LevitationPower;
			if(!isActive && wasActive)ParentShip.LevitationPower-=LevitationPower;
			wasActive=isActive;
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
			if(isActive)ParentShip.LevitationPower-=LevitationPower;
		}
		base.onTileRemove();
	}
}
