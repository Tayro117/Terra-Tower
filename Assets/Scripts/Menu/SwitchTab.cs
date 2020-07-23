using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchTab : MonoBehaviour, IPointerClickHandler
{
	public GameObject Background;
    public GameObject Hotbar;


    private int inventorySize;

    void Start() {
        inventorySize=Hotbar.GetComponent<DisplayHotBar>().GetInventorySize();
    }
    // Start is called before the first frame update
    public void OnPointerClick(PointerEventData eventData)
    {
        Switch();
    }
    public void Switch() {
        var HotbarController = Hotbar.GetComponent<DisplayHotBar>();
        string currentActiveTab = HotbarController.getMenuBackground();
        GameObject.Find("Canvas/Menu/"+currentActiveTab).SetActive(false);
        Background.SetActive(true);
        Background.transform.SetAsLastSibling();
        transform.SetAsLastSibling();
        HotbarController.setMenuTab(name, Background.name);
        if (name != "InventoryMenuTab") {
            for (int i = 10;i< inventorySize;i++) {
                Hotbar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else {
            for (int i = 10;i< inventorySize;i++) {
                Hotbar.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
