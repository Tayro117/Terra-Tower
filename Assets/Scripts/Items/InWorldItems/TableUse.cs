using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableUse : OnUseItem
{
	private int health = 1;
	public TableUse() {
		spawnChances = new Dictionary<int, int>();
        spawnChances.Add(13, 1000);
        itemID=13;
	}
    public override bool hit(Item item){
    	if (item != null && item.stats.ContainsKey("Axe Power")) {
    		health-=item.stats["Axe Power"];
    		if (health <= 0) {
    			spawnItem();
                return true;
    		}
    	}
        return false;
    }
}