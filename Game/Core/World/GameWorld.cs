using Godot;
using System;

public partial class GameWorld : Node2D
{
	public static GameWorld Instance { get; set; }

	[Export] public Area2D worldBoundary;
	[Export] public Sprite2D worldBoundarySprite;
	[Export] public Area2D spawnArea;
	[Export] public Area2D finishArea;
	[Export] public Vector2 WorldSize=new Vector2(2000,1000);
	[Export] public int SpawnAreaSize=200;
	[Export] public int FinishAreaSize=200;
	static bool start;
    // Called when the node enters the scene tree for the first time.
    public override void _EnterTree()
    {
		Instance = this;
		base._EnterTree();
	}
    public override void _Ready()
    {
		ClientStatics.UI_Selector.ShowGUI(ClientStatics.UI_Selector.InGameOverlay);
		setShape(WorldSize*1.5f,worldBoundary,new Vector2(0-(SpawnAreaSize/2),0));
		setShape(new Vector2(SpawnAreaSize,WorldSize.Y),spawnArea,new Vector2(0-(SpawnAreaSize/2),0));
		setShape(new Vector2(FinishAreaSize,WorldSize.Y),finishArea,new Vector2((WorldSize.X-(SpawnAreaSize/2))-FinishAreaSize,0));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if(!start){
			start = true;
		}
	}
	public void setShape(Vector2 size,Area2D area,Vector2 pos){
		RectangleShape2D shape = new RectangleShape2D();
		shape.Size = size;
		area.Position = pos;
		CollisionShape2D collisionShape = new CollisionShape2D();
		collisionShape.Shape = shape;
		collisionShape.Position=size/2;
		area.AddChild(collisionShape);
		
	}
}
