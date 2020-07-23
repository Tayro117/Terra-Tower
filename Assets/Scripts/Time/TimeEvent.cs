using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeEvent : MonoBehaviour
{
    public bool isOutside=true;
    public GameObject clock;

	private float time=240;
	private int hour = 4;
	private int minute = 0;
	private int day = 0;
	//List of scripts that will be 'subscribed'
	private List<OnUseItem> daySubscribers = new List<OnUseItem>();
    private List<OnUseItem> lightSubscribers = new List<OnUseItem>();

    private Color nightColor = new Color32( 25, 25, 25, 255 );
    private Color dayColor = new Color32( 255, 255, 255, 255 );
    private int currentColor = 1;
    private bool lerping = true;
    private float currentLightPercent = 1f;
    private int lightingTimeChange = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        time+=Time.deltaTime;
        if ((int)(time/10) > minute) {
        	minute=((int)(time/10));
            clock.GetComponent<Text>().text = "" + hour + ":" + (minute%6) + "0";
        	notifyDaySubscribers();
        }
        if ((int)(time/60) > hour) {
        	hour+=1;
            if (hour == 4 || hour == 19) {
                lerping=true;
            }
            minute = 0;
            clock.GetComponent<Text>().text = "" + hour + ":" + (minute%6) + "0";
        }
        if (hour>=24) {
        	time =0;
        	hour = 0;
        	minute = 0;
        	notifyDaySubscribers();
        }
        if (lerping && (int)(time/2)>lightingTimeChange) {
            lightingTimeChange=(int)(time/2);
            if (currentColor == 0)
                currentLightPercent+=0.01f;
            else
                currentLightPercent-=0.01f;
            if (currentLightPercent==1 || currentLightPercent == 0) {
                lerping=false;
                if (currentColor==0)
                    currentColor=1;
                else
                    currentColor=0;
                return;
            }
            RenderSettings.ambientLight = Color.Lerp (dayColor, nightColor, currentLightPercent);
            notifyLights(currentLightPercent);
        }
    }
    private void notifyLights(float currentLightPercent) {
        foreach(OnUseItem scr in lightSubscribers) {
            scr.UpdateLight(currentLightPercent);
        }
    }
    private void notifyDaySubscribers() {
    	foreach (OnUseItem scr in daySubscribers.ToArray()) {
    		scr.UpdateDay();
    	}
    }
    public void SubscribeDay(OnUseItem scr) {
        daySubscribers.Add(scr);
    }
    public void UnsubscribeDay(OnUseItem scr) {
    	daySubscribers.Remove(scr);
    }
    public void SubscribeLight(OnUseItem scr) {
        lightSubscribers.Add(scr);
    }
    public void UnsubscribeLight(OnUseItem scr) {
        lightSubscribers.Remove(scr);
    }
    public float GetPercentage() {
        return currentLightPercent;
    }
    public float GetTime() {
        return time;
    }
    public void UnsubscribeAll() {
        daySubscribers = new List<OnUseItem>();
        lightSubscribers = new List<OnUseItem>();
    }
}
