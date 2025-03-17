using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        foreach(String path in dict.Keys){
            LoadNode(dict,path,Tree);
        }
        foreach(Node node in Tree.GetNodesInGroup("Save")){
            if(node is ISerializable serializable){
                serializable.finishLoad();
            }
        }
 
    }
    private static void LoadNode(Godot.Collections.Dictionary<String,Godot.Collections.Dictionary<String,String>> dict,String path,SceneTree Tree){
        GD.Print("loading:"+path);
            Node existingNode=Tree.Root.GetNodeOrNull(path);
            if(existingNode!=null) return;
            Godot.Collections.Dictionary<String,String> components=dict[path];
            PackedScene scene=GD.Load<PackedScene>(components["SceneFile"]);
            Node node=scene.Instantiate();
            if(node is ISerializable serializable){
                serializable.DeserializeComponents(components);
                String ParentPath=path.Substring(0,path.LastIndexOf("/"));
                Node ParentNode=Tree.Root.GetNodeOrNull(ParentPath);
                if(ParentNode==null){ 
                    if(dict.ContainsKey(ParentPath)){
                        LoadNode(dict,ParentPath,Tree);
                    }
                    else
                    GD.Print("parent not found"+ParentPath);
                    return;
                }
                ParentNode.AddChild(node);
            }
            dict.Remove(path);
            node.AddToGroup("Save");
    } 
}