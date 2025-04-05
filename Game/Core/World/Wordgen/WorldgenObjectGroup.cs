using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airship_Game.Game.Core.World.Wordgen
{
    [GlobalClass]
    public partial class WorldgenObjectGroup:WorldgenObject
    {
        [Export] public WorldgenObject[] Objects;
        [Export] public float Radius;

        public override void _Place(Vector2 position)
        {
            foreach (WorldgenObject obj in Objects)
            {
                obj._Place(position+new Vector2(GD.Randf()*Radius,GD.Randf()*Radius));
            }
        }
    }
}