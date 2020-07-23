using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenPage : MonoBehaviour, IPointerClickHandler
{
	private GameObject InfoPanel;
	private GameObject ComponentPrefab;
	private List<Item> itemList;
	private Item item;
	private Recipe recipe;


	void Start() {
		InfoPanel = GameObject.Find("Canvas/Menu/CraftingBackground/InfoPanel");
	}

	public void setup(Item i, Recipe r, GameObject c, List<Item> items) {
		item = i;
		recipe = r;
		ComponentPrefab = c;
		itemList = items;
	}
    public void OnPointerClick(PointerEventData eventData)
    {
        removeChildren();
        InfoPanel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = item.icon;
        InfoPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = item.name;
        InfoPanel.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = item.description;
        var CraftController = InfoPanel.transform.GetChild(5).gameObject.GetComponent<CraftItem>();
        CraftController.setup(item, recipe);

		foreach (KeyValuePair<int, int> pair in recipe.components)
        {
        	Item item = itemList[pair.Key];
        	GameObject component = Instantiate(ComponentPrefab, Vector3.zero, Quaternion.identity);
        	component.transform.SetParent(InfoPanel.transform.GetChild(4), false);
        	component.name = item.name + " Component";
        	component.transform.GetChild(0).gameObject.GetComponent<Image>().sprite=item.icon;
        	component.transform.GetChild(1).gameObject.GetComponent<Text>().text = "" + pair.Value;
        }
        CraftController.count = 0;
        CraftController.checkBaseComponents();

    }
    private void removeChildren() {

    	foreach (Transform t in InfoPanel.transform.GetChild(4)) {
    		Destroy(t.gameObject);
    	}
    }
}
