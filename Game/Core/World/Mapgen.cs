using Godot;
using System;
using Airship_Game.Game.Core.World.Wordgen;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class Mapgen : Node
{
    public static Mapgen Instance { get; private set; }
    public static String Biome = "Windswept Isles";
    public static ulong Seed = 696969696;

    int WorldgenTreadCount = 0;
    int FinishedWorldgenTreads = 0;

    public override void _Ready()
    {
        GD.Seed(Seed);
        Instance = this;
        LinkedList<WorldgenObjectPlacer> placers = new LinkedList<WorldgenObjectPlacer>();
        DirAccess dir = DirAccess.Open("res://Game/Content/Worldgen/" + Biome);
        foreach (String file in dir.GetFiles())
        {
            WorldgenObjectPlacer placer = GD.Load<WorldgenObjectPlacer>("res://Game/Content/Worldgen/" + Biome + "/" + file);
            placers.AddLast(placer);
            WorldgenTreadCount++;
        }
        foreach (WorldgenObjectPlacer placer in placers)
        {
            Task.Run(() => PlacePlacer(placer));
        }
        while (FinishedWorldgenTreads < WorldgenTreadCount) { }//Scary


    }
    private void PlacePlacer(WorldgenObjectPlacer placer)
    {
        placer._Place();
        FinishedWorldgenTreads++;
    }

}
