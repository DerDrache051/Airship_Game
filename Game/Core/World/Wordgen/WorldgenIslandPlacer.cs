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
            throw new NotImplementedException();
        }
    }
}