using Godot;
using System;
using System.Collections.Generic;

public partial class Autocrafter : InventoryTile
{
    [Export] public int Speed;

    public List<Recipe> currentRecipes = new List<Recipe>();

    public int Progress;

    public override void _Ready()
    {
        ProcessMode=ProcessModeEnum.Pausable;
        base._Ready();
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
    }


}
