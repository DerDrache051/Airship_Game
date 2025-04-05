using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airship_Game.Game.Core.World.Wordgen
{
    [GlobalClass]
    public abstract partial class WorldgenObject : Resource
    {
        public abstract void _Place(Vector2 position);
    }

}