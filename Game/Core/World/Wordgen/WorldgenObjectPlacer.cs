using Godot;
using Godot.NativeInterop;



namespace Airship_Game.Game.Core.World.Wordgen

{
    [GlobalClass]
    public partial class WorldgenObjectPlacer:Resource
    {
    [ExportGroup("Spacing")]

    [Export] public int minDistancetoGrids = 0;

    [Export] public int maxDistancetoGrids = 200;

    [Export] public int attempts = 100;

    [ExportGroup("Elements")]
    [Export] public WorldgenObject[] objects;
    [Export] public int[] weight;
    [Export] public int[] size;
    [Export] public Vector2 offsett;
    [Export] public int MinAmount = 1;
    [Export] public int MaxAmount = 1;
    }
}