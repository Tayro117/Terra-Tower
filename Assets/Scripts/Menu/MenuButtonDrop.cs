using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonDrop : MonoBehaviour, IPointerClickHandler
{
    public GameObject Hotbar;


    public void OnPointerClick(PointerEventData eventData)
    {
    	Hotbar.GetComponent<DisplayHotBar>().DropHeldItem();
    }
}
