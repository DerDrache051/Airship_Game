using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class Registry : Node
{
	public static Registry instance;

	public PackedScene[] Tiles;
	public PackedScene[] Items;
	public PackedScene[] Entities;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		String[] TileFiles=DirAccess.GetFilesAt("res://Game/Content/Tiles");
		String[] ItemFiles=DirAccess.GetFilesAt("res://Game/Content/Items");
		String[] EntitiyFiles=DirAccess.GetFilesAt("res://Game/Content/Entities");

		LinkedList<PackedScene> TileList=new LinkedList<PackedScene>();
		foreach(String file in TileFiles){
			TileList.AddLast(GD.Load<PackedScene>("res://Game/Content/Tiles/"+file));
		}
		Tiles=TileList.ToArray();

		LinkedList<PackedScene> ItemList=new LinkedList<PackedScene>();
		foreach(String file in ItemFiles){
			ItemList.AddLast(GD.Load<PackedScene>("res://Game/Content/Items/"+file));
		}
		Items=ItemList.ToArray();

		LinkedList<PackedScene> EntitiyList=new LinkedList<PackedScene>();
		foreach(String file in EntitiyFiles){
			EntitiyList.AddLast(GD.Load<PackedScene>("res://Game/Content/Entities/"+file));
		}
		Entities=EntitiyList.ToArray();
		instance=this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
