using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChestData
{
    public int[] chestItems = new int[30];
    public int[] inventoryItemCounts = new int[30];
    public ChestData(GameObject buttons) {
    	// int currentSpot = 0;

    	for (int i =0;i<30;i++) {
    		chestItems[i]=-1;
    	}
    	for (int i =0;i<30;i++) {
    		inventoryItemCounts[i]=-1;
    	}
    	for (int i =50;i<80;i++) {
            var obj = buttons.transform.GetChild(i);
            if (obj.childCount > 0) {
                Item item = obj.GetChild(0).gameObject.GetComponent<DroppedItemReference>().GetItem();
        		chestItems[i-50]=item.id;
        		inventoryItemCounts[i-50]=item.stats["Current Count"];
            }
    	}
    }
}
