using Godot;
using System;

public partial class UI_Selector : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.

	[Export] public PackedScene MainMenu;
	[Export] public PackedScene InventoryGUI;
	[Export] public PackedScene InGameOverlay;
	[Export] public PackedScene DeathScreen;
	[Export] public PackedScene Settings;

	[Export] public PackedScene LoadingScreen;

	public bool ShowCursor=true;
	[Export] public Control currentGUI;
	public Inventory GUI_Inventory;
	public override void _Ready()
	{
		ClientStatics.UI_Selector = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		
	}
	public void ShowGUI(PackedScene GUI){
		if(currentGUI!=null){
			currentGUI.QueueFree();
		}
		currentGUI=GUI.Instantiate<Control>();
		currentGUI.Visible=true;
		AddChild(currentGUI);
	}
	
}
