using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int[] inventoryItems = new int[50];
    public int[] inventoryItemCounts = new int[50];
    public int[] unlockedRecipes = new int[100];
    public PlayerData(UseItem u, FoundRecipes cm) {
    	// int currentSpot = 0;

    	for (int i =0;i<50;i++) {
    		inventoryItems[i]=-1;
    	}
    	for (int i =0;i<100;i++) {
    		unlockedRecipes[i]=-1;
    	}
    	foreach (KeyValuePair<int, Item> pair in u.inventoryItems) {
    		inventoryItems[pair.Key]=pair.Value.id;
    		inventoryItemCounts[pair.Key]=pair.Value.stats["Current Count"];
    	}
    	// currentSpot = 0;
    	foreach (KeyValuePair<int, Recipe> pair in cm.GetRecipes()) {
    		unlockedRecipes[pair.Key]=pair.Value.id;
    	}
    }
}
