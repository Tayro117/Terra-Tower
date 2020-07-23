using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoundRecipes : MonoBehaviour
{
	public GameObject CraftRecipePrefab;
    public GameObject ComponentPrefab;
    public GameObject player;
    public GameObject Panel;
	private Dictionary<int, Recipe> foundRecipes = new Dictionary<int, Recipe>();
    private List<Recipe> recipes;
    private List<Item> itemList;
    private Dictionary<int, GameObject> recipeObjects = new Dictionary<int, GameObject>();
    void Start()
    {
        recipes = GameObject.Find("RecipeDatabaseObject").GetComponent<RecipeDatabase>().recipes;
        itemList = GameObject.Find("ItemDatabaseObject").GetComponent<ItemDatabase>().items;
        AddDefaultRecipes();

    }
    public void AddDefaultRecipes() {
        AddToRecipes(recipes[0]);
        AddToRecipes(recipes[1]);
        AddToRecipes(recipes[2]);
        AddToRecipes(recipes[3]);
        AddToRecipes(recipes[4]);
        AddToRecipes(recipes[5]);
        AddToRecipes(recipes[6]);
        AddToRecipes(recipes[7]);
    }
    public void ClearRecipes() {
        foreach (KeyValuePair<int, GameObject> pair in recipeObjects) {
            Destroy(pair.Value);
        }
        recipeObjects = new Dictionary<int, GameObject>();
        foundRecipes = new Dictionary<int, Recipe>();
    }

    public void AddRecipe(int i) {
        Debug.Log(i);
        AddToRecipes(recipes[i]);
    }
    public Dictionary<int, Recipe> GetRecipes() {
        return foundRecipes;
    }
    public bool CheckIfFound(int recipeID) {
        if (foundRecipes.ContainsKey(recipeID)) {
            return true;
        }
        return false;
    }
    public void AddToRecipes(Recipe recipe) {
        Debug.Log(recipe.name);
        GameObject pf = Instantiate(CraftRecipePrefab, Vector3.zero, Quaternion.identity);
        pf.transform.SetParent(Panel.transform, false);
        pf.gameObject.name = recipe.name + " Recipe";
        pf.transform.localScale = Vector3.one;

        pf.GetComponent<Image>().sprite = itemList[recipe.itemReference].icon;
        pf.AddComponent<OpenPage>();
        pf.GetComponent<OpenPage>().setup(itemList[recipe.itemReference],recipe, ComponentPrefab, itemList);


        
        recipeObjects.Add(recipe.id, pf);
        foundRecipes.Add(recipe.id, recipe);
    }
    
}
