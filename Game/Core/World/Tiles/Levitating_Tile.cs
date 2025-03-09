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

	}
	public override void onTileAdd()
	{
		base.onTileAdd();
		if (ParentGrid is Ship ParentShip)
		{
			ParentShip.LevitationPower += LevitationPower;
		}


	}
	public override void onTileRemove()
	{
		if (ParentGrid is Ship ParentShip)
		{
			ParentShip.LevitationPower -= LevitationPower;
		}
		base.onTileRemove();
	}
}
