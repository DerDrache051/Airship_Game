using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

public partial class MapPlacer : Node2D
{

    [ExportGroup("Spacing")]

    [Export] public int minDistancetoGrids = 0;

    [Export] public int maxDistancetoGrids = 200;

    [Export] public int attempts = 100;

    [ExportGroup("Elements")]
    [Export] public PackedScene[] scenes;
    [Export] public int[] weight;
    [Export] public int[] size;

    [Export] public Vector2 offsett;

    [Export] public int MinAmount = 1;
    [Export] public int MaxAmount = 1;

    Queue<MapPlacementHelper> helpers =new(); 
    List<Thread> threads = new();
    public override void _Ready()
    {
        int amount = GD.RandRange(MinAmount, MaxAmount);
        int selected = 0;
        int max = 0;
        for (int i = 0; i < weight.Length; i++)
        {
            max += weight[i];
        }
        int rand = GD.RandRange(0, max);
        for (int i = 0; i < weight.Length; i++)
        {
            rand -= weight[i];
            if (rand <= 0)
            {
                selected = i;
                break;
            }
        }
        if (scenes[selected] == null) return;
        for (int i = 0; i < amount; i++)
        {
            for (int j = 0; j < attempts; j++)
            {
                int x = (int)GD.RandRange(GameWorld.Instance.SpawnAreaSize, GameWorld.Instance.WorldSize.X-GameWorld.Instance.FinishAreaSize);
                int y = (int)GD.RandRange(0, GameWorld.Instance.WorldSize.Y);

                if (
                    (calculateDistanceToNearestGrid(new Vector2(x, y)) > minDistancetoGrids || minDistancetoGrids <= 0) &&
                    (calculateDistanceToNearestGrid(new Vector2(x, y)) < maxDistancetoGrids || maxDistancetoGrids <= 0)
                )
                {
                    MapPlacementHelper mapPlacementHelper = new MapPlacementHelper();
                    mapPlacementHelper.scene = scenes[selected];
                    mapPlacementHelper.GlobalPosition = new Vector2(x, y);
                    helpers.Enqueue(mapPlacementHelper);
                    GD.Print("Placed at" + x + "," + y);
                    break;
                }

            }
        }

    }
    public static float calculateDistanceToNearestGrid(Vector2 pos)
    {
        Godot.Collections.Array<Node> grids = GameWorld.Instance.GetTree().GetNodesInGroup("Grid");
        float dist = -1;
        foreach (Node node in grids)
        {
            if (node is Node2D node2d)
            {
                if (dist == -1 || node2d.GlobalPosition.DistanceTo(pos) < dist)
                {
                    dist = node2d.GlobalPosition.DistanceTo(pos);
                }
            }
        }
        return dist;
    }
    public override void _Process(double delta)
    {
        if (helpers.Count > 0)
        {
            MapPlacementHelper helper = helpers.Dequeue();
            helper._Place();
        }
    }
}
