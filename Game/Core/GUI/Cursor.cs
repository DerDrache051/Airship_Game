using Godot;
using System;

public partial class Cursor : Control
{
	[Export] public TextureRect[] CursorTextures;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Hidden;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(ClientStatics.player == null) { 
			SetCursor(CursorStates.DefaultState);
			return;
		}
		GlobalPosition = GetGlobalMousePosition();
		Item item = ClientStatics.player.inventory.GetItem(ClientStatics.player.SelectedItemSlot);
		if (item == null)SetCursor(CursorStates.DefaultState);
		else SetCursor(item.cursorState);
	}
	public void SetCursor(CursorStates state)
	{
		for (int i = 0; i < CursorTextures.Length; i++)
		{
			CursorTextures[i].Visible = (int)state == i;
		}

	}
}
public enum CursorStates
{
	DefaultState = 0,
	Attack = 1,
	Break = 2,
	Build = 3
}
