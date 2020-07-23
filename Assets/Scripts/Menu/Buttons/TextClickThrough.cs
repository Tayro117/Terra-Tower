using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextClickThrough : MonoBehaviour
{
	public GameObject player;


    void Update() {
    	if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) {
    		player.GetComponent<TextController>().NextTextEvent();
    	}
    }


}