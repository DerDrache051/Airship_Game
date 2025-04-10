using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Resolvers;
using Godot;

namespace Airship_Game.Game.Core.World.Tiles;



[GlobalClass]
public partial class TileMaterial : Resource
{
    [ExportGroup("Size")]
	[Export] public int SizeX = 1;
	[Export] public int SizeY = 1;

	[ExportGroup("DefaultValues")]
	[Export] public int Weight = 10;
	[Export] public int MaxHealth = 20;
	[Export] public string ID;
	[Export] public string DisplayName;
	[Export] public TileCollisionShape CollisionShape = TileCollisionShape.Rectangle;
	[Export] public bool SemiSolid;
	[Export] public bool useSprites;
	[Export] public TileRotationMode RotationMode = TileRotationMode.NoRotation;
	[Export] public bool canBeClimbed;
	[Export] public float PiercingDamageMultiplier = 1;
	[Export] public float ImpactDamageMultiplier = 1;
	[Export] public float ExplosionDamageMultiplier = 1;
	[Export] public float FireDamageMultiplier = 1;
	[Export] public bool flammable;
	[Export] public float Hardness = 1;//Multiplier for mining time,set to 0 for instant mine,set to -1 for unbreakable

	[ExportGroup("Lighting")]
	[Export] public int lightEmission=0;
	[Export] public int lightReduction=0;
	
	[ExportGroup("Drops")]
	[Export] public PackedScene[] Drops = new PackedScene[1];
	[Export] public float[] DropChances = { 1 };
	[Export] public int[] DropAmounts = { 1 };
	[Export] public Texture2D itemTexture;
	[Export] public PackedScene itemEntity=GD.Load<PackedScene>("res://Game/Core/World/Entities/item_entity.tscn");

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
	[Export] public Texture2D DamageTexture=GD.Load<Texture2D>("res://Game/Core/Assets/Tile_Damage_Indicator.png");
}

public enum TileRotationMode{
	NoRotation,
	Rotate90,
	mirrorX,
	mirrorY
}

public enum TileCollisionShape{
	Rectangle,
	Triangle_tl,
	Triangle_tr,
	Triangle_bl,
	Triangle_br,
	Disabled,
}
