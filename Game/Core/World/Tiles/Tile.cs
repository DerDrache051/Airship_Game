using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;


public partial class Tile : Node2D, IDamageable
{
	[ExportGroup("Size")]
	[Export] public int SizeX = 1;
	[Export] public int SizeY = 1;

	[ExportGroup("DefaultValues")]
	[Export] public int Weight = 10;
	[Export] public int MaxHealth = 20;
	[Export] public string ID;
	[Export] public string DisplayName;
	[Export] public bool CanBeReplaced;
	public int Health;
	[Export] public float PiercingDamageMultiplier = 1;
	[Export] public float ImpactDamageMultiplier = 1;
	[Export] public float ExplosionDamageMultiplier = 1;
	[Export] public float FireDamageMultiplier = 1;
	[Export] public bool flammable;
	[Export] public float Hardness = 1;//Multiplier for mining time,set to 0 for instant mine,set to -1 for unbreakable

	[ExportGroup("Lighting")]
	[Export] public int lightEmission=0;
	[Export] public int lightReduction=0;

	public int lightLevel=0;
	
	[ExportGroup("Drops")]
	[Export] public PackedScene[] Drops = new PackedScene[1];
	[Export] public float[] DropChances = { 1 };
	[Export] public int[] DropAmounts = { 1 };
	[Export] public Texture2D itemTexture;
	[Export] public PackedScene itemEntity;

	[ExportGroup("GridLayers")]
	[Export] public bool castShadows = true;
	[Export] public Texture2D ForegroundDecorationLayer;
	[Export] public Texture2D CollisionLayer;
	[Export] public Texture2D InBetweenDecorationLayer;
	[Export] public Texture2D InteractionLayer;
	[Export] public Texture2D ConnectionLayer;
	[Export] public Texture2D BackgroundDecorationLayer;
	[Export] public Texture2D BackgroundLayer;
	[Export] public Texture2D BehindShipMapDecorationLayer;
	[Export] public Texture2D BehindShipMapLayer;

	[Export] public Texture2D DamageTexture;
	
	public bool[] Layers = new bool[9];
	public Grid ParentGrid = null;

	public int X;
	public int Y;
	public bool isMarked;

	public bool hasBorderLight;
	public int LightUpdateLevel;

	public Sprite2D damageSprite;

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
	public void makeInvisible(){
		Visible=false;
	}
	public void makeVisible(){
		Visible=true;
	}

	private Sprite2D CreateTileSprite(Texture2D texture,Grid grid,int layer){
		Sprite2D sprite = new Sprite2D();
		sprite.Texture = texture;
		sprite.Position = new Vector2(grid.TilePixelSize * (SizeX - 1) / 2, grid.TilePixelSize * (SizeY - 1) / 2);
		sprite.Scale = new Vector2(SizeX * grid.TilePixelSize, SizeY * grid.TilePixelSize) / sprite.Texture.GetSize();
		sprite.ZIndex=10-layer*10;
		float modulationFactor=(1-layer*0.08f);
		sprite.Modulate=new Color(modulationFactor,modulationFactor,modulationFactor,1);
		return sprite;
	}
	public virtual bool canTileAdd(Grid grid, int x, int y)
	{
		Health=MaxHealth;
		Layers[0] = ForegroundDecorationLayer!=null;
		Layers[1] = CollisionLayer!=null;
		Layers[2] = InBetweenDecorationLayer!=null;
		Layers[3] = InteractionLayer!=null;
		Layers[4] = ConnectionLayer!=null;
		Layers[5] = BackgroundDecorationLayer!=null;
		Layers[6] = BackgroundLayer!=null;
		Layers[7] = BehindShipMapDecorationLayer!=null;
		Layers[8] = BehindShipMapLayer!=null;
		return true;
	}

	public virtual void onTileAdd()
	{
	}
	public virtual void onTileRemove()
	{

	}
	public void onTileAddGrid(Grid grid)
	{
		
		if (ParentGrid == null && grid != null)
		{
			
			grid.Weight += Weight;
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
			ParentGrid.Weight -= Weight;
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
				actualDamage *= PiercingDamageMultiplier;
				break;
			case DamageTypes.Impact:
				actualDamage *= ImpactDamageMultiplier;
				break;
			case DamageTypes.Explosion:
				actualDamage *= ExplosionDamageMultiplier;
				break;
			case DamageTypes.Fire:
				actualDamage *= FireDamageMultiplier;
				break;
			default:
				break;
		}

		Health -= (int)actualDamage;
		damageSprite.SelfModulate=new Color(1,1,1,Math.Min(1-((float)Health/(float)MaxHealth),1));
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
		for (int i = 0; i < Drops.Length; i++)
		{
			if (GD.Randf() < DropChances[i])
			{
				for (int j = 0; j < DropAmounts[i]; j++)
				{
					if(Drops[i]!=null&&itemEntity!=null)
					{
					ItemEntity item = (ItemEntity)itemEntity.Instantiate();
					item.item = (Item)Drops[i].Instantiate();
					item.Position = Position;
					GetTree().Root.AddChild(item);
					}
					else if(Drops[i]==null)
					{
						dropTileItem();
					}
				}
			}
		}
	}
	public Item getTileItem(){
		TileItem item=new TileItem();
		item.DisplayName=DisplayName;
		item.ID=ID;
		item.MaxStackSize=16;
		item.StackSize=1;
		if(itemTexture!=null)
			item.ItemTexture=itemTexture;
		else if(ForegroundDecorationLayer!=null)
			item.ItemTexture=ForegroundDecorationLayer;
		else if(CollisionLayer!=null)
			item.ItemTexture=CollisionLayer;
		else if(InBetweenDecorationLayer!=null)
			item.ItemTexture=InBetweenDecorationLayer;
		else if(InteractionLayer!=null)
			item.ItemTexture=InteractionLayer;
		else if(ConnectionLayer!=null)
			item.ItemTexture=ConnectionLayer;
		else if(BackgroundDecorationLayer!=null)
			item.ItemTexture=BackgroundDecorationLayer;
		else if(BackgroundLayer!=null)
			item.ItemTexture=BackgroundLayer;
		else if(BehindShipMapDecorationLayer!=null)
			item.ItemTexture=BehindShipMapDecorationLayer;
		else if(BehindShipMapLayer!=null)
			item.ItemTexture=BehindShipMapLayer;
		item.tile=this;
		item.cursorState=CursorStates.Build;
		return item;
	}
	public void dropTileItem(){
			ItemEntity item = (ItemEntity)itemEntity.Instantiate();
			item.item = getTileItem();
			item.GlobalPosition = GlobalPosition;
			GameWorld.Instance.AddChild(item);
	}
	public virtual bool canBePickedUp(){
		return true;
	}
	public virtual bool isSame(Tile tile){
		return tile.ID==ID;
	}
	public virtual Tile Clone(){
		Tile tile = Duplicate() as Tile;
		tile.SizeX=SizeX;
		tile.SizeY=SizeY;
		tile.Weight=Weight;
		tile.MaxHealth=MaxHealth;
		tile.ID=ID;
		tile.DisplayName=DisplayName;
		tile.Health=MaxHealth;
		tile.PiercingDamageMultiplier=PiercingDamageMultiplier;
		tile.ImpactDamageMultiplier=ImpactDamageMultiplier;
		tile.ExplosionDamageMultiplier=ExplosionDamageMultiplier;
		tile.FireDamageMultiplier=FireDamageMultiplier;
		tile.flammable=flammable;
		tile.Hardness=Hardness;
		tile.Drops=Drops;
		tile.DropChances=DropChances;
		tile.DropAmounts=DropAmounts;
		tile.itemTexture=itemTexture;
		tile.itemEntity=itemEntity;
		tile.ForegroundDecorationLayer=ForegroundDecorationLayer;
		tile.CollisionLayer=CollisionLayer;
		tile.InBetweenDecorationLayer=InBetweenDecorationLayer;
		tile.InteractionLayer=InteractionLayer;
		tile.ConnectionLayer=ConnectionLayer;
		tile.BackgroundDecorationLayer=BackgroundDecorationLayer;
		tile.BackgroundLayer=BackgroundLayer;
		tile.BehindShipMapDecorationLayer=BehindShipMapDecorationLayer;
		tile.BehindShipMapLayer=BehindShipMapLayer;
		//tile.Layers=Layers;
		return tile;	
	}
	public void readyTile(){
		if(ForegroundDecorationLayer!=null)AddChild(CreateTileSprite(ForegroundDecorationLayer,ParentGrid,0));
		if(CollisionLayer!=null){AddChild(CreateTileSprite(CollisionLayer,ParentGrid,1));}
		if(InBetweenDecorationLayer!=null)AddChild(CreateTileSprite(InBetweenDecorationLayer,ParentGrid,2));
		if(InteractionLayer!=null)AddChild(CreateTileSprite(InteractionLayer,ParentGrid,3));
		if(ConnectionLayer!=null)AddChild(CreateTileSprite(ConnectionLayer,ParentGrid,4));
		if(BackgroundDecorationLayer!=null)AddChild(CreateTileSprite(BackgroundDecorationLayer,ParentGrid,5));
		if(BackgroundLayer!=null)AddChild(CreateTileSprite(BackgroundLayer,ParentGrid,6));
		if(BehindShipMapDecorationLayer!=null)AddChild(CreateTileSprite(BehindShipMapDecorationLayer,ParentGrid,7));
		if(BehindShipMapLayer!=null)AddChild(CreateTileSprite(BehindShipMapLayer,ParentGrid,8));

		for(int i=0;i<Layers.Length;i++){
			if(Layers[i]) {
				damageSprite=CreateTileSprite(DamageTexture,ParentGrid,i);
				damageSprite.ZIndex+=1;
				damageSprite.SelfModulate=new Color(1,1,1,0);
				AddChild(damageSprite);
				break;
			}
		}
		damageSprite.SelfModulate=new Color(1,1,1,0);
		if(lightReduction==0){
			if(Layers[1])lightReduction=3;
			else lightReduction=1;
		}
	}
	
}
