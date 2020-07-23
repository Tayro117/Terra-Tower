using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeUse : OnUseItem
{
	private int health = 100;
	public TreeUse() {
		spawnChances = new Dictionary<int, int>();
        spawnChances.Add(3, 1000);
        itemID=15;
	}
    public override bool hit(Item item){
    	if (item != null && item.stats.ContainsKey("Axe Power")) {
            StartCoroutine(playAudio());
    		health-=item.stats["Axe Power"];
    		if (health <= 0) {
                System.Random rand = new System.Random();
                int c = rand.Next(10,15);
                for (int i =0;i<c;i++) {
                    spawnItem();
                }
                return true;
    		}
    	}
        return false;
    }
}
