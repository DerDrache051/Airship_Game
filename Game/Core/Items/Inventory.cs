using Godot;
using Godot.Collections;
using System;

public partial class Inventory : Node,ISerializable
{
	// Called when the node enters the scene tree for the first time.
	[Export] public int InventorySize=6;
	[Export] public int StackSizeMultiplier=1;
	private Item[] Items;
	[Export] public PackedScene[] StartingItems;

	public int SelectedItem=1;//the currently Selected item for GUI

	public override void _Ready()
	{
		Items=new Item[InventorySize];
		for(int i=0;i<StartingItems.Length;i++){
			if(StartingItems[i]==null)continue;
			Node node=StartingItems[i].Instantiate();
			if(node!=null&&node is Item)AddItem((Item)node);
		}
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public bool AddItem(Item item){
		int firstEmptyStack=0;
		for(int i=0; i<InventorySize;i++){
			if(Items[i]==null&&firstEmptyStack==0){
				firstEmptyStack=i;
			}
			if (Items[i]!=null&&Items[i].isSame(item)&&(item.StackSize+Items[i].StackSize<=item.MaxStackSize*StackSizeMultiplier)){
				Items[i].StackSize+=item.StackSize;
				return true;
			}
		}
		if(firstEmptyStack==0)return false;
		Items[firstEmptyStack]=item;
		AddChild(item);
		item.Slot=firstEmptyStack;
		if(IsInGroup("Save"))item.AddToGroup("Save");
		return true;
	}
	public bool canAddItem(Item item){
		int firstEmptyStack=0;
		for(int i=0; i<InventorySize;i++){
			if(Items[i]==null&&firstEmptyStack==0){
				firstEmptyStack=i;
			}
			if (Items[i]!=null&&Items[i].isSame(item)&&(item.StackSize+Items[i].StackSize<=item.MaxStackSize*StackSizeMultiplier)){
				return true;
			}
		}
		if(firstEmptyStack==0)return false;
		return true;
	}

	public bool addItemToSlot(Item item,int slot){
		if(Items[slot]==null){
			Items[slot]=item;
			AddChild(item);
			return true;
		}
		if(Items[slot].isSame(item)&&(item.StackSize+Items[slot].StackSize<=item.MaxStackSize*StackSizeMultiplier)){
			Items[slot].StackSize+=item.StackSize;
			return true;
		}
		return false;
	}
	public bool canAddItemToSlot(Item item,int slot){
		if(Items[slot]==null){
			return true;
		}
		if(Items[slot].isSame(item)&&(item.StackSize+Items[slot].StackSize<=item.MaxStackSize*StackSizeMultiplier)){
			return true;
		}
		return false;
	}
	public bool removeItem(Item item){
		for(int i=0; i<InventorySize;i++){
			if(Items[i]!=null&&Items[i].isSame(item)){
				Items[i].QueueFree();
				Items[i]=null;
				return true;
			}
		}
		return false;
	}
	public int ShrinkStack(int slot,int amount=1){
		if(Items[slot]==null)return 0;
		int size=Items[slot].StackSize;
		Items[slot].StackSize-=amount;
		if(Items[slot].StackSize<=0){
			Items[slot].QueueFree();
			Items[slot]=null;
			return size;
		}
		return size-Items[slot].StackSize;
	}
	public int getItemCount(String itemID){
		int count=0;
		for(int i=0; i<InventorySize;i++){
			if(Items[i]!=null&&Items[i].ID==itemID){
				count+=Items[i].StackSize;
			}
		}
		return count;
	}
	public void swapItems(int from,int to,Inventory targetInventory=null){
		if(targetInventory==null)targetInventory=this;
		if(Items[from]!=null)Items[from].Slot=to;
		if(targetInventory.Items[to]!=null)targetInventory.Items[to].Slot=from;
		Item item=Items[from];
		Items[from]=targetInventory.Items[to];
		targetInventory.Items[to]=item;
	}
	public void removeItems(String itemID,int amount){
		int n=amount;
		for(int i=0; i<InventorySize;i++){
			if(Items[i]!=null&&Items[i].ID==itemID){
				n-=ShrinkStack(i,amount);
				if(n<=0)break;
			}
		}
	}
	public int getEmptySlotCount(){
		int count=0;
		for(int i=0; i<InventorySize;i++){
			if(Items[i]==null)count++;
		}
		return count;
	}

	public Item GetItem(int slot){return Items[slot];}

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
        dict.Add("SceneFile",SceneFilePath);
		return dict;
    }

    public virtual void DeserializeComponents(Dictionary<string, string> dict)
    {
		SceneFilePath=dict["SceneFile"];
    }

    public virtual void finishLoad()
    {
		GD.Print("loading Items");
        foreach(Node node in GetChildren()){
			
			if(node is Item item)addItemToSlot(item,item.Slot);
		}
	}
	public int getItemSlot(Item item){
		for(int i=0; i<InventorySize;i++){
			if(Items[i]==item){
				return i;
			}
		}
		return -1;
	}

}
