using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(ClientStatics.player==null)return;
		this.MaxValue=ClientStatics.player.maxHealth;
		this.Value=ClientStatics.player.health;
	}
}
