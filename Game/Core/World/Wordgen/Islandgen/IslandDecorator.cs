using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airship_Game.Game.Core.World.Wordgen.Islandgen
{
    [GlobalClass]
    public abstract partial class IslandDecorator : Resource
    {
        [Export] public float chance;
        public abstract void _Place(Vector2I position,Grid grid);
    }
}