using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftItem : MonoBehaviour, IPointerClickHandler
{
    public int count = 0;
    private int maxCount = 100;
    private GameObject Hotbar;
    private GameObject player;

    private Recipe recipe;
    private Item item;
    private Color blue = new Color32(76,185,255,255);


    public void SubtractToMin() {
        if (recipe == null) {
            return;
        }
    	if (count > 0) {
    		count--;
    		UpdateText();
    	}
    }
    public void AddToMax() {
        if (recipe == null) {
            return;
        }
    	var scr = Hotbar.GetComponent<DisplayHotBar>();
    	maxCount = scr.CheckMaxItemCraft(recipe);
    	if (count < item.stats["Max Count"] && maxCount > count) {
    		count++;
    	}
    	UpdateText();

    }
	void Start() {
		Hotbar = GameObject.Find("Canvas/Buttons");
		player = GameObject.Find("Player");
        UpdateText();
	}
    void OnEnable() {
        checkBaseComponents();
    }
    public void checkBaseComponents() {
        if (count == 0 && recipe != null && Hotbar.GetComponent<DisplayHotBar>().CheckItemComponents(recipe, 1)) 
        {
            count = 1;
        }
        UpdateText();
    }
	public void setup(Item i, Recipe r) {
		item = i;
		recipe = r;
	}
    public void OnPointerClick(PointerEventData eventData)
    {
    	if (count < 1)
    		return;
        var HotbarController = Hotbar.GetComponent<DisplayHotBar>();
        var UseItemController = player.GetComponent<UseItem>();
        if (HotbarController.CheckItemComponents(recipe, count) && UseItemController.CheckAddItem(item,0,UseItemController.GetInventorySize())) {
        	HotbarController.RemoveItemComponents(recipe, count);
	        Item newItemStats = new Item(item);
	        newItemStats.stats["Current Count"]=count;
	        UseItemController.AddItem(newItemStats);
	        maxCount = HotbarController.CheckMaxItemCraft(recipe);
	        if (maxCount < count)
	        	count = maxCount;
	        UpdateText();
        }
    }
    public void UpdateText() {
    	transform.GetChild(0).GetComponent<Text>().text = "Craft " + count;
    	if (count < 1) {
    		GetComponent<Image>().color = Color.gray;
    	}
    	else if (GetComponent<Image>().color == Color.gray && count > 0) {
    		GetComponent<Image>().color = blue;
    	}
    }
}
