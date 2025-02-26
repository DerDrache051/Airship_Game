using Godot;
using System;
public partial class GeneratedIsland : Grid
{
	[ExportCategory("StandardGeneratorParameters")]
	[Export] public int HorizontalSize =40;
	[Export] public int VerticalSize=20;
	[Export] public int SizeOffsett=5;
	[Export] public int HillMultiplier=5;//Negative to create a valley in the middle
	[Export] public int DirtLayer=5;
	[Export] public int SurfaceJaggedness=1;
	[Export] public int BottomJaggedness=1;

	[Export] public PackedScene Grass;
	[Export] public PackedScene Dirt;
	[Export] public PackedScene Stone;

	[Export] public PackedScene BackgroundGrass;
	[Export] public PackedScene BackgroundDirt;
	[Export] public PackedScene BackgroundStone;
	[ExportCategory("CaveGeneratorParameters")]

	[Export] public int CaveSize=1;
	[Export] public int CaveSizeOffset=1;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		isLive = false;
		//Pregeneration
		int actualHorizontalSize= (int)(HorizontalSize +(SizeOffsett*GD.Randf()));
		int actualVerticalSize= (int)(VerticalSize +(SizeOffsett*GD.Randf()));
		int actualHillSize= (int)(HillMultiplier +GD.Randf());
		int actualSurfaceJaggedness= (int)(SurfaceJaggedness *GD.Randf());
		int acualBottomJaggedness= (int)(BottomJaggedness *GD.Randf());
		int actualCaveSize= (int)( CaveSize+GD.Randf()*CaveSizeOffset);

		int[] TopLayer=new int[actualHorizontalSize];
		int[] CaveCeling=new int[actualHorizontalSize];
		int[] CaveFloor=new int[actualHorizontalSize];
		int[] BottomLayer=new int[actualHorizontalSize];
		FastNoiseLite CaveNoise=new();
		//RawGeneration
		for(int i=0;i<actualHorizontalSize;i++){
			int x=i-actualHorizontalSize/2;
			TopLayer[i]=(int)(-0.01*actualHillSize*(x*x)+Math.Sin(0.5*actualSurfaceJaggedness*x)+actualVerticalSize+GD.Randf());
			BottomLayer[i]=(int)(0.05*(x*x)+(Math.Cos(2*x)*acualBottomJaggedness*5)-actualVerticalSize*5+GD.Randf()*5);
		}
		//CaveGeneration
		CaveNoise.NoiseType=FastNoiseLite.NoiseTypeEnum.Simplex;
		CaveNoise.FractalOctaves=3;
		CaveNoise.FractalGain=1;
		//TileBuilder
		for(int i=0;i<actualHorizontalSize;i++){
			if(BottomLayer[i]>TopLayer[i]){
				continue;
			}
			for(int j=TopLayer[i];j>BottomLayer[i];j--){
				if(j==TopLayer[i]){
					addTile(BackgroundGrass,i,0-j);
				}
				else if(j>TopLayer[i]-DirtLayer+(int)GD.Randf()){
					addTile(BackgroundDirt,i,0-j);
				}
				else{
					addTile(BackgroundStone,i,0-j);
				}
				if(CaveNoise.GetNoise2D(i*actualCaveSize,j*actualCaveSize)<(-0.3))continue;
				if(j==TopLayer[i]){
					addTile(Grass,i,0-j);
				}
				else if(j>TopLayer[i]-DirtLayer+(int)GD.Randf()){
					addTile(Dirt,i,0-j);
				}
				else{
					addTile(Stone,i,0-j);
				}
			}
		}
		isLive = true;
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
