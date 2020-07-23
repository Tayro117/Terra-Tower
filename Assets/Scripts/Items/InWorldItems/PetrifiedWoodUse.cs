using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetrifiedWoodUse : OnUseItem
{
    private int health = 5;
	public PetrifiedWoodUse() {
		spawnChances = new Dictionary<int, int>();
        spawnChances.Add(4, 1000);
        itemID=4;
	}
    public override bool hit(Item item){
    	if (item != null && item.stats.ContainsKey("Pickaxe Power")) {
    		health-=item.stats["Pickaxe Power"];
    		if (health <= 0) {
    			spawnItem();
                return true;
    		}
    	}
        return false;
    }
}
