using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    public int id;
    public string name;
    public string tooltip;
    public int itemReference;
    public Dictionary<int, int> components;


    public Recipe(int id, string name, string tooltip, int itemReference, Dictionary<int,int> components){
    	this.id=id;
    	this.name=name;
    	this.tooltip=tooltip;
    	this.itemReference=itemReference;
    	this.components=components;
    }

}
