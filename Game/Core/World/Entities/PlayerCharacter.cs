using Godot;
using Godot.Collections;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

public partial class PlayerCharacter : AnimatedEntity
{

	public const float Speed = 100.0f;
	public const float JumpVelocity = -400.0f;

	public Vector2 direction;
	public bool controlPlayer = true;
	public bool isInUI = false;
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public Vector2 CurserPosition;


	[MaybeNull] public IIntractable currentInteraction;



	public int SelectedItemSlot = 1;

	public Inventory inventory;
	public PlayerCharacter()
	{
		if (ClientStatics.player == null) ClientStatics.player = this;
	}
	public override void _Ready()
	{
		inventory = GetNode<Inventory>("Inventory");
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		this.FloorStopOnSlope = true;
		this.FloorMaxAngle=Mathf.DegToRad(60);
		CurserPosition = GetGlobalMousePosition();
		//Movement
		direction = Input.GetVector("Character_Left", "Character_Right", "Character_Down", "Character_Up");
		Vector2 velocity = Velocity;
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;
		if (controlPlayer)
		{
			if (Input.IsActionJustPressed("Character_Jump") && IsOnFloor())
				velocity.Y = JumpVelocity;

			if (direction != Vector2.Zero)
			{
				velocity.X = direction.X * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			}
		}
		else velocity.X = 0;
		Velocity = velocity;
		//passing through semi solids
		if (lastCollidedGrid != null && direction.Y < 0)
		{
			int[] tilepos = lastCollidedGrid.getTilePos(new Vector2(GlobalPosition.X, GlobalPosition.Y+16)-lastCollidedGrid.GlobalPosition);
			Tile tile = lastCollidedGrid.getTileAt(tilepos[0], tilepos[1], GridLayer.CollisionLayer);
			//if(tile!=null)GD.Print(tile.SceneFilePath);
			if (tile != null && tile.Tilematerial.SemiSolid)
			{
				GlobalPosition = new Vector2(GlobalPosition.X, GlobalPosition.Y+2);
			}
		}
				if (lastCollidedGrid != null && direction.Y > 0)
		{
			int[] tilepos = lastCollidedGrid.getTilePos(new Vector2(GlobalPosition.X, GlobalPosition.Y)-lastCollidedGrid.GlobalPosition);
			Tile tile = lastCollidedGrid.getTileAt(tilepos[0], tilepos[1], GridLayer.InteractionLayer);
			//if(tile!=null)GD.Print(tile.SceneFilePath);
			if (tile != null && tile.Tilematerial.canBeClimbed)
			{
				Velocity=new Vector2(Velocity.X,direction.Y*-Speed);
			}
		}
		//Interaction
		if (Input.IsActionJustPressed("Interact"))
		{
			IIntractable target = null;
			float targetdist = 33;
			Array<Node> nodes = GetTree().GetNodesInGroup("Interaction");
			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i] is IIntractable && nodes[i] is Node2D)
				{
					Node2D node2D = (Node2D)nodes[i];
					float dist = node2D.GlobalPosition.DistanceTo(GlobalPosition);
					if (dist < targetdist)
					{
						target = (IIntractable)node2D;
						targetdist = dist;
					}
				}
			}
			if (target != null)
			{
				currentInteraction = target;
				target.interact(this);
			}
		}
		;
		//End_Interaction
		if (Input.IsActionJustPressed("UseItemPrimary"))
		{
			PlayAnimation("attack");
			if (inventory.GetItem(SelectedItemSlot) != null)
				inventory.GetItem(SelectedItemSlot).onPrimaryInteraction(this);
		}
		if (Input.IsActionJustPressed("UseItemSecondary"))
		{
			PlayAnimation("interact");
			if (inventory.GetItem(SelectedItemSlot) != null)
				inventory.GetItem(SelectedItemSlot).onSecondaryInteraction(this);
		}
		if (Input.IsActionJustPressed("next_item"))
		{
			SelectedItemSlot += 1;
			if (SelectedItemSlot > 9) SelectedItemSlot = 0;
		}
		if (Input.IsActionJustPressed("previous_item"))
		{
			SelectedItemSlot -= 1;
			if (SelectedItemSlot < 0) SelectedItemSlot = 9;
		}
		if (!isInUI) inventory.SelectedItem = SelectedItemSlot;
		if (inventory.GetItem(SelectedItemSlot) != null)
		{
			itemInHand.Texture = inventory.GetItem(SelectedItemSlot).ItemTexture; itemInHand.Visible = true;
		}
		else
		{
			itemInHand.Texture = null;
		}
		if (Input.IsActionJustPressed("Inventory"))
		{
			if (isInUI)
			{
				ClientStatics.UI_Selector.ShowGUI(ClientStatics.UI_Selector.InGameOverlay);
				isInUI = false;
			}
			else
			{
				ClientStatics.UI_Selector.ShowGUI(ClientStatics.UI_Selector.InventoryGUI);
				isInUI = true;
			}

		}
		if (Input.IsActionJustPressed("Exit_UI") && isInUI)
		{
			if (currentInteraction != null) { currentInteraction.endInteraction(); currentInteraction = null; }
			ClientStatics.UI_Selector.ShowGUI(ClientStatics.UI_Selector.InGameOverlay);
			isInUI = false;
		}
		base._PhysicsProcess(delta);
	}
	public override void _ExitTree()
	{
		ClientStatics.UI_Selector.ShowGUI(ClientStatics.UI_Selector.DeathScreen);
		base._ExitTree();
	}

}

public enum PermissionLevels
{
	None = 0,
	Cheats = 1,
	Moderator = 2,
	Admin = 3,
	Console = 4
}
