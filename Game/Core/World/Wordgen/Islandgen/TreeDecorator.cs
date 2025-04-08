using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace Airship_Game.Game.Core.World.Wordgen.Islandgen
{
    [GlobalClass]


    public partial class TreeDecorator : IslandDecorator
    {
        [Export] public PackedScene Base { get; set; }
        [Export] public PackedScene Stem { get; set; }
        [Export] public PackedScene Leafs { get; set; }

        [Export] public int Height;
        [Export] public int HeightVariance;
        [Export] public int LeafSize;
        [Export] public int LeafSizeVariance;
        public override void _Place(Vector2I position, Grid grid)
        {
            if (GD.Randf() > chance) return;
            int height = Height + GD.RandRange(-HeightVariance, HeightVariance);
            int leafSize = LeafSize + GD.RandRange(-LeafSizeVariance, LeafSizeVariance);
            grid.addTile(Base, position.X, position.Y-1);
        }
    }
}