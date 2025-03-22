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
			addTile("res://Game/Content/Tiles/levitite_baloon.tscn", 0, 0);
			addTile("res://Game/Content/Tiles/levitite_baloon.tscn", 5, 0);
			addTile("res://Game/Content/Tiles/wood_tile.tscn", 1, 2);
			addTile("res://Game/Content/Tiles/wood_tile.tscn", 2, 2);
			addTile("res://Game/Content/Tiles/wood_tile.tscn", 3, 2);
			addTile("res://Game/Content/Tiles/wood_tile.tscn", 4, 2);
			addTile("res://Game/Content/Tiles/wood_tile.tscn", 5, 2);
			addTile("res://Game/Content/Tiles/small_burner_engine.tscn", 2, 1);
			addTile("res://Game/Content/Tiles/helm.tscn", 4, 0);
		}
		this.LinearDamp=70;
		base._Ready();
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
		this.Mass=Weight;
		if(LevitationPower>Weight)this.GravityScale=0;
		else this.GravityScale=1;
		DesiredMovement = DesiredMovement.Normalized();
		state.ApplyCentralForce(new Vector2(DesiredMovement.X * EnginePower, DesiredMovement.Y * LevitationPower)* gravity);
        base._IntegrateForces(state);
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
					for (int j = -1; j <= current.Tilematerial.SizeX; j++)
					{
						for (int k = -1; k <= current.Tilematerial.SizeY; k++)
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
			Weight += tile.Tilematerial.Weight;
		}
	}
}
