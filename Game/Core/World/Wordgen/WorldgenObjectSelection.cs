using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airship_Game.Game.Core.World.Wordgen
{
    [GlobalClass]
    public partial class WorldgenObjectSelection:WorldgenObject
    {
        [Export] public WorldgenObject[] Objects;
        [Export] public float[] Weight;

        public override void _Place(Vector2 position)
        {
            float max=0;
            foreach(float val in Weight){
                max+=val;
            }
            float rand=GD.Randf()*max;
            for(int i=0;i<Weight.Length;i++){
                rand-=Weight[i];
                if(rand<=0){
                    Objects[i]._Place(position);
                    break;
                }
            }
        }
    }
}