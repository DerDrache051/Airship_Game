using Godot;
using System;

public partial class EntitySpawnList : Godot.ItemList
{
	bool UpdateList = true;

	PackedScene[] EntityScenes;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public delegate void OnItemSelected(int item);
	public event OnItemSelected onItemSelected;
	public override void _Process(double delta)
	{
		if (UpdateList)
		{
			EntityScenes = new PackedScene[Registry.instance.Entities.Length];
			int itemNr = 0;
			Clear();
			for (int i = 0; i < Registry.instance.Entities.Length; i++)
			{
				Entity ent = Registry.instance.Entities[i].Instantiate<Entity>();
				AddItem(ent.DisplayName);
				itemNr++;
			}
			UpdateList = false;
		}
		for(int i = 0; i < ItemCount; i++)
		{
			if (IsSelected(i))
			{
				Entity ent = Registry.instance.Entities[i].Instantiate<Entity>();
				SpawnEntity(ent);
				Deselect(i);
			}
		}
	}
	public void SpawnEntity(Entity entity){
		if(entity==null)return;
		entity.GlobalPosition=ClientStatics.player.GlobalPosition;
		GameWorld.Instance.AddChild(entity);
	}
}
