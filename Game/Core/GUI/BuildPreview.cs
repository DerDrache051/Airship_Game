using Godot;
using System;

public partial class BuildPreview : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] Texture2D breakRectangle;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(ClientStatics.player==null){Visible=false;return;};
		if(ClientStatics.player.inventory==null){Visible=false;return;};
		Grid grid=ClientStatics.player.lastCollidedGrid;
		if(grid==null){Visible=false;return;};
		Item item=ClientStatics.player.inventory.GetItem(ClientStatics.player.SelectedItemSlot);
		if(item==null){Visible=false;return;};
		if(item.cursorState==CursorStates.Build){
			Visible=true;
			
			GlobalPosition=grid.snapToNearestTile(ClientStatics.player.CurserPosition);
			Texture=item.ItemTexture;
			this.Modulate=new Color(1,1,1,0.5f);
			if(item is TileItem tileItem &&tileItem.tile!=null){
				Transform=tileItem.tile.Transform;
				GlobalPosition=grid.snapToNearestTile(ClientStatics.player.CurserPosition);
				Scale=new Vector2(tileItem.tile.Tilematerial.SizeX*grid.TilePixelSize,tileItem.tile.Tilematerial.SizeY*grid.TilePixelSize)/Texture.GetSize();
				GlobalPosition+=new Vector2(grid.TilePixelSize*(tileItem.tile.Tilematerial.SizeX-1),grid.TilePixelSize*(tileItem.tile.Tilematerial.SizeY-1))/2;
			}
		} 
		else if(item.cursorState==CursorStates.Break){
			
			Texture=breakRectangle;
			this.Modulate=new Color(1,1,1,0.5f);
			Tile tile=grid.getFirstTileAt(ClientStatics.player.CurserPosition);
			if(tile!=null){
				GlobalPosition=tile.GlobalPosition+new Vector2(grid.TilePixelSize*(tile.Tilematerial.SizeX-1),grid.TilePixelSize*(tile.Tilematerial.SizeY-1))/2;
				Scale=new Vector2(tile.Tilematerial.SizeX*grid.TilePixelSize,tile.Tilematerial.SizeY*grid.TilePixelSize)/Texture.GetSize();
				Visible=true;
			}
			else Visible=false;
		}
		else{
			Visible=false;
		}
	}
}
