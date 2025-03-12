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
	public override void _PhysicsProcess(double delta)
	{
		if(TargetPosition==Vector2.Zero)TargetPosition=GlobalPosition;
		if(Pilot!=null){
			if(Pilot.direction.X!=0)TargetPosition.X=GlobalPosition.X+Pilot.direction.X*10;
			if(Pilot.direction.Y!=0)TargetPosition.Y=GlobalPosition.Y+Pilot.direction.Y*-10;
		}
		if(ParentGrid is Ship ParentShip){
			ParentShip.DesiredMovement = (TargetPosition-GlobalPosition).Normalized();
			if(TargetPosition.DistanceTo(GlobalPosition)<5)ParentShip.DesiredMovement=Vector2.Zero;
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
