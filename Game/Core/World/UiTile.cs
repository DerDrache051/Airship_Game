using Godot;
using System;

public partial class UiTile : Tile,IIntractable
{
	[Export] public int UI_ID;
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
		ClientStatics.UI_Selector.ShownGUI_ID=UI_ID;
		ClientStatics.UI_Selector.updateGUI();
	}
	public virtual void endInteraction()
	{
		ClientStatics.UI_Selector.ShownGUI_ID=0;
	}
}
