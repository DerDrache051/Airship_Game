using Godot;
using Godot.Collections;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

public partial class PlayerCharacter : Entity
{
	public const float Speed = 100.0f;
	public const float JumpVelocity = -200.0f;

	public Vector2 direction;
	public bool controlPlayer=true;
	public bool isInUI=false;
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public Vector2 CurserPosition;


	[MaybeNull]public IIntractable currentInteraction;



	public int SelectedItemSlot=1;

	public Inventory inventory;
	public PlayerCharacter(){
		if(ClientStatics.player==null)ClientStatics.player=this;
	}
    public override void _Ready()
    {
		inventory=GetNode<Inventory>("Inventory");
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
	{
		CurserPosition=GetGlobalMousePosition();
		//Movement
		direction= Input.GetVector("Character_Left", "Character_Right", "Character_Down", "Character_Up");
		Vector2 velocity = Velocity;
		if (!IsOnFloor())
		velocity.Y += gravity * (float)delta;
		if(controlPlayer){
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
		else velocity.X=0;
		Velocity = velocity;
		//Interaction
		if (Input.IsActionJustPressed("Interact"))
		{
			IIntractable target=null;
			float targetdist=33;
			Array<Node> nodes=GetTree().GetNodesInGroup("Interaction");
			for (int i=0; i<nodes.Count;i++){
				if (nodes[i] is IIntractable && nodes[i] is Node2D){
					Node2D node2D = (Node2D)nodes[i];
					float dist =node2D.GlobalPosition.DistanceTo(GlobalPosition);
					if (dist < targetdist){
						target=(IIntractable)node2D;
						targetdist=dist;
					}
				}
			}
			if(target!=null){
				currentInteraction=target;
				target.interact(this);
			}
		};
		//End_Interaction
		if (Input.IsActionJustPressed("Exit_UI")){
			if(currentInteraction!=null)
			currentInteraction.endInteraction();
			currentInteraction=null;
		}
		base._PhysicsProcess(delta);
		if(Input.IsActionJustPressed("UseItemPrimary")){
			if (inventory.GetItem(SelectedItemSlot) != null)
			inventory.GetItem(SelectedItemSlot).onPrimaryInteraction(this);
		}
		if(Input.IsActionJustPressed("UseItemSecondary")){
			if (inventory.GetItem(SelectedItemSlot) != null)
			inventory.GetItem(SelectedItemSlot).onSecondaryInteraction(this);
		}
		if(Input.IsActionJustPressed("next_item")){
			SelectedItemSlot+=1;
			if (SelectedItemSlot>9)SelectedItemSlot=0;
		}
		if(Input.IsActionJustPressed("previous_item")){
			SelectedItemSlot-=1;
			if (SelectedItemSlot<0)SelectedItemSlot=9;
		}
		if(!isInUI)inventory.SelectedItem=SelectedItemSlot;
		base._PhysicsProcess(delta);
		if(Input.IsActionJustPressed("Inventory")){
			ClientStatics.UI_Selector.ShownGUI_ID=1;
		}
		if(Input.IsActionJustPressed("Exit_UI")){
			ClientStatics.UI_Selector.ShownGUI_ID=0;
		}
	}
	
}

public enum PermissionLevels{
	None=0,
	Cheats=1,
	Moderator=2,
	Admin=3,
	Console=4
}
