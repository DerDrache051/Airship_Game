using System;
using System.Runtime.CompilerServices;
using Airship_Game.Game.Core.Events;
using Airship_Game.Game.Core.World.Tiles;
using Godot;


public partial class Tile : Node2D, IDamageable, ISerializable
{

	[Export] public TileMaterial Tilematerial;
	public bool[] Layers = new bool[9];
	public Grid ParentGrid = null;
	public int Health;
	public int X;
	public int Y;
	public bool isMarked;
	public int lightLevel;
	public bool hasBorderLight;
	public int LightUpdateLevel;
	public Sprite2D damageSprite;
	public bool wasVisibleOnScreen;
	public bool hasSprites;

	public TileRotation tileRotation;
	

	// Called when the node enters the scene tree for the first time.
	public Tile()
	{

	}
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	public void makeInvisible()
	{
		Visible = false;
	}
	public void makeVisible()
	{
		Visible = true;
	}

	private Sprite2D CreateTileSprite(Texture2D texture, Grid grid, int layer)
	{

		Sprite2D sprite = new Sprite2D();
		sprite.Texture = texture;
		sprite.Position = new Vector2(grid.TilePixelSize * (Tilematerial.SizeX - 1) / 2, grid.TilePixelSize * (Tilematerial.SizeY - 1) / 2);
		sprite.Scale = new Vector2(Tilematerial.SizeX * grid.TilePixelSize, Tilematerial.SizeY * grid.TilePixelSize) / sprite.Texture.GetSize();
		sprite.ZIndex = 10 - layer * 10;
		float modulationFactor = (1 - layer * 0.08f);
		sprite.Modulate = new Color(modulationFactor, modulationFactor, modulationFactor, 1);
		return sprite;
	}
	public virtual bool canTileAdd(Grid grid, int x, int y)
	{
		Health = Tilematerial.MaxHealth;
		Layers[0] = Tilematerial.ForegroundDecorationLayer != null;
		Layers[1] = Tilematerial.CollisionLayer != null;
		Layers[2] = Tilematerial.InBetweenDecorationLayer != null;
		Layers[3] = Tilematerial.InteractionLayer != null;
		Layers[4] = Tilematerial.ConnectionLayer != null;
		Layers[5] = Tilematerial.BackgroundDecorationLayer != null;
		Layers[6] = Tilematerial.BackgroundLayer != null;
		Layers[7] = Tilematerial.BehindShipMapDecorationLayer != null;
		Layers[8] = Tilematerial.BehindShipMapLayer != null;
		return true;
	}

	public virtual void onTileAdd()
	{
	}
	public virtual void onTileRemove()
	{
		GD.Print(Serialize());
	}
	public void onTileAddGrid(Grid grid)
	{

		if (ParentGrid == null && grid != null)
		{

			grid.Weight += Tilematerial.Weight;
			ParentGrid = grid;
			this.onTileAdd();
		}
		readyTile();
	}
	public void onTileRemoveGrid(Grid grid)
	{
		if (ParentGrid != null)
		{
			this.onTileRemove();
			ParentGrid.Weight -= Tilematerial.Weight;
			GD.Print("lol");
			ParentGrid = null;
		}
	}
	public virtual float dealDamage(float damage, DamageTypes damageType, Node2D source, Node2D projectile)
	{
		float actualDamage = damage;

		switch (damageType)
		{
			case DamageTypes.Piercing:
				actualDamage *= Tilematerial.PiercingDamageMultiplier;
				break;
			case DamageTypes.Impact:
				actualDamage *= Tilematerial.ImpactDamageMultiplier;
				break;
			case DamageTypes.Explosion:
				actualDamage *= Tilematerial.ExplosionDamageMultiplier;
				break;
			case DamageTypes.Fire:
				actualDamage *= Tilematerial.FireDamageMultiplier;
				break;
			default:
				break;
		}

		Health -= (int)actualDamage;
		damageSprite.SelfModulate = new Color(1, 1, 1, Math.Min(1 - ((float)Health / (float)Tilematerial.MaxHealth), 1));
		if (Health <= 0)
		{
			Health = 0;
			dropItems();
			ParentGrid.onTileDestroyed(this);

		}

		return actualDamage;
	}
	public void dropItems()
	{
		for (int i = 0; i < Tilematerial.Drops.Length; i++)
		{
			if (GD.Randf() < Tilematerial.DropChances[i])
			{
				for (int j = 0; j < Tilematerial.DropAmounts[i]; j++)
				{
					if (Tilematerial.Drops[i] != null && Tilematerial.itemEntity != null)
					{
						ItemEntity item = (ItemEntity)Tilematerial.itemEntity.Instantiate();
						item.item = (Item)Tilematerial.Drops[i].Instantiate();
						item.Position = Position;
						GetTree().Root.AddChild(item);
					}
					else if (Tilematerial.Drops[i] == null)
					{
						dropTileItem();
					}
				}
			}
		}
	}
	public Item getTileItem()
	{
		TileItem item = new();
		item.DisplayName = Tilematerial.DisplayName;
		item.SceneFilePath = "res://Game/Core/Items/tile_item.tscn";
		item.ID = Tilematerial.ID;
		item.MaxStackSize = 16;
		item.StackSize = 1;
		if (Tilematerial.itemTexture != null)
			item.ItemTexture = Tilematerial.itemTexture;
		else if (Tilematerial.ForegroundDecorationLayer != null)
			item.ItemTexture = Tilematerial.ForegroundDecorationLayer;
		else if (Tilematerial.CollisionLayer != null)
			item.ItemTexture = Tilematerial.CollisionLayer;
		else if (Tilematerial.InBetweenDecorationLayer != null)
			item.ItemTexture = Tilematerial.InBetweenDecorationLayer;
		else if (Tilematerial.InteractionLayer != null)
			item.ItemTexture = Tilematerial.InteractionLayer;
		else if (Tilematerial.ConnectionLayer != null)
			item.ItemTexture = Tilematerial.ConnectionLayer;
		else if (Tilematerial.BackgroundDecorationLayer != null)
			item.ItemTexture = Tilematerial.BackgroundDecorationLayer;
		else if (Tilematerial.BackgroundLayer != null)
			item.ItemTexture = Tilematerial.BackgroundLayer;
		else if (Tilematerial.BehindShipMapDecorationLayer != null)
			item.ItemTexture = Tilematerial.BehindShipMapDecorationLayer;
		else if (Tilematerial.BehindShipMapLayer != null)
			item.ItemTexture = Tilematerial.BehindShipMapLayer;
		item.tile = this;
		item.cursorState = CursorStates.Build;
		return item;
	}
	public void dropTileItem()
	{
		ItemEntity item = (ItemEntity)Tilematerial.itemEntity.Instantiate();
		item.item = getTileItem();
		item.GlobalPosition = GlobalPosition;
		GameWorld.Instance.AddChild(item);
	}
	public virtual bool canBePickedUp()
	{
		return true;
	}
	public virtual bool isSame(Tile tile)
	{
		return tile.Tilematerial.ID == Tilematerial.ID;
	}
	public virtual Tile Clone()
	{
		Tile tile = Duplicate() as Tile;
		tile.Tilematerial = Tilematerial;
		tile.Health = Tilematerial.MaxHealth;
		//tile.Layers=Layers;
		return tile;
	}
	public void readyTile()
	{

	}
	public virtual Godot.Collections.Dictionary<String, String> SerializeComponents(Godot.Collections.Dictionary<String, String> dict)
	{
		dict.Add("SceneFile", SceneFilePath);
		dict.Add("X", X + "");
		dict.Add("Y", Y + "");
		dict.Add("Health", Health + "");
		return dict;
	}
	public virtual void DeserializeComponents(Godot.Collections.Dictionary<String, String> dict)
	{
		SceneFilePath = dict["SceneFile"];
		X = int.Parse(dict["X"]);
		Y = int.Parse(dict["Y"]);
		Health = int.Parse(dict["Health"]);
	}
	public static Tile CreateTileFromDataComponents(Godot.Collections.Dictionary<String, String> dict)
	{
		Tile tile = GD.Load<PackedScene>(dict["SceneFile"]).Instantiate<Tile>();
		if (tile == null) return null;
		tile.DeserializeComponents(dict);
		return tile;
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
	public static Node CreateFromData(String data)
	{
		Tile tile = CreateTileFromDataComponents(Godot.Json.ParseString(data).As<Godot.Collections.Dictionary<String, String>>());
		return tile;
	}
	public void finishLoad()
	{

	}

	private void CreateSprites()
	{
		if (Tilematerial.ForegroundDecorationLayer != null) AddChild(CreateTileSprite(Tilematerial.ForegroundDecorationLayer, ParentGrid, 0));
		if (Tilematerial.CollisionLayer != null) { AddChild(CreateTileSprite(Tilematerial.CollisionLayer, ParentGrid, 1)); }
		if (Tilematerial.InBetweenDecorationLayer != null) AddChild(CreateTileSprite(Tilematerial.InBetweenDecorationLayer, ParentGrid, 2));
		if (Tilematerial.InteractionLayer != null) AddChild(CreateTileSprite(Tilematerial.InteractionLayer, ParentGrid, 3));
		if (Tilematerial.ConnectionLayer != null) AddChild(CreateTileSprite(Tilematerial.ConnectionLayer, ParentGrid, 4));
		if (Tilematerial.BackgroundDecorationLayer != null) AddChild(CreateTileSprite(Tilematerial.BackgroundDecorationLayer, ParentGrid, 5));
		if (Tilematerial.BackgroundLayer != null) AddChild(CreateTileSprite(Tilematerial.BackgroundLayer, ParentGrid, 6));
		if (Tilematerial.BehindShipMapDecorationLayer != null) AddChild(CreateTileSprite(Tilematerial.BehindShipMapDecorationLayer, ParentGrid, 7));
		if (Tilematerial.BehindShipMapLayer != null) AddChild(CreateTileSprite(Tilematerial.BehindShipMapLayer, ParentGrid, 8));
		CreateDamageSprite();
		hasSprites = true;
	}
	private void CreateDamageSprite(){
		for (int i = 0; i < Layers.Length; i++)
		{
			if (Layers[i])
			{
				damageSprite = CreateTileSprite(Tilematerial.DamageTexture, ParentGrid, i);
				damageSprite.ZIndex += 1;
				damageSprite.SelfModulate = new Color(1, 1, 1, 0);
				AddChild(damageSprite);
				break;
			}
		}
		damageSprite.SelfModulate = new Color(1, 1, 1, 0);
		if (Tilematerial.lightReduction == 0)
		{
			if (Layers[1]) Tilematerial.lightReduction = 3;
			else Tilematerial.lightReduction = 1;
		}
	}
	public bool isVisibleOnScreen()
	{
		Rect2 rect = GetViewportRect();
		if (rect.HasPoint(GetViewportTransform().Origin)) return true;
		return false;
	}

	public override void _Draw()
	{
		
		if (Tilematerial.useSprites)
		{
			if(!hasSprites)CreateSprites();
		}
		else
		{
			Texture2D text=null;
			int layer = 0;
			if (Tilematerial.ForegroundDecorationLayer != null)
			{
				text = Tilematerial.ForegroundDecorationLayer;
				layer=0;
			}
			else if (Tilematerial.CollisionLayer != null)
			{
				text = Tilematerial.CollisionLayer;
				layer=1;
			}
			else if (Tilematerial.InBetweenDecorationLayer != null)
			{
				text = Tilematerial.InBetweenDecorationLayer;
				layer=2;
			}
			else if (Tilematerial.InteractionLayer != null)
			{
				text = Tilematerial.InteractionLayer;
				layer=3;
			}
			else if (Tilematerial.ConnectionLayer != null)
			{
				text = Tilematerial.ConnectionLayer;
				layer=4;
			}
			else if (Tilematerial.BackgroundDecorationLayer != null)
			{
				text = Tilematerial.BackgroundDecorationLayer;
				layer=5;
			}
			else if (Tilematerial.BackgroundLayer != null)
			{
				text = Tilematerial.BackgroundLayer;
				layer=6;
			}
			else if (Tilematerial.BehindShipMapDecorationLayer != null)
			{
				text = Tilematerial.BehindShipMapDecorationLayer;
				layer=7;
			}
			else if (Tilematerial.BehindShipMapLayer != null)
			{
				text = Tilematerial.BehindShipMapLayer;
				layer=8;
			}
			if (text != null)
			{
				float ModulationFactor=1-(layer*0.08f);
				this.ZIndex = 10 - layer * 10;
				DrawTextureRect(text,new Rect2(ParentGrid.TilePixelSize*-0.5f,ParentGrid.TilePixelSize*-0.5f,ParentGrid.TilePixelSize*Tilematerial.SizeX,ParentGrid.TilePixelSize*Tilematerial.SizeY),false,new Color(Modulate.R*ModulationFactor,Modulate.G*ModulationFactor,Modulate.B*ModulationFactor,1));
			}
			CreateDamageSprite();
		}
	}
	public TileCollisionShape GetLocalCollisionShape(){
	if (Tilematerial.CollisionShape == TileCollisionShape.Disabled) return TileCollisionShape.Disabled;
	if (Tilematerial.CollisionShape == TileCollisionShape.Rectangle) return TileCollisionShape.Rectangle;
	if(Tilematerial.CollisionShape == TileCollisionShape.Triangle_tl)
	    if(tileRotation == TileRotation.Up) return TileCollisionShape.Triangle_tl;
		if(tileRotation == TileRotation.Right) return TileCollisionShape.Triangle_tr;
		if(tileRotation == TileRotation.Down) return TileCollisionShape.Triangle_br;
		if(tileRotation == TileRotation.Left) return TileCollisionShape.Triangle_bl;
		if(tileRotation == TileRotation.mirroredX) return TileCollisionShape.Triangle_tr;
		if(tileRotation == TileRotation.mirroredY) return TileCollisionShape.Triangle_bl;
	if(Tilematerial.CollisionShape == TileCollisionShape.Triangle_tr)
	    if(tileRotation == TileRotation.Up) return TileCollisionShape.Triangle_tr;
		if(tileRotation == TileRotation.Right) return TileCollisionShape.Triangle_br;
		if(tileRotation == TileRotation.Down) return TileCollisionShape.Triangle_bl;
		if(tileRotation == TileRotation.Left) return TileCollisionShape.Triangle_tl;
		if(tileRotation == TileRotation.mirroredX) return TileCollisionShape.Triangle_bl;
		if(tileRotation == TileRotation.mirroredY) return TileCollisionShape.Triangle_tr;
	if(Tilematerial.CollisionShape == TileCollisionShape.Triangle_bl)
	    if(tileRotation == TileRotation.Up) return TileCollisionShape.Triangle_bl;
		if(tileRotation == TileRotation.Right) return TileCollisionShape.Triangle_tl;
		if(tileRotation == TileRotation.Down) return TileCollisionShape.Triangle_tr;
		if(tileRotation == TileRotation.Left) return TileCollisionShape.Triangle_br;
		if(tileRotation == TileRotation.mirroredX) return TileCollisionShape.Triangle_tr;
		if(tileRotation == TileRotation.mirroredY) return TileCollisionShape.Triangle_bl;
	if(Tilematerial.CollisionShape == TileCollisionShape.Triangle_br)
	    if(tileRotation == TileRotation.Up) return TileCollisionShape.Triangle_br;
		if(tileRotation == TileRotation.Right) return TileCollisionShape.Triangle_bl;
		if(tileRotation == TileRotation.Down) return TileCollisionShape.Triangle_tl;
		if(tileRotation == TileRotation.Left) return TileCollisionShape.Triangle_tr;
		if(tileRotation == TileRotation.mirroredX) return TileCollisionShape.Triangle_tl;
		if(tileRotation == TileRotation.mirroredY) return TileCollisionShape.Triangle_br;
	return TileCollisionShape.Disabled;
	}
	public void RotateTile90(){
		if (tileRotation == TileRotation.Up) tileRotation = TileRotation.Right;
		else if (tileRotation == TileRotation.Right) tileRotation = TileRotation.Down;
		else if (tileRotation == TileRotation.Down) tileRotation = TileRotation.Left;
		else if (tileRotation == TileRotation.Left) tileRotation = TileRotation.Up;
		RotationDegrees=(int)tileRotation*90;
	}
	public void mirrorX(){
		if (tileRotation != TileRotation.mirroredX) tileRotation = TileRotation.mirroredX;
		else tileRotation = TileRotation.Up;
		Transform=new Transform2D(Transform.X*-1,Transform.Y,Transform.Origin);
	}
	public void mirrorY(){
		if (tileRotation != TileRotation.mirroredY) tileRotation = TileRotation.mirroredY;
		else tileRotation = TileRotation.Up;
		Transform=new Transform2D(Transform.X,Transform.Y*-1,Transform.Origin);
	}
}



public class TilePlacedEvent(Tile tile, Grid grid, int x, int y) : CancelableEvent{
	public Tile tile = tile;
	public Grid Grid = grid;
	public int X = x;
	public int Y = y;
}
public class TileDestroyedEvent(Tile tile, Grid grid, int x, int y) : CancelableEvent{
	public Tile tile = tile;
	public Grid Grid = grid;
	public int X = x;
	public int Y = y;
}
public class TileDamagedEvent(Tile tile, Grid grid, int x, int y, float damage, DamageTypes damageType, Node2D source, Node2D projectile) : CancelableEvent{
	public Tile tile = tile;
	public Grid Grid = grid;
	public int X = x;
	public int Y = y;
	public float Damage = damage;
	public DamageTypes DamageType = damageType;
	public Node2D Source = source;
	public Node2D Projectile = projectile;
    private DamageTypes type;

}

public enum TileRotation{
	Up=0,
	Right=1,
	Down=2,
	Left=3,
	mirroredX,
	mirroredY
}
