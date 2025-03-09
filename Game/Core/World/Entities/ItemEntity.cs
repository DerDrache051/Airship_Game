using Godot;
using System;

public partial class ItemEntity : Entity
{
	public Item item;
	[Export] Sprite2D sprite;
	public override void _PhysicsProcess(double delta)
	{
		sprite.Texture=item.ItemTexture;
		Godot.Collections.Array<Node> nodes = GetTree().GetNodesInGroup("Entity");
		foreach (Node node in nodes)
		{
			if (node is PlayerCharacter)
			{
				PlayerCharacter player = (PlayerCharacter)node;
				if (player.Position.DistanceTo(Position) < 12)
				{
					player.inventory.AddItem(item);
					QueueFree();
				}
			}
		}
		base._PhysicsProcess(delta);
	}
}
