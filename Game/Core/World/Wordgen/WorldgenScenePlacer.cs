using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airship_Game.Game.Core.World.Wordgen
{
    [GlobalClass]
    public  partial class WorldgenScenePlacer:WorldgenObject
    {
        [Export] public PackedScene Scene { get; set; }

        public override void _Place(Vector2 position)
        {
            Node2D scene = (Node2D)Scene.Instantiate();
            scene.GlobalPosition = position;
            GameWorld.Instance.AddChild(scene);
        }
    }
}