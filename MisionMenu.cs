using Godot;
using System;

public partial class MisionMenu : Control
{
    public void StartMission(){
        GameWorld gw=GD.Load<PackedScene>("res://Game/Core/World/game_world.tscn").Instantiate<GameWorld>();
        ClientStatics.UI_Selector.ShownGUI_ID=6;
        ClientStatics.UI_Selector.updateGUI();
        gw.Name="Game_World";
        GetTree().Root.AddChild(gw);{
        }
    }
}
