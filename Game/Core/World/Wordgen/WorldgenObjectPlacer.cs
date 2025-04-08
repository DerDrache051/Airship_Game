using System.Threading.Tasks;
using Godot;
using Godot.NativeInterop;



namespace Airship_Game.Game.Core.World.Wordgen

{
    [GlobalClass]
    public partial class WorldgenObjectPlacer : Resource
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

        private int WordgenThreads = 0;
        private int FinishedWorldgenTreads = 0;
        public virtual void _Place()
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
            WordgenThreads = amount;
            for (int i = 0; i < amount; i++)
            {
                Task.Run(() => PlaceWGObject(objects[selected]));
            }
            while(FinishedWorldgenTreads<WordgenThreads){}//Scary
        }
        private void PlaceWGObject(WorldgenObject obj)
        {
            int x = (int)GD.RandRange(GameWorld.Instance.SpawnAreaSize, GameWorld.Instance.WorldSize.X - GameWorld.Instance.FinishAreaSize);
            int y = (int)GD.RandRange(0, GameWorld.Instance.WorldSize.Y);
            obj._Place(new Vector2(x, y)-offsett);
            FinishedWorldgenTreads++;
        }
    }


}