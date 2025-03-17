using Godot;
using Godot.Collections;
using System;

public partial class Item : Node,ISerializable
{
	[Export] public int MaxStackSize;
	[Export] public String ID;
	[Export] public String DisplayName;
	[Export] public Texture2D ItemTexture;
	[Export] public String CustomData;
	[Export] public CursorStates cursorState=CursorStates.DefaultState;
	public int StackSize=1;

	public int Slot=-1;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public virtual void onPrimaryInteraction(Node node){}
	public virtual void onSecondaryInteraction(Node node){}

	public virtual bool isSame(Item item){
		return item!=null && item.ID==ID&& item.CustomData==CustomData;
	}
	public Item setLength(int length){
		StackSize=length;
		return this;
	}

    public string Serialize()
    {
		return Godot.Json.Stringify(SerializeComponents(new Godot.Collections.Dictionary<String,String>()));
    }

    public void Deserialize(string data)
    {
		DeserializeComponents(Godot.Json.ParseString(data).As<Godot.Collections.Dictionary<String,String>>());
    }

    public virtual Dictionary<string, string> SerializeComponents(Dictionary<string, string> dict)
    {
        dict.Add("StackSize",StackSize+"");
		dict.Add("SceneFile",SceneFilePath+"");
		dict.Add("Slot",Slot+"");	
		return dict;
    }

    public virtual void DeserializeComponents(Dictionary<string, string> dict)
    {
		StackSize=int.Parse(dict["StackSize"]);
		SceneFilePath=dict["SceneFile"];
		Slot=int.Parse(dict["Slot"]);
    }

    public virtual void finishLoad()
    {
		
    }
}
