using Godot;
using System;

public partial class UI_Selector : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public int ShownGUI_ID=5;
	private int lastGUI=-1;
	public bool ShowCursor=true;
	public Control currentGUI;
	public Inventory GUI_Inventory;
	public override void _Ready()
	{
		ClientStatics.UI_Selector = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		updateGUI();	
	}
	public void updateGUI(){
			if(ShownGUI_ID!=lastGUI){
			if(currentGUI!=null)currentGUI.QueueFree();
			currentGUI=Registry.instance.UIs[ShownGUI_ID].Instantiate<Control>();
			AddChild(currentGUI);
			currentGUI.Visible=true;
			lastGUI=ShownGUI_ID;
		}
	}
}
