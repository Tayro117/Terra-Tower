using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : OnUseItem
{
    //THIS IS FOR EMPTY DIRT THINGS
	private bool tilled = false;
	private bool watered = false;
	private Sprite tilledSprite;
	private Sprite tilledWateredSprite;
	private Sprite base1;
    private GameObject TimeEventObject;
    private GridSystem interactables;
    private int gridX;
    private int gridY;
    private System.Random rand = new System.Random();

	void Awake() {
		tilledSprite = Resources.Load<Sprite>("Sprites/Tiles/TilledDirt");
		tilledWateredSprite = Resources.Load<Sprite>("Sprites/Tiles/TilledWateredDirt");
		base1 = Resources.Load<Sprite>("Sprites/Tiles/Transparent");
        TimeEventObject = GameObject.Find("TimeManager");
        interactables = GameObject.Find("RoomManager").GetComponent<Room>().startingRoom;
        gridX = Mathf.FloorToInt(transform.position.x*100/32f);
        gridY = Mathf.FloorToInt(transform.position.y*100/32f);
        ItemDatabaseObject = GameObject.Find("ItemDatabaseObject");
	}


    public override bool hit(Item item){
    	if (item == null || (interactables.checkGrid(gridX,gridY, 1, 1)) && !item.stats.ContainsKey("Watering Power")) {
			growthNum=-1;
            return false;
		}
    	if (item.stats.ContainsKey("Hoe Power") && !tilled) {
    		GetComponent<SpriteRenderer>().sprite = tilledSprite;
            growthNum=1;
            tilled = true;
            var TimeController = TimeEventObject.GetComponent<TimeEvent>();
            TimeController.SubscribeDay(GetComponent<Dirt>());
    		return true;
    	}
    	else if (item.stats.ContainsKey("Watering Power") && !watered && tilled) {
    		GetComponent<SpriteRenderer>().sprite = tilledWateredSprite;
            growthNum=2;
            watered=true;
    		return true;
    	}
        growthNum=-1;
    	return false;
    }

    public bool isPlantable() {
        if (tilled && !interactables.checkGrid(gridX,gridY, 1, 1))
            return true;
        return false;
    }
    public override void UpdateDay() {
        if (watered && tilled) {
            if (Random.Range(0f, 1f) >= 0.95) {
                GetComponent<SpriteRenderer>().sprite = base1;
                tilled =false;
                watered=false;
                growthNum=-1;
                var TimeController = TimeEventObject.GetComponent<TimeEvent>();
                TimeController.UnsubscribeDay(GetComponent<Dirt>());
                DestroyPlant();
            }
            else {
                GetComponent<SpriteRenderer>().sprite = tilledSprite;
                watered=false;
                growthNum=1;
                //DO plant watering :)
                if (interactables.checkGrid(gridX,gridY, 1, 1)) {
                    var obj = interactables.getItem(gridX,gridY).GetComponent<OnUseItem>();
                    if (obj) {
                        obj.UpdateDay();
                    }
                }
            }
        }
        else if (tilled) {
            if (Random.Range(0f, 1f) >= 0.8) {
                tilled=false;
                growthNum=-1;
                GetComponent<SpriteRenderer>().sprite = base1;
                var TimeController = TimeEventObject.GetComponent<TimeEvent>();
                TimeController.UnsubscribeDay(GetComponent<Dirt>());
                DestroyPlant();
            }
        }
    }
    public override void SetGrowth(int n) {
        growthNum=n;
        if (n < 1)
            return;
        else if (n == 1) {
            GetComponent<SpriteRenderer>().sprite = tilledSprite;
            tilled = true;
            var TimeController = TimeEventObject.GetComponent<TimeEvent>();
            TimeController.SubscribeDay(GetComponent<Dirt>());
        }
        else if (n == 2) {
            GetComponent<SpriteRenderer>().sprite = tilledWateredSprite;
            growthNum=2;
            watered=true;
            tilled=true;
            var TimeController = TimeEventObject.GetComponent<TimeEvent>();
            TimeController.SubscribeDay(GetComponent<Dirt>());
        }
    }
    public void DestroyPlant() {
        if (interactables.checkGrid(gridX,gridY, 1, 1)) {
                var obj = interactables.getItem(gridX,gridY);
                if (obj) {
                    Destroy(obj);
                    interactables.removeItem(gridX,gridY, 1, 1);
                }
            }
    }
}