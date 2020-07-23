using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDatabase : MonoBehaviour
{
	public List<Recipe> recipes = new List<Recipe>();


	private void Awake() {
		BuildDatabase();
	}
	void BuildDatabase() {
		recipes.Add(new Recipe(0, "Stone Pickaxe", "Better than what you've got now :)", (int)ItemID.Stone_Pickaxe, new Dictionary<int,int>
				{
					{(int)ItemID.Stone, 10},
					{(int)ItemID.Wood, 5}
					}));
		recipes.Add(new Recipe(1, "Petrified Wood", "Better than wood?", (int)ItemID.Petrified_Wood, new Dictionary<int, int>
				{
					{(int)ItemID.Stone, 1},
					{(int)ItemID.Wood, 1}
				}));
		recipes.Add(new Recipe(2, "Stone Axe", "Can chop some wood now :)", (int)ItemID.Stone_Axe, new Dictionary<int, int>
				{
					{(int)ItemID.Stone, 5},
					{(int)ItemID.Wood, 5}
				}));
		recipes.Add(new Recipe(3, "Stone Hoe", "Can till the ground for crops now :)", (int)ItemID.Stone_Hoe, new Dictionary<int, int>
				{
					{(int)ItemID.Stone, 5},
					{(int)ItemID.Wood, 5}
				}));
		recipes.Add(new Recipe(4, "Stone Watering Can", "Can water :)", (int)ItemID.Stone_Watering_Can, new Dictionary<int, int>
				{
					{(int)ItemID.Stone, 5},
				}));
		recipes.Add(new Recipe(5, "Torch", "Light up your surroundings with this simple trick :)", (int)ItemID.Torch, new Dictionary<int, int>
				{
					{(int)ItemID.Wood, 3},
				}));
		recipes.Add(new Recipe(6, "Table", "Table :)", (int)ItemID.Table, new Dictionary<int, int>
				{
					{(int)ItemID.Wood, 10},
				}));
		recipes.Add(new Recipe(7, "Wooden Chest", "Wooden Chest :)", (int)ItemID.Wooden_Chest, new Dictionary<int, int>
				{
					{(int)ItemID.Wood, 20},
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
public enum ItemID {
	Pickaxe = 0,
	Stone_Pickaxe,
	Stone,
	Wood,
	Petrified_Wood,
	Stone_Axe,
	Stone_Hoe,
	Stone_Watering_Can,
	Mixed_Seeds,
	Turnip_Plant,
	Turnip,
	Torch,
	Petrified_Wood_Recipe,
	Table,
	Wooden_Chest
}
