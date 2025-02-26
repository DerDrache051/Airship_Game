using Godot;
using System;
using System.Collections.Generic;

public partial class Ship : Grid
{
	public Vector2 DesiredMovement;//Where the ship wants to go
	float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public int LevitationPower;//Modified by tiles
	public int EnginePower;//Modified by tiles

	[Export] public bool DebugSpawn = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		//Testin
		if (DebugSpawn)
		{
			addTile("res://Game/Content/Airship_Tiles/levitite_baloon.tscn", 0, 0);
			addTile("res://Game/Content/Airship_Tiles/levitite_baloon.tscn", 5, 0);
			addTile("res://Game/Content/Airship_Tiles/wood_tile.tscn", 1, 2);
			addTile("res://Game/Content/Airship_Tiles/wood_tile.tscn", 2, 2);
			addTile("res://Game/Content/Airship_Tiles/wood_tile.tscn", 3, 2);
			addTile("res://Game/Content/Airship_Tiles/wood_tile.tscn", 4, 2);
			addTile("res://Game/Content/Airship_Tiles/wood_tile.tscn", 5, 2);
			addTile("res://Game/Content/Airship_Tiles/small_burner_engine.tscn", 2, 1);
			addTile("res://Game/Content/Airship_Tiles/helm.tscn", 4, 0);
		}
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//GD.Print(Weight);
		if (Weight == 0) recalulateWeight();
		if (Weight == 0)
		{
			PhysicsServer2D.BodyClearShapes(GetRid());
			GetParent().RemoveChild(this);
			Dispose();
			return;
		}
		int LevitationFactor = LevitationPower / Weight;
		int EnginePowerFactor = EnginePower / Weight;
		Vector2 Movement = new Vector2(DesiredMovement.X * EnginePowerFactor, DesiredMovement.Y * LevitationFactor);
		if (Weight > LevitationPower) Movement.Y += gravity;
		Velocity = Movement.LimitLength(100);
		//GD.Print(Velocity);
		MoveAndSlide();
		//GD.Print(Weight);
		base._Process(delta);
	}
	public void BreakShip()
	{
		LinkedList<LinkedList<int[]>> tileLists = new();
		foreach (Tile tile in Tiles.Values)
		{
			int[] tilepos = new int[3];
			tilepos[0] = tile.X;
			tilepos[1] = tile.Y;
			LinkedList<int[]> tilegroup = new LinkedList<int[]>();
			addNeighbours(tilegroup, tilepos);
			if (tilegroup.Count > 0) tileLists.AddLast(tilegroup);
		}
		if (tileLists.Count == 0) return;
		tileLists.RemoveFirst();
		UnmarkAll();
		foreach (LinkedList<int[]> tilegroup in tileLists)
		{
			Ship newShip = new Ship();
			newShip.TilePixelSize = TilePixelSize;
			newShip.GlobalPosition = GlobalPosition;
			GD.Print("createdNewGrid");
			GetParent().AddChild(newShip);
			foreach (int[] tilepos in tilegroup)
			{
				TransferTile(getTileAt(tilepos[0], tilepos[1], (GridLayer)tilepos[2]), this, newShip);
			}
		}

	}
	public void addNeighbours(LinkedList<int[]> tiles, int[] startTile)
	{
		Queue<int[]> queue = new();
		queue.Enqueue(startTile);

		while (queue.Count > 0)
		{
			int[] tile = queue.Dequeue();
			for (int i = 0; i < 8; i++)
			{
				Tile current = getTileAt(tile[0], tile[1], (GridLayer)i);
				if (current != null && !current.isMarked)
				{
					current.isMarked = true;
					int[] newTile = new int[3] { tile[0], tile[1], i };
					tiles.AddLast(newTile);

					// Check all neighboring positions
					for (int j = -1; j <= current.SizeX; j++)
					{
						for (int k = -1; k <= current.SizeY; k++)
						{
							int[] neighborTile = new int[3] { tile[0] + j, tile[1] + k, i };
							queue.Enqueue(neighborTile);
						}
					}
				}
			}
		}
	}
	/*
	public void addNeighbours(LinkedList<int[]> tiles, int[] tile)
	{
		for (int i = 0; i < 8; i++)
		{
			Tile current = getTileAt(tile[0], tile[1], (GridLayer)i);
			if (current != null && !current.isMarked)
			{
				current.isMarked = true;
				int[] newTile = new int[3];
				newTile[0] = tile[0];
				newTile[1] = tile[1];
				newTile[2] = i;
				tiles.AddLast(newTile);
				for (int j = -1; j < current.SizeX + 1; j++)
				{
					for (int k = -1; k < current.SizeY + 1; k++)
					{
						int[] tilepos = new int[3];
						tilepos[0] = j + tile[0];
						tilepos[1] = k + tile[1];
						tilepos[2] = i;
						addNeighbours(tiles, tilepos);
					}
				}
			}
		}
	}*/
	public override void onTileDestroyed(Tile tile)
	{
		removeTile(tile);
		BreakShip();
	}
	public void recalulateWeight()
	{
		Weight = 0;
		foreach (Tile tile in Tiles.Values)
		{
			Weight += tile.Weight;
		}
	}
}
