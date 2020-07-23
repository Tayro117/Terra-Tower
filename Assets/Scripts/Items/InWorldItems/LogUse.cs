using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogUse : OnUseItem
{
	private int health = 10;
	public LogUse() {
		spawnChances = new Dictionary<int, int>();
        spawnChances.Add(3, 1000);
        itemID=3;
	}
    public override bool hit(Item item){
    	if (item != null && item.stats.ContainsKey("Axe Power")) {
            StartCoroutine(playAudio());
    		health-=item.stats["Axe Power"];
    		if (health <= 0) {
    			spawnItem();
                return true;
    		}
    	}
        return false;
    }
}
