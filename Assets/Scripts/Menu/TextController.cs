using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
	public GameObject textController;
	

    private List<string> dialogue;
    private float timer = 0;
    private float timeBetweenChar = 0.05f;
    private bool isDisplaying = true;
    private int currentChar = 0;
    private string currentString = "";

    // Start is called before the first frame update
    void Start()
    {  
    	//textController.SetActive(false);
        dialogue = new List<string>();
        dialogue.Add("Hello and welcome to Taylor's game :)");
        dialogue.Add("A few simple rules to get started");
        dialogue.Add("Pls try to break everything and tell me how bad I am at coding");
        dialogue.Add("Also let me know if any controls/sprites/etc feel wonky");
        dialogue.Add("And have FUN!!!");

    }

    void Update() {
        if (isDisplaying) {
            timer-=Time.deltaTime;
            if (timer <= 0f) {
                timer = timeBetweenChar;
                currentString+=dialogue[0][currentChar];
                currentChar++;
                textController.transform.GetChild(0).gameObject.GetComponent<Text>().text = currentString;
                if (currentChar >= dialogue[0].Length){
                    isDisplaying=false;
                    return;
                }
            }
        }

    }
    public void TriggerTextEvent(List<string> text) {
    	if (text.Count < 1)
    		return;
        textController.transform.GetChild(0).gameObject.GetComponent<Text>().text="";
    	dialogue=text;
    	GetComponent<Movement>().SetCanMove(false);
        GetComponent<Movement>().StopMovement();
    	textController.SetActive(true);
        isDisplaying=true;
    }
    public void NextTextEvent() {
        currentString="";
        currentChar=0;
        if (isDisplaying) {
            textController.transform.GetChild(0).gameObject.GetComponent<Text>().text = dialogue[0];
            isDisplaying=false;
            return;
        }
        dialogue.RemoveAt(0);
    	if (dialogue.Count > 0) {
            isDisplaying=true;
    		return;
    	}
    	else {
    		StartCoroutine(EndTextEvent());
    	}
    }
    public IEnumerator EndTextEvent() {
        isDisplaying=false;
    	textController.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        GetComponent<Movement>().SetCanMove(true);
    }
}
