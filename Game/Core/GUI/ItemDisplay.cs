using Godot;
using System;

public partial class ItemDisplay : TextureRect
{
	[Export] int slot;
	[Export] bool PlayerInventorySlot;
	Label itemCountDisplayLabel;
	TextureRect itemDisplayRect;
	
	public Inventory inv;

	bool isBeingDragged = false;
	ReferenceRect referenceRect;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("ItemContainer");
		if (PlayerInventorySlot) inv = ClientStatics.player.inventory;
		else inv=ClientStatics.UI_Selector.GUI_Inventory;
		itemCountDisplayLabel = GetNode<Label>("Label");
		itemDisplayRect = GetNode<TextureRect>("TextureRect");
		referenceRect = GetNode<ReferenceRect>("ReferenceRect");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(inv==null)return;
		if (inv.Items[slot] != null)
		{
			itemDisplayRect.Texture = inv.Items[slot].ItemTexture;
			itemCountDisplayLabel.Text = inv.Items[slot].StackSize.ToString();
		}
		else
		{
			itemDisplayRect.Texture = null;
			itemCountDisplayLabel.Text = "";
		}

		referenceRect.Visible = inv.SelectedItem == slot && PlayerInventorySlot;

		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			if (isBeingDragged)
			{
				itemDisplayRect.GlobalPosition = GetGlobalMousePosition();
			}
		}
		else
		{
			if (isBeingDragged)
			{
				GD.Print("Placing the item");
				Godot.Collections.Array<Node> nodes = GetTree().GetNodesInGroup("ItemContainer");
				foreach (Node node in nodes)
				{
				
					if (node is ItemDisplay itemDisplay)
					{
						if (GetGlobalMousePosition().DistanceTo(itemDisplay.GlobalPosition+itemDisplay.GetRect().Size/2) < itemDisplay.GetRect().Size.X / 1.7)
						{
							Item swap = itemDisplay.inv.Items[itemDisplay.slot];
							itemDisplay.inv.Items[itemDisplay.slot] = inv.Items[slot];
							inv.Items[slot] = swap;
							break;
						}
					}
				}
			}
			isBeingDragged = false;
			itemDisplayRect.GlobalPosition = GlobalPosition + new Vector2(2, 2);
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
			{
				if (GetGlobalMousePosition().DistanceTo(GlobalPosition+GetRect().Size/2) < itemDisplayRect.GetRect().Size.X / 1.7)
				{
					isBeingDragged = true;
					
				}
			}
		}
		base._Input(@event);
	}
}