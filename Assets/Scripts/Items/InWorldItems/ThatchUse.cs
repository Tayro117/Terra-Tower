using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThatchUse : OnUseItem
{
	public ThatchUse() {
		spawnChances = new Dictionary<int, int>();
        spawnChances.Add(12, 1000);
        spawnChances.Add(8, 300);
        itemID=12;
	}
    public override bool hit(Item item){
    	StartCoroutine(playAudio());
		spawnItem();
        return true;
    }
}
