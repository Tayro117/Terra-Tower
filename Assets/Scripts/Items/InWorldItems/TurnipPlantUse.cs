using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnipPlantUse : OnUseItem
{

	private List<Sprite> growthCycle = new List<Sprite>();
	void Awake() {
		growthCycle.Add(Resources.Load<Sprite>("Sprites/Tiles/Plants/Turnips/Turnip Seeds"));
		growthCycle.Add(Resources.Load<Sprite>("Sprites/Tiles/Plants/Turnips/Turnip Growth 1"));
		growthCycle.Add(Resources.Load<Sprite>("Sprites/Tiles/Plants/Turnips/Turnip Growth 2"));
		growthCycle.Add(Resources.Load<Sprite>("Sprites/Tiles/Plants/Turnips/Turnip Growth 3"));
		growthCycle.Add(Resources.Load<Sprite>("Sprites/Tiles/Plants/Turnips/Turnip Growth 4"));
		growthCycle.Add(Resources.Load<Sprite>("Sprites/Tiles/Plants/Turnips/Turnip Growth 5"));
        itemID=9;
        growthNum = 0;
        ItemDatabaseObject = GameObject.Find("ItemDatabaseObject");
	}
    void Start() {
        //Turnip item
        obtainableItem = ItemDatabaseObject.GetComponent<ItemDatabase>().items[10];
    }
    public override bool hit(Item item){
    	if (item != null && item.stats.ContainsKey("Pickaxe Power") || item.stats.ContainsKey("Axe Power")) {
			DestroyItem();
            return true;
    	}
        return false;
    }
    public override bool Interact() {
    	if (growthNum >= 5) {
    		if (obtainItem()) {
                return true;
            }

    	}
        return false;
    }
    public override void UpdateDay() {
    	growthNum+=1;
    	if (growthNum >= 5)
    		return; 
    	GetComponent<SpriteRenderer>().sprite = growthCycle[growthNum];
    }
    public override void SetGrowth(int n) {
        growthNum=n;
        GetComponent<SpriteRenderer>().sprite = growthCycle[growthNum];
    }
}
