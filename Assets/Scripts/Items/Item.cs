using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id;
    public string name;
    public string description;
    public Sprite icon;
    public Dictionary<string, int> stats;


    public Item(int id, string name, string description, Dictionary<string,int> stats){
    	this.id=id;
    	this.name=name;
    	this.description=description;
    	this.icon=Resources.Load<Sprite>("Sprites/Items/"+toFileName(name));
    	this.stats=stats;
    }
    public Item(Item item){
    	this.id=item.id;
    	this.name=item.name;
    	this.description=item.description;
    	this.icon=Resources.Load<Sprite>("Sprites/Items/"+toFileName(item.name));
    	this.stats=new Dictionary<string, int>();
        foreach(KeyValuePair<string, int> s in item.stats) {
            this.stats.Add(s.Key, s.Value);
        }


    }

    private string toFileName(string name) {
        return name.ToLower().Replace(' ','_');
    }
}
