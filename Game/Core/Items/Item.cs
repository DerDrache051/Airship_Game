using Godot;
using System;

public partial class Item : Node
{
	[Export] public int MaxStackSize;
	[Export] public String ID;
	[Export] public String DisplayName;
	[Export] public Texture2D ItemTexture;
	[Export] public String CustomData;
	[Export] public CursorStates cursorState=CursorStates.DefaultState;
	public int StackSize=1;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public virtual void onPrimaryInteraction(Node node){}
	public virtual void onSecondaryInteraction(Node node){}

	public virtual bool isSame(Item item){
		return item!=null && item.ID==ID&& item.CustomData==CustomData;
	}
	public Item setLength(int length){
		StackSize=length;
		return this;
	}

}
