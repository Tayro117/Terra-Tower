using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeCraftCount : MonoBehaviour, IPointerClickHandler
{
	public GameObject CraftButton;

	/*private bool isPressed;
	private float waitTime = 0.15f;
	private float timer = 0;

	void Update() {
		if (isPressed) {
			var scr = CraftButton.GetComponent<CraftItem>();
			timer+=Time.deltaTime;
			if (timer>=waitTime) {
				timer-=waitTime;
				if (name=="Plus") {
					scr.AddToMax();
				}
				else if (scr.count > 1) {
					scr.SubtractToMin();
				}
			}
		}
	}
*/
    public void OnPointerClick(PointerEventData eventData)
    {
    	var scr = CraftButton.GetComponent<CraftItem>();
    	if (name == "Minus" && scr.count > 0) {
    		scr.SubtractToMin();
    	}
    	else if (name=="Plus") {
    		scr.AddToMax();
    	}
    }
	// public void onPointerDownRaceButton()
	// {
	// 	isPressed = true;
	// }
	// public void onPointerUpRaceButton()
	// {
	// 	isPressed = false;
	// 	timer = 0;
	// }
}
