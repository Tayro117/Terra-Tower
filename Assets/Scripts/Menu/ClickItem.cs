using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickItem : MonoBehaviour, IPointerDownHandler//, IPointerEnterHandler, IPointerExitHandler
{
	public GameObject player;
    public GameObject Hotbar;

	private int id;
    // private bool isListening = false;
    // private bool isHovering = false;
    // Start is called before the first frame update
    void Start()
    {
        id = int.Parse(name.Substring(name.IndexOf('_')+1));
    }
    // void Update() {
    //     if (isListening && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))) {
    //         Hotbar.GetComponent<DisplayHotBar>().setHeldItem(id);
    //         isListening=false;
    //     }
    // }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (player.GetComponent<Movement>().menuActive) {
            if (Input.GetKey(KeyCode.LeftShift) && player.GetComponent<Movement>().GetChestMenuActive()) {
                if (id < 10) {
                    Hotbar.GetComponent<DisplayHotBar>().ShiftDown(id, 50, 80);
                }
                else {
                    Hotbar.GetComponent<DisplayHotBar>().ShiftDown(id, 0, 10);
                }
            }
            else if (Input.GetKey(KeyCode.LeftShift)) {
                if (id < 10) {
                    Hotbar.GetComponent<DisplayHotBar>().ShiftDown(id, 10, player.GetComponent<UseItem>().GetInventorySize());
                }
                else {
                    Hotbar.GetComponent<DisplayHotBar>().ShiftDown(id, 0, 10);
                }
            }
            else if (eventData.button == 0) {
                Hotbar.GetComponent<DisplayHotBar>().setHeldItem(id);
                //StartCoroutine(DelayPickup());
            }
            else if (eventData.button == PointerEventData.InputButton.Right) {
                Hotbar.GetComponent<DisplayHotBar>().moveOneItem(id);
                //StartCoroutine(DelayPickup());
            }
        }
        else if (id > -1 && id < 10) {
            Hotbar.GetComponent<DisplayHotBar>().switchSelector(id);
        }
    }
    // public void OnPointerExit(PointerEventData eventData) {
    //     isHovering = false;
    //     isListening=false;
    // }
    // public void OnPointerEnter(PointerEventData eventData) {
    //     isHovering=true;
    //     isListening=true;
    // }
    // public IEnumerator DelayPickup() {
    //     isListening=false;
    //     yield return new WaitForSeconds(0.25f);
    //     if (isHovering)
    //         isListening=true;
    // }
}
