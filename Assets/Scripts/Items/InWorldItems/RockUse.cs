using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockUse : OnUseItem
{
	private int health = 20;
	public RockUse() {
		spawnChances = new Dictionary<int, int>();
        spawnChances.Add(2, 1000);
        itemID=2;
	}
	void Start() {
	}
    public override bool hit(Item item){
    	if (item != null && item.stats.ContainsKey("Pickaxe Power")) {
            StartCoroutine(playAudio());
    		health-=item.stats["Pickaxe Power"];
    		if (health <= 0) {
    			spawnItem();
                return true;
    		}
    	}
        return false;
    }
}
