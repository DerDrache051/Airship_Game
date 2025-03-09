using Godot;
using System;

public partial class InventoryTile : UiTile
{
	[Export] public Inventory inventory;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public override void interact(Node source)
    {
		ClientStatics.UI_Selector.GUI_Inventory=inventory;
        base.interact(source);
    }

}
