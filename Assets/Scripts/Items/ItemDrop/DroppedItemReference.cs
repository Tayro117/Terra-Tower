using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemReference : MonoBehaviour
{
    private Item item;
    public void SetItem(Item i){
    	item = i;
    }
    public Item GetItem() {
    	return item;
    }
}
