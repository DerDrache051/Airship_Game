using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Godot;
public interface ISerializable
{
    public string Serialize();
    public void Deserialize(string data);
    public Godot.Collections.Dictionary<String,String> SerializeComponents(Godot.Collections.Dictionary<String,String> dict);
    public void DeserializeComponents(Godot.Collections.Dictionary<String,String> dict);
    public void finishLoad();

}
public class SaveAndLoad(){
    
    public static void Save(){
        Godot.Collections.Dictionary<String,Godot.Collections.Dictionary<String,String>> dict=new Godot.Collections.Dictionary<String,Godot.Collections.Dictionary<String,String>>();
        foreach(Node node in Registry.instance.GetTree().GetNodesInGroup("Save")){
            if(node is ISerializable serializable){
                dict.Add(node.GetPath(),serializable.SerializeComponents(new Godot.Collections.Dictionary<String,String>()));
            }
        }
        FileAccess file= FileAccess.Open("user://save.save",FileAccess.ModeFlags.Write);
        GD.Print(FileAccess.GetOpenError());
        file.StoreString(Godot.Json.Stringify(dict, "\t"));
        file.Close();
    }
    public static void Load(){
        if(!FileAccess.FileExists("user://save.save")){
            GD.Print("no save found");
            return;
        }
        FileAccess file= FileAccess.Open("user://save.save",FileAccess.ModeFlags.Read);
        String data=file.GetAsText();
        file.Close();
        Godot.Collections.Dictionary<String,Godot.Collections.Dictionary<String,String>> dict=Godot.Json.ParseString(data).As<Godot.Collections.Dictionary<String,Godot.Collections.Dictionary<String,String>>>();
        SceneTree Tree=Registry.instance.GetTree();
        LinkedList<ISerializable> serializables=new LinkedList<ISerializable>();
        foreach(String path in dict.Keys){
            GD.Print("loading:"+path);
            Godot.Collections.Dictionary<String,String> components=dict[path];
            PackedScene scene=GD.Load<PackedScene>(components["SceneFile"]);
            Node node=scene.Instantiate();
            if(node is ISerializable serializable){
                serializable.DeserializeComponents(components);
                String ParentPath=path.Substring(0,path.LastIndexOf("/"));
                Node ParentNode=Tree.Root.GetNode(ParentPath);
                if(ParentNode==null) continue;
                ParentNode.AddChild(node);
                serializables.AddLast(serializable);
            }
            node.AddToGroup("Save");
           
        }
        foreach(ISerializable serializable in serializables){
            serializable.finishLoad();
        }
        
    } 
}