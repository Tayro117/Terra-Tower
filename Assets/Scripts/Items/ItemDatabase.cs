using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
	public List<Item> items = new List<Item>();


	private void Awake() {
		BuildDatabase();
	}
	void BuildDatabase() {
		items.Add(new Item(0, "Pickaxe", "Just a regular pickaxe", new Dictionary<string,int>
				{
					{"Damage", 10},
					{"Pickaxe Power", 10},
					{"Durability", 10},
					{"Value", 10},
					{"Max Count", 1},
					{"Current Count", 1},
					{"Axe Power", 4}

				}));
		items.Add(new Item(1, "Stone Pickaxe", "Just a regular pickaxe", new Dictionary<string,int>
				{
					{"Damage", 15},
					{"Pickaxe Power", 20},
					{"Durability", 10},
					{"Value", 10},
					{"Max Count", 1},
					{"Current Count", 1},
					{"Axe Power", 4}
				}));
		items.Add(new Item(2, "Stone", "Some rock", new Dictionary<string,int>
				{
					{"Value", 1},
					{"Max Count", 99},
					{"Current Count", 1},
					{"Placeable Width", 1},
					{"Placeable Length", 1},
					{"Sprite Tile Height", 1},
					{"Sprite Tile Width", 1}
				}));
		items.Add(new Item(3, "Wood", "Some wood", new Dictionary<string,int>
				{
					{"Value", 1},
					{"Max Count", 99},
					{"Current Count", 1},
					{"Placeable Width", 1},
					{"Placeable Length", 1},
					{"Sprite Tile Height", 1},
					{"Sprite Tile Width", 1}
				}));
		items.Add(new Item(4, "Petrified Wood", "Some petrified wood", new Dictionary<string, int>
				{
					{"Value", 5000},
					{"Max Count", 99},
					{"Current Count", 1},
					{"Placeable Size", 1},
					{"Placeable Width", 1},
					{"Placeable Length", 1},
					{"Sprite Tile Height", 1},
					{"Sprite Tile Width", 1}
				}));
		items.Add(new Item(5, "Stone Axe", "A stone axe", new Dictionary<string, int>
				{
					{"Damage", 15},
					{"Durability", 10},
					{"Value", 10},
					{"Max Count", 1},
					{"Current Count", 1},
					{"Axe Power", 10}
				}));
		items.Add(new Item(6, "Stone Hoe", "A stone hoe", new Dictionary<string, int>
				{
					{"Durability", 10},
					{"Value", 10},
					{"Max Count", 1},
					{"Current Count", 1},
					{"Hoe Power", 10}
				}));
		items.Add(new Item(7, "Stone Watering Can", "A stone watering can", new Dictionary<string, int>
				{
					{"Durability", 10},
					{"Value", 10},
					{"Max Count", 1},
					{"Current Count", 1},
					{"Watering Power", 10}
				}));
		items.Add(new Item(8, "Mixed Seeds", "Who knows which seeds you'll plant", new Dictionary<string, int>
				{
					{"Value", 1},
					{"Max Count", 99},
					{"Current Count", 1},
					{"Dynamic Type", 9}, //TODO add random number generator to figure out the type
					{"Placeable Size", 1},
					{"Placeable Width", 1},
					{"Placeable Length", 1},
					{"Sprite Tile Height", 1},
					{"Sprite Tile Width", 1}
				}));
		items.Add(new Item(9, "Turnip Plant", "How did you find this description?", new Dictionary<string, int>
				{
					{"Value", 1},
					{"Max Count", 1},
					{"Current Count", 1},
					{"Placeable Width", 1},
					{"Placeable Length", 1},
					{"Sprite Tile Height", 1},
					{"Sprite Tile Width", 1}
				}));
		items.Add(new Item(10, "Turnip", "Just a simple turnip", new Dictionary<string, int>
				{
					{"Value", 50},
					{"Max Count", 99},
					{"Current Count", 1}
				}));
		items.Add(new Item(11, "Torch", "Just a simple torch", new Dictionary<string, int>
				{
					{"Value", 5},
					{"Max Count", 99},
					{"Current Count", 1},
					{"Placeable Size", 1},
					{"Placeable Width", 1},
					{"Placeable Length", 1},
					{"Sprite Tile Height", 1},
					{"Sprite Tile Width", 1}
				}));
		items.Add(new Item(12, "Thatch", "Plant materials used for construction", new Dictionary<string, int>
				{
					{"Value", 1},
					{"Max Count", 99},
					{"Current Count", 1},
					{"Placeable Width", 1},
					{"Placeable Length", 1},
					{"Sprite Tile Height", 1},
					{"Sprite Tile Width", 1}
				}));
		items.Add(new Item(13, "Table", "A simple table", new Dictionary<string, int>
				{
					{"Value", 1},
					{"Max Count", 99},
					{"Current Count", 1},
					{"Placeable Size", 2},
					{"Placeable Width", 2},
					{"Placeable Length", 2},
					{"Sprite Tile Height", 2},
					{"Sprite Tile Width", 2}
				}));
		items.Add(new Item(14, "Wooden Chest", "Something to store your items in", new Dictionary<string, int>
				{
					{"Value", 20},
					{"Max Count", 99},
					{"Current Count", 1},
					{"Placeable Size", 1},
					{"Placeable Width", 1},
					{"Placeable Length", 1},
					{"Sprite Tile Height", 1},
					{"Sprite Tile Width", 1}
				}));
		items.Add(new Item(15, "Tree", "idk how this got in your inventory", new Dictionary<string, int>
				{
					{"Value", 0},
					{"Max Count", 1},
					{"Current Count", 1},
					{"Placeable Size", 2},
					{"Placeable Width", 1},
					{"Placeable Length", 1},
					{"Sprite Tile Height", 5},
					{"Sprite Tile Width", 1}
				}));
		items.Add(new Item(16, "Pond", "idk how this got in your inventory", new Dictionary<string, int>
				{
					{"Value", 0},
					{"Max Count", 1},
					{"Current Count", 1},
					{"Placeable Width", 10},
					{"Placeable Length", 10},
					{"Sprite Tile Height", 10},
					{"Sprite Tile Width", 10}
				}));
		items.Add(new Item(17, "Copper Ore", "Smelt to get copper bars", new Dictionary<string, int>
				{
					{"Value", 15},
					{"Max Count", 99},
					{"Current Count", 1},
					{"Placeable Width", 1},
					{"Placeable Length", 1},
					{"Sprite Tile Height", 1},
					{"Sprite Tile Width", 1}
				}));
		items.Add(new Item(18, "Iron Ore", "Smelt to get iron bars", new Dictionary<string, int>
				{
					{"Value", 20},
					{"Max Count", 99},
					{"Current Count", 1},
					{"Placeable Width", 1},
					{"Placeable Length", 1},
					{"Sprite Tile Height", 1},
					{"Sprite Tile Width", 1}
				}));

	}
    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
