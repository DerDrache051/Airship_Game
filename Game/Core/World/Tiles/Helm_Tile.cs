using Godot;
using System;
using System.Diagnostics.CodeAnalysis;

public partial class Helm_Tile : UiTile, IIntractable
{

	public bool Active;
	public Vector2 TargetPosition;

	[MaybeNull] public PlayerCharacter Pilot;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TargetPosition = GlobalPosition;
		Active = true;
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Active)
		{
			Vector2 Movement = Vector2.Zero;
			if (Pilot != null)
			{
				Movement = Pilot.direction;
			}

			// Update TargetPosition based on Movement
			if (Movement != Vector2.Zero)
			{
				TargetPosition = GlobalPosition + Movement;
			}

			if (ParentGrid != null && ParentGrid is Ship ParentShip)
			{
				ParentShip.DesiredMovement = TargetPosition - GlobalPosition;

				// Check if the ship has reached the target position
				if (TargetPosition.DistanceSquaredTo(GlobalPosition) < 0.6)
				{
					ParentShip.DesiredMovement = Vector2.Zero;
					TargetPosition = GlobalPosition;
				}
			}
		}
	}
	public override void interact(Node source)
	{
		if (source is PlayerCharacter) Pilot = (PlayerCharacter)source;
		Pilot.isInUI = true;
		Pilot.controlPlayer = false;
		Active = true;
		base.interact(source);
	}
	public override void endInteraction()
	{
		if (Pilot == null) return;
		Pilot.isInUI = false;
		Pilot.controlPlayer = true;
		Pilot = null;
		TargetPosition = Position;
		Active = false;
		base.endInteraction();
	}
}
