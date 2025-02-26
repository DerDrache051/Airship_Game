using Godot;
using System;

public partial class Registry : Node
{
	public static Registry instance;

	[ExportCategory("Items")]
	[Export]public PackedScene[] Items;

	[ExportCategory("Tiles")]
	[Export]public PackedScene[] Tiles;

	[ExportCategory("Entities")]
	[Export]public PackedScene[] Entities;

	[ExportCategory("UIs")]
	[Export]public PackedScene[] UIs;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance=this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
