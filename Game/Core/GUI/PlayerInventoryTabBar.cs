using Godot;
using System;

public partial class PlayerInventoryTabBar : TabBar
{
	[Export] public Control[] Tabs=new Control[0];
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		for(int i=0;i<Tabs.Length;i++){
			if(Tabs[i]==null)continue;
			if(i==CurrentTab){
				Tabs[i].Show();
			}else{
				Tabs[i].Hide();
			}
		}
	}
}
