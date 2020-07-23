using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUse : OnUseItem
{
	private int health = 1;
	public ChestUse() {
		spawnChances = new Dictionary<int, int>();
        spawnChances.Add(14, 1000);
        itemID=14;
	}
    public override bool hit(Item item){
        GameObject player = GameObject.Find("Player");
        int floor = player.GetComponent<Movement>().GetFloorNum();
        ChestData cd = SaveSystem.LoadChest("" +startX + "" + startY + "" + floor);
        if (cd != null) {
            for (int i = 0;i<30;i++) {
                if (cd.chestItems[i] != -1) {
                    return false;
                }
            }
        }
    	if (item != null && item.stats.ContainsKey("Axe Power")) {
    		health-=item.stats["Axe Power"];
    		if (health <= 0) {
    			spawnItem();
                return true;
    		}
    	}

        return false;
    }
    public override bool Interact() {
        GameObject player = GameObject.Find("Player");
        int floor = player.GetComponent<Movement>().GetFloorNum();
        player.GetComponent<Movement>().OpenChestMenu("" +startX + "" + startY + "" + floor);
        return false;
    }
}