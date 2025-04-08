using Godot;
using Airship_Game.Game.Core.World.Wordgen.Islandgen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airship_Game.Game.Core.World.Wordgen
{
    [GlobalClass]
    public partial class WorldgenIslandPlacer : WorldgenObject

    {
        [Export] public PackedScene GridScene;

        [ExportCategory("StandardGeneratorParameters")]
        [Export] public int HorizontalSize = 40;
        [Export] public int VerticalSize = 20;
        [Export] public int SizeOffsett = 5;
        [Export] public int HillMultiplier = 5;//Negative to create a valley in the middle
        [Export] public int DirtLayer = 5;
        [Export] public int SurfaceJaggedness = 1;
        [Export] public int BottomJaggedness = 1;

        [Export] public PackedScene Grass;
        [Export] public PackedScene Dirt;
        [Export] public PackedScene Stone;

        [Export] public PackedScene BackgroundGrass;
        [Export] public PackedScene BackgroundDirt;
        [Export] public PackedScene BackgroundStone;

        [ExportCategory("CaveGeneratorParameters")]
        [Export] public int CaveSize = 1;
        [Export] public int CaveSizeOffset = 1;

        [ExportCategory("Decorators")]
        [Export] public IslandDecorator[] surfaceDecorators;
        [Export] public IslandDecorator[] caveDecorators;
        [Export] public IslandDecorator[] islandDecorators;

        public override void _Place(Vector2 position)
        {
            GD.Print("Generating Island at"+position.ToString());
            Grid grid = GridScene.Instantiate<Grid>();
            GeneratedIsland(grid);
            grid.GlobalPosition = position;
            GameWorld.Instance.CallDeferred("add_child", grid);
        }
        private void GeneratedIsland(Grid grid)
        {
            
            grid.isLive = false;
            //Pregeneration
            int actualHorizontalSize = (int)(HorizontalSize + (SizeOffsett * GD.Randf()));
            int actualVerticalSize = (int)(VerticalSize + (SizeOffsett * GD.Randf()));
            int actualHillSize = (int)(HillMultiplier + GD.Randf());
            int actualSurfaceJaggedness = (int)(SurfaceJaggedness * GD.Randf());
            int acualBottomJaggedness = (int)(BottomJaggedness * GD.Randf());
            int actualCaveSize = (int)(CaveSize + GD.Randf() * CaveSizeOffset);

            int[] TopLayer = new int[actualHorizontalSize];
            int[] CaveCeling = new int[actualHorizontalSize];
            int[] CaveFloor = new int[actualHorizontalSize];
            int[] BottomLayer = new int[actualHorizontalSize];
            FastNoiseLite CaveNoise = new();
            //RawGeneration
            for (int i = 0; i < actualHorizontalSize; i++)
            {
                int x = i - actualHorizontalSize / 2;
                TopLayer[i] = (int)(-0.01 * actualHillSize * (x * x) + Math.Sin(0.5 * actualSurfaceJaggedness * x) + actualVerticalSize + GD.Randf());
                BottomLayer[i] = (int)(0.05 * (x * x) + (Math.Cos(2 * x) * acualBottomJaggedness * 5) - actualVerticalSize * 5 + GD.Randf() * 5);
            }
            //CaveGeneration
            CaveNoise.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
            CaveNoise.FractalOctaves = 3;
            CaveNoise.FractalGain = 1;
            //TileBuilder
            for (int i = 0; i < actualHorizontalSize; i++)
            {
                if (BottomLayer[i] > TopLayer[i])
                {
                    continue;
                }
                for (int j = TopLayer[i]; j > BottomLayer[i]; j--)
                {
                    if (j == TopLayer[i])
                    {
                        grid.addTile(BackgroundGrass, i, 0 - j);
                    }
                    else if (j > TopLayer[i] - DirtLayer + (int)GD.Randf())
                    {
                        grid.addTile(BackgroundDirt, i, 0 - j);
                    }
                    else
                    {
                        grid.addTile(BackgroundStone, i, 0 - j);
                    }
                    if (CaveNoise.GetNoise2D(i * actualCaveSize, j * actualCaveSize) < (-0.3)) continue;
                    if (j == TopLayer[i])
                    {
                        grid.addTile(Grass, i, 0 - j);
                    }
                    else if (j > TopLayer[i] - DirtLayer + (int)GD.Randf())
                    {
                        grid.addTile(Dirt, i, 0 - j);
                    }
                    else
                    {
                        grid.addTile(Stone, i, 0 - j);
                    }
                }
            }
            //Apply Decorators
            for (int i = 0; i < actualHorizontalSize; i++)
            {
                if (BottomLayer[i] > TopLayer[i])
                {
                    continue;
                }
                for (int j = TopLayer[i]; j > BottomLayer[i]; j--)
                {
                    if (j == TopLayer[i])
                    {
                        for(int k=0;k<surfaceDecorators.Length;k++)
                        {
                            surfaceDecorators[k]._Place(new Vector2I(i,j),grid);
                        }
                    }
                    else
                    {
                        for(int k=0;k<islandDecorators.Length;k++)
                        {
                            islandDecorators[k]._Place(new Vector2I(i,j),grid);
                        }
                    }
                    if (CaveNoise.GetNoise2D(i * actualCaveSize, j * actualCaveSize) > (-0.2)){
                        for(int k=0;k<caveDecorators.Length;k++)
                        {
                            caveDecorators[k]._Place(new Vector2I(i,j),grid);
                        }
                    }
                }
            }
        }
    }
}