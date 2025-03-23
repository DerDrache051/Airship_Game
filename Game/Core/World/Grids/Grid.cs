using Airship_Game.Game.Core.Events;
using Godot;
using Godot.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

public partial class Grid : RigidBody2D, IDamageable, ISerializable, IEventAction<TileDamagedEvent>, IEventAction<TileDestroyedEvent>, IEventAction<TilePlacedEvent>
{
	[Export] public int TilePixelSize = 8;
	[Export] public Node team;
	[Export] public bool isStatic = true;
	public System.Collections.Generic.Dictionary<String, Tile> Tiles = new();
	public System.Collections.Generic.Dictionary<String, Rid> physicsShapes = new();
	public System.Collections.Generic.Dictionary<String, LightOccluder2D> lightOccluders = new();
	public int Weight = 0;

	public bool isLive = false;//if phyiscs and light shapes should be updated. set to true after the grid is ready. set to false when making large changes, like breaking a ship or during world generation or loading
	public Grid()
	{
		EventManager.addEventAction<TileDamagedEvent>(this);
		EventManager.addEventAction<TileDestroyedEvent>(this);
		EventManager.addEventAction<TilePlacedEvent>(this);
	}

	//Stores the Tiles. the key is a 3 dimensional int array, with the first two storing the position and the last one the layer
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		isLive = true;
		RecalulatePhysicsShapes();
		//RecalculateLights();
		if (isStatic) PhysicsServer2D.BodySetMode(GetRid(), PhysicsServer2D.BodyMode.Static);
		EventManager.triggerEvent(new GridAddedEvent(this, Tiles.Values.ToList()));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	/*
	public void UpdateLights(Tile tile, int layer)
	{
		UpdateLights2(tile, layer);
		UnmarkAll();
	}
	public void RedrawAllTiles()
	{
		foreach (Tile tile in Tiles.Values)
		{
			tile.QueueRedraw();
		}
	}*/
	public void UnmarkAll()//removes all markers used by internal processes
	{
		foreach (Tile tile in Tiles.Values)
		{
			tile.isMarked = false;
			tile.LightUpdateLevel = 0;
		}

	}/*
	public void UpdateLights2(Tile tile, int layer)
	{
		if (!isLive || layer <= 0 || tile.isMarked) return;
		tile.lightLevel = Math.Max(getHighestLightLevelFromNeighbours(tile) - tile.Tilematerial.lightReduction, tile.Tilematerial.lightEmission);
		tile.isMarked = true;
		tile.QueueRedraw();
		foreach (Tile neighbour in getNeighboursOnAllLayers(tile))
		{
			UpdateLights2(neighbour, layer - 1);
		}
	}
	public void RecalculateLights()
	{
		ResetLight();
		foreach (Tile tile in Tiles.Values)
		{
			AddLight(tile);
			UpdateBorderLights(tile);
		}
	}
	public void AddLight(Tile tile)
	{
		AddLight2(tile, tile.Tilematerial.lightEmission);
		UnmarkAll();
	}
	/*public void AddLight2(Tile tile, int level)
	{
		if (tile.isMarked || level <= 0) return;
		tile.lightLevel += level;
		tile.Modulate = new Color(Math.Min(tile.lightLevel / 4f, 1), Math.Min(tile.lightLevel / 4f, 1), Math.Min(tile.lightLevel / 4f, 1), 1);
		tile.isMarked = true;
		foreach (Tile neighbour in getNeighboursOnAllLayers(tile))
		{
			AddLight2(neighbour, level - tile.lightReduction);
		}
	}*/
	/*
	private void AddLight2(Tile tile, int level)
	{
		if (tile.isMarked || level <= 0) return;
		tile.lightLevel += level;
		tile.Modulate = new Color(Math.Min(tile.lightLevel / 4f, 1), Math.Min(tile.lightLevel / 4f, 1), Math.Min(tile.lightLevel / 4f, 1), 1);
		tile.isMarked = true;
		Queue<Tile> queue = new Queue<Tile>();
		queue.Enqueue(tile);
		tile.LightUpdateLevel = level;
		tile.QueueRedraw();
		while (queue.Count > 0)
		{
			Tile currentTile = queue.Dequeue();
			foreach (Tile neighbour in getNeighboursOnAllLayers(currentTile))
			{
				if (currentTile.Tilematerial.lightReduction == 0)
				{
					if (currentTile.Tilematerial.CollisionLayer != null)
					{
						currentTile.Tilematerial.lightReduction = 3;
					}
					else currentTile.Tilematerial.lightReduction = 1;
				}
				int newLevel = currentTile.LightUpdateLevel - currentTile.Tilematerial.lightReduction;
				if (!neighbour.isMarked && newLevel > 0)
				{
					neighbour.lightLevel += newLevel;
					neighbour.LightUpdateLevel = newLevel;
					neighbour.Modulate = new Color(Math.Min(neighbour.lightLevel / 4f, 1), Math.Min(neighbour.lightLevel / 4f, 1), Math.Min(neighbour.lightLevel / 4f, 1), 1);
					neighbour.isMarked = true;
					neighbour.QueueRedraw();
					queue.Enqueue(neighbour);
				}
			}
		}
	}
	public void RemoveLight(Tile tile)
	{
		GD.Print("removing light");
		RemoveLight2(tile, tile.Tilematerial.lightEmission);
		UnmarkAll();
	}*/
	/*public void RemoveLight2(Tile tile,int level){
		if (tile.isMarked || level <= 0) return;
		GD.Print(level);
		tile.lightLevel -= level;
		tile.Modulate = new Color(Math.Min(tile.lightLevel / 4f, 1), Math.Min(tile.lightLevel / 4f, 1), Math.Min(tile.lightLevel / 4f, 1), 1);
		tile.isMarked = true;
		foreach (Tile neighbour in getNeighboursOnAllLayers(tile)){
			RemoveLight2(neighbour, level - tile.lightReduction);
		}
	}*/
	/*private void RemoveLight2(Tile tile, int level)
	{
		if (tile.isMarked || level <= 0) return;
		tile.lightLevel -= level;
		tile.Modulate = new Color(Math.Min(tile.lightLevel / 4f, 1), Math.Min(tile.lightLevel / 4f, 1), Math.Min(tile.lightLevel / 4f, 1), 1);
		tile.isMarked = true;
		Queue<Tile> queue = new Queue<Tile>();
		queue.Enqueue(tile);
		tile.LightUpdateLevel = level;
		tile.QueueRedraw();
		while (queue.Count > 0)
		{
			Tile currentTile = queue.Dequeue();
			foreach (Tile neighbour in getNeighboursOnAllLayers(currentTile))
			{
				if (currentTile.Tilematerial.lightReduction == 0)
				{
					if (currentTile.Tilematerial.CollisionLayer != null)
					{
						currentTile.Tilematerial.lightReduction = 3;
					}
					else currentTile.Tilematerial.lightReduction = 1;
				}
				int newLevel = currentTile.LightUpdateLevel - currentTile.Tilematerial.lightReduction;
				if (!neighbour.isMarked && newLevel > 0)
				{
					neighbour.LightUpdateLevel = newLevel;
					neighbour.lightLevel -= newLevel;
					neighbour.Modulate = new Color(Math.Min(neighbour.lightLevel / 4f, 1), Math.Min(neighbour.lightLevel / 4f, 1), Math.Min(neighbour.lightLevel / 4f, 1), 1);
					neighbour.isMarked = true;
					neighbour.QueueRedraw();
					queue.Enqueue(neighbour);
				}
			}
		}
	}
	public void ResetLight()
	{
		foreach (Tile tile in Tiles.Values)
		{
			tile.lightLevel = 0;
			tile.isMarked = false;
			tile.hasBorderLight = false;
		}
	}
	private void UpdateBorderLights2(Tile tile)
	{
		if (!isSurroundedOnAnyLayer(tile) && !tile.hasBorderLight)
		{
			AddLight2(tile, 8);
			tile.hasBorderLight = true;
		}
		if (isSurroundedOnAnyLayer(tile) && tile.hasBorderLight)
		{
			RemoveLight2(tile, 8);
			tile.hasBorderLight = false;
		}
	}
	public void UpdateBorderLights(Tile tile)
	{
		UpdateBorderLights2(tile);
		foreach (Tile neighbour in getNeighboursOnAllLayers(tile))
		{
			UpdateBorderLights2(neighbour);
		}
	}*/ 
	//Adds a tile to the grid
	public bool addTileToDictOnly(Tile tile, int x, int y)
	{
		//CheckBounds
		for (int i = 0; i < tile.Tilematerial.SizeX; i++)
		{
			for (int j = 0; j < tile.Tilematerial.SizeY; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					if (tile.Layers[k])
					{
						String key = "" + (x + i) + "," + (y + j) + "," + k;
						if (Tiles.ContainsKey(key))
						{
							return false;
						}
					}
				}
			}
		}
		//AddTile
		for (int i = 0; i < tile.Tilematerial.SizeX; i++)
		{
			for (int j = 0; j < tile.Tilematerial.SizeY; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					if (tile.Layers[k])
					{
						String key = "" + (x + i) + "," + (y + j) + "," + k;
						Tiles.Add(key, tile);
					}
				}
			}
		}
		return true;
	}
	public bool addTile(Tile tile, int x, int y)
	{
		if (!tile.canTileAdd(this, x, y)) return false;
		if (!addTileToDictOnly(tile, x, y)) return false;
		//SetPosition
		tile.X = x;
		tile.Y = y;
		tile.Position = new Vector2(x * TilePixelSize, y * TilePixelSize);
		if (tile.Layers[1])
		{
		}
		//addNeighbours
		tile.onTileAddGrid(this);
		//if (isLive) AddLight(tile);
		//if (isLive) UpdateBorderLights(tile);
		if (tile.GetParent() == null) AddChild(tile);
		if (tile.Layers[1] && isLive) addAndOptimizeShapes(tile);
		if (IsInGroup("Save")) tile.AddToGroup("Save");
		return true;
	}
	public bool addTile(String SceneFile, int x, int y)
	{
		PackedScene pc = GD.Load<PackedScene>(SceneFile);
		Node node = pc.Instantiate(PackedScene.GenEditState.Disabled);
		if (node == null || !(node is Tile)) return false;
		if (addTile((Tile)node, x, y))
		{
			//AddChild(node);
			return true;
		}
		return false;
	}
	public bool addTile(PackedScene scene, int x, int y)
	{
		Node node = scene.Instantiate(PackedScene.GenEditState.Disabled);
		if (node == null || !(node is Tile)) return false;
		if (addTile((Tile)node, x, y))
		{
			//AddChild(node);
			return true;
		}
		return false;
	}
	public int[] getTilePos(Vector2 pos)
	{
		int[] Out = new int[2];
		Out[0] = (int)(pos.Round().X / TilePixelSize);
		Out[1] = (int)(pos.Round().Y / TilePixelSize);
		return Out;
	}

	public int getHighestLightLevelFromNeighbours(Tile tile)
	{
		int highest = 0;
		if (!isSurroundedOnAnyLayer(tile)) highest = 4;
		foreach (Tile neighbour in getNeighboursOnAllLayers(tile))
		{
			highest = Math.Max(highest, neighbour.lightLevel);
		}
		return highest;
	}
	public bool isSurroundedOnAnyLayer(Tile tile)
	{
		for (int i = 0; i < 8; i++)
		{
			if (isSurrounded(tile, tile.X, tile.Y, (GridLayer)i))
			{
				return true;
			}
		}
		return false;
	}
	public Tile getTileAt(int x, int y, GridLayer layer)
	{
		String key = "" + x + "," + y + "," + (int)layer;
		if (Tiles.ContainsKey(key)) return Tiles[key];
		else return null;
	}
	public List<Tile> getTilesAt(int x, int y, GridLayer layer)
	{
		List<Tile> Out = new();
		for (int i = 0; i < 8; i++)
		{
			Tile tile = getTileAt(x, y, (GridLayer)i);
			if (tile != null) Out.Add(tile);
		}
		return Out;
	}
	public Tile getFirstTileAt(int x, int y)
	{
		for (int i = 0; i < 8; i++)
		{
			Tile tile = getTileAt(x, y, (GridLayer)i);
			if (tile != null) return tile;
		}
		return null;
	}
	public bool isTileAt(int x, int y, GridLayer layer)
	{
		String key = "" + x + "," + y + "," + (int)layer;
		return Tiles.ContainsKey(key);
	}
	public bool isTileAt(int x, int y)
	{
		for (int i = 0; i < 8; i++)
		{
			if (isTileAt(x, y, (GridLayer)i)) return true;
		}
		return false;
	}
	public void removeTile(Tile tile)
	{
		if (tile == null) return;
		tile.onTileRemoveGrid(this);
		if (tile.Layers[1] && isLive) removeAndOptimizeShapes(tile);
		//if (isLive) RemoveLight(tile);
		removeTilefromDictOnly(tile);
		//if (isLive) UpdateBorderLights(tile);
		RemoveChild(tile);
	}
	public void removeTilefromDictOnly(Tile tile)
	{
		if (tile == null) return;
		for (int i = 0; i < tile.Tilematerial.SizeX; i++)
		{
			for (int j = 0; j < tile.Tilematerial.SizeY; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					if (tile.Layers[k])
					{
						String key = "" + (tile.X + i) + "," + (tile.Y + j) + "," + k;
						Tiles.Remove(key);
					}
				}
			}
		}
	}
	public virtual void onTileDestroyed(Tile tile)
	{
		removeTile(tile);
	}

	public void TransferTile(Tile tile, Grid source, Grid target)
	{
		source.removeTile(tile);
		target.addTile(tile, tile.X, tile.Y);
	}
	public void RecalulatePhysicsShapes()
	{
		PhysicsServer2D.BodyClearShapes(GetRid());
		physicsShapes.Clear();
		foreach (Tile tile in Tiles.Values)
		{
			if (tile.Layers[1])
			{
				addAndOptimizeShapes(tile);
			}
		}

	}
	public bool hasNeighbourTile(Tile tile, int x, int y)
	{
		for (int i = -1; i < tile.Tilematerial.SizeX + 1; i++)
		{
			for (int j = -1; j < tile.Tilematerial.SizeY + 1; j++)
			{
				if (isTileAt(x + i, y + j)) return true;
			}
		}
		return false;
	}

	public float damageTileAt(Vector2 globalPos, float damage, DamageTypes type, Node2D source, Node2D projectile)
	{
		int[] tilepos = getTilePos(globalPos - GlobalPosition);
		Tile tile = getTileAt(tilepos[0], tilepos[1], GridLayer.CollisionLayer);
		if (tile != null)
		{
			EventManager.triggerEvent(new TileDamagedEvent(tile, this, tile.X, tile.Y, damage, type, source, projectile));
		}
		return 0;
	}
	public float dealDamage(float damage, DamageTypes damageType, Node2D source, Node2D projectile)
	{
		return damageTileAt(projectile.GlobalPosition, damage, damageType, source, projectile);
	}
	public Tile getTileAt(Vector2 globalPos, GridLayer layer)
	{
		int[] tilepos = getTilePos(globalPos - GlobalPosition);
		return getTileAt(tilepos[0], tilepos[1], layer);
	}
	public Tile getFirstTileAt(Vector2 globalPos)
	{
		int[] tilepos = getTilePos(globalPos - GlobalPosition);
		return getFirstTileAt(tilepos[0], tilepos[1]);
	}
	public List<Tile> getTilesAt(Vector2 globalPos, GridLayer layer)
	{
		int[] tilepos = getTilePos(globalPos - GlobalPosition);
		return getTilesAt(tilepos[0], tilepos[1], layer);
	}
	public Vector2 snapToNearestTile(Vector2 pos)
	{
		int[] tilepos = getTilePos(pos - GlobalPosition);
		return new Vector2(tilepos[0] * TilePixelSize, tilepos[1] * TilePixelSize) + GlobalPosition;
	}
	public int getNumberOfNeighbours(Tile tile, int x, int y, GridLayer layer)
	{
		if (tile == null) return 0;
		int count = 0;
		for (int i = -1; i < tile.Tilematerial.SizeX + 1; i++)
		{
			for (int j = -1; j < tile.Tilematerial.SizeY + 1; j++)
			{
				if (isTileAt(x + i, y + j, layer)) count++;
			}
		}
		return count;
	}
	public bool isSurrounded(Tile tile, int x, int y, GridLayer layer)
	{
		if (tile == null) return false;
		for (int i = -1; i < tile.Tilematerial.SizeX + 1; i++)
		{
			for (int j = -1; j < tile.Tilematerial.SizeY + 1; j++)
			{
				if (!isTileAt(x + i, y + j, layer)) return false;
			}
		}
		return true;
	}
	public bool isCollisionSurrounded(int x, int y)
	{
		return isSurrounded(getTileAt(x, y, GridLayer.CollisionLayer), x, y, GridLayer.CollisionLayer);
	}
	public List<Tile> getNeighbourTiles(Tile tile, int x, int y, GridLayer layer)
	{
		List<Tile> tiles = new List<Tile>();
		for (int i = -1; i < tile.Tilematerial.SizeX + 1; i++)
		{
			for (int j = -1; j < tile.Tilematerial.SizeY + 1; j++)
			{
				if (isTileAt(x + i, y + j, layer) && getTileAt(x + i, y + j, layer) != tile) tiles.Add(getTileAt(x + i, y + j, layer));
			}
		}
		return tiles;
	}
	public void addShape(int x, int y, int sizeX, int sizeY)
	{
		if (physicsShapes.ContainsKey(x + "," + y + "," + sizeX + "," + sizeY)) return;
		Vector2[] vertices = new Vector2[4];
		vertices[0] = new Vector2(x * TilePixelSize, y * TilePixelSize) - new Vector2(TilePixelSize / 2, TilePixelSize / 2);
		vertices[1] = new Vector2((x + sizeX) * TilePixelSize, y * TilePixelSize) - new Vector2(TilePixelSize / 2, TilePixelSize / 2);
		vertices[2] = new Vector2((x + sizeX) * TilePixelSize, (y + sizeY) * TilePixelSize) - new Vector2(TilePixelSize / 2, TilePixelSize / 2);
		vertices[3] = new Vector2(x * TilePixelSize, (y + sizeY) * TilePixelSize) - new Vector2(TilePixelSize / 2, TilePixelSize / 2);
		Rid rid = PhysicsServer2D.ConvexPolygonShapeCreate();
		PhysicsServer2D.ShapeSetData(rid, vertices);
		PhysicsServer2D.BodyAddShape(GetRid(), rid);
		physicsShapes.Add(x + "," + y + "," + sizeX + "," + sizeY, rid);

		if(lightOccluders.ContainsKey(x + "," + y + "," + sizeX + "," + sizeY))return;
		LightOccluder2D lightOccluder = new LightOccluder2D();
		lightOccluder.SetOccluderPolygon(new OccluderPolygon2D());
		//lightOccluder.Occluder.CullMode=OccluderPolygon2D.CullModeEnum.Clockwise;
		lightOccluder.Occluder.Polygon=vertices;
		lightOccluders.Add(x + "," + y + "," + sizeX + "," + sizeY, lightOccluder);
		AddChild(lightOccluder);
	}
	public void addAndOptimizeShapes(int x, int y, int sizeX, int sizeY)
	{
		addShape(x, y, sizeX, sizeY);
		List<Tile> tiles = getNeighbourTiles(getTileAt(x, y, GridLayer.CollisionLayer), x, y, GridLayer.CollisionLayer);
		foreach (Tile tile in tiles)
		{
			if (isCollisionSurrounded(tile.X, tile.Y))
			{
				removeShape(tile);
			}
		}
	}
	public void addAndOptimizeShapes(Tile tile)
	{
		addAndOptimizeShapes(tile.X, tile.Y, tile.Tilematerial.SizeX, tile.Tilematerial.SizeY);
	}
	public void removeShape(int x, int y, int sizeX, int sizeY)
	{
		if (physicsShapes.ContainsKey(x + "," + y + "," + sizeX + "," + sizeY))
		{
			for (int i = 0; i < PhysicsServer2D.BodyGetShapeCount(GetRid()); i++)
			{
				if (PhysicsServer2D.BodyGetShape(GetRid(), i) == physicsShapes[x + "," + y + "," + sizeX + "," + sizeY])
				{
					PhysicsServer2D.BodyRemoveShape(GetRid(), i);
				}
			}
			physicsShapes.Remove(x + "," + y + "," + sizeX + "," + sizeY);
		}
		if (lightOccluders.ContainsKey(x + "," + y + "," + sizeX + "," + sizeY))
		{
			RemoveChild(lightOccluders[x + "," + y + "," + sizeX + "," + sizeY]);
			lightOccluders[x + "," + y + "," + sizeX + "," + sizeY].QueueFree();
			lightOccluders.Remove(x + "," + y + "," + sizeX + "," + sizeY);
		}
	}
	public void removeAndOptimizeShapes(int x, int y, int sizeX, int sizeY)
	{
		removeShape(x, y, sizeX, sizeY);
		List<Tile> tiles = getNeighbourTiles(getTileAt(x, y, GridLayer.CollisionLayer), x, y, GridLayer.CollisionLayer);
		foreach (Tile tile in tiles)
		{
			addShape(tile);
		}
	}
	public void removeAndOptimizeShapes(Tile tile)
	{
		removeAndOptimizeShapes(tile.X, tile.Y, tile.Tilematerial.SizeX, tile.Tilematerial.SizeY);
	}
	public void addShape(Tile tile)
	{
		addShape(tile.X, tile.Y, tile.Tilematerial.SizeX, tile.Tilematerial.SizeY);
	}
	public void removeShape(Tile tile)
	{
		removeShape(tile.X, tile.Y, tile.Tilematerial.SizeX, tile.Tilematerial.SizeY);
	}
	public List<Tile> getNeighboursOnAllLayers(Tile tile)
	{
		List<Tile> tiles = new();
		for (int i = -1; i < tile.Tilematerial.SizeX + 1; i++)
		{
			for (int j = -1; j < tile.Tilematerial.SizeY + 1; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					if (i == -1 && j == -1 || i == -1 && j == tile.Tilematerial.SizeY || i == tile.Tilematerial.SizeX && j == -1 || i == tile.Tilematerial.SizeX && j == tile.Tilematerial.SizeY) continue;
					if (isTileAt(tile.X + i, tile.Y + j, (GridLayer)k)) tiles.Add(getTileAt(tile.X + i, tile.Y + j, (GridLayer)k));
				}
			}

		}
		return tiles;
	}
	/*public void generatePolygons()
	{
		LinkedList<Vector2> points = new LinkedList<Vector2>();
		PhysicsServer2D.BodyClearShapes(GetRid());
		if (isLive == false) return;
		foreach (Tile tile in Tiles.Values)
		{
			if (tile.Layers[1])
			{
				int x = tile.X;
				int y = tile.Y;
				int sizeX = tile.SizeX;
				int sizeY = tile.SizeY;
				// Calculate the vertices of the polygon
				Vector2[] vertices = new Vector2[4];
				vertices[0] = new Vector2(x * TilePixelSize, y * TilePixelSize) - new Vector2(TilePixelSize / 2, TilePixelSize / 2);
				vertices[1] = new Vector2((x + sizeX) * TilePixelSize, y * TilePixelSize) - new Vector2(TilePixelSize / 2, TilePixelSize / 2);
				vertices[2] = new Vector2((x + sizeX) * TilePixelSize, (y + sizeY) * TilePixelSize) - new Vector2(TilePixelSize / 2, TilePixelSize / 2);
				vertices[3] = new Vector2(x * TilePixelSize, (y + sizeY) * TilePixelSize) - new Vector2(TilePixelSize / 2, TilePixelSize / 2);

				for (int i = 0; i < 4; i++)
				{
					points.AddLast(vertices[i]);
				}
			}
		}		

		Rid rid = PhysicsServer2D.ConvexPolygonShapeCreate();
		PhysicsServer2D.ShapeSetData(rid,points.ToArray());
		PhysicsServer2D.BodyAddShape(GetRid(), rid);
	}*/
	/*private void walkAlongOutside(int x, int y, GridLayer layer, LinkedList<Vector2> points, int dx, int dy)
	{

	}*/
	public virtual Godot.Collections.Dictionary<String, String> SerializeComponents(Godot.Collections.Dictionary<String, String> dict)
	{
		dict.Add("TilePixelSize", TilePixelSize + "");
		dict.Add("X", GlobalPosition.X + "");
		dict.Add("Y", GlobalPosition.Y + "");
		dict.Add("SceneFile", SceneFilePath);
		return dict;
	}
	public virtual void DeserializeComponents(Godot.Collections.Dictionary<String, String> dict)
	{
		TilePixelSize = int.Parse(dict["TilePixelSize"]);
		GlobalPosition = new Vector2(float.Parse(dict["X"]), float.Parse(dict["Y"]));
		SceneFilePath = dict["SceneFile"];
	}
	public String Serialize()
	{
		String Out = Godot.Json.Stringify(SerializeComponents(new Godot.Collections.Dictionary<String, String>()));
		return Out;
	}
	public void Deserialize(String data)
	{
		DeserializeComponents(Godot.Json.ParseString(data).As<Godot.Collections.Dictionary<String, String>>());
	}
	public String WriteBlueprint(String filePath, String fileName)
	{

		Godot.Collections.Dictionary<String, String>[] dict = new Godot.Collections.Dictionary<String, String>[Tiles.Values.Count];
		for (int i = 0; i < Tiles.Values.Count; i++)
		{
			dict[i] = Tiles.Values.ElementAt(i).SerializeComponents(new Godot.Collections.Dictionary<String, String>());
		}
		Godot.Collections.Array<Godot.Collections.Dictionary<String, String>> dict2 = new(dict);
		String Out = Godot.Json.Stringify(dict2, "\t");
		FileAccess file = FileAccess.Open(filePath + "/" + fileName + ".blueprint", FileAccess.ModeFlags.Write);
		file.Close();
		return Out;
	}
	public void finishLoad()
	{
		buildGridFromChildren();
	}

	public void buildGridFromChildren()
	{
		foreach (Node node in GetChildren())
		{
			if (node is Tile tile)
			{
				addTile(tile, tile.X, tile.Y);
			}
		}
	}
	public void EventAction(TileDamagedEvent e)
	{
		if (e.Grid == this) e.tile.dealDamage(e.Damage, e.DamageType, e.Source, e.Projectile);
	}
	public void EventAction(TileDestroyedEvent e)
	{
		if (e.Grid == this) removeTile(e.tile);
	}
	public void EventAction(TilePlacedEvent e)
	{
		if (e.Grid == this) addTile(e.tile, e.tile.X, e.tile.Y);
	}
}
public class GridAddedEvent(Grid grid, List<Tile> tiles) : Event
{
	public Grid grid = grid;
	public List<Tile> tiles = tiles;
}
public class GridModifiedEvent(Grid grid, List<Tile> added, List<Tile> removed) : Event
{
	public Grid grid = grid;
	public List<Tile> added = added;

	public List<Tile> removed = removed;
}
