using Godot;
using System;

public partial class UiTile : Tile,IIntractable
{
	[Export] public PackedScene UI;
    // Called when the node enters the scene tree for the first time.

    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public virtual void interact(Node source)
	{
		ClientStatics.player.isInUI=true;
		ClientStatics.UI_Selector.ShowGUI(UI);
	}
	public virtual void endInteraction()
	{
	}
}
