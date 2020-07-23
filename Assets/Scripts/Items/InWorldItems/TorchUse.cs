using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchUse : OnUseItem
{
    private float intensity;
    private TimeEvent TimeController;
    private Light _light;
    private float currentIntensity;
    private float flickerTime =1;
    private bool isFlickering=true;
	void Start() {
		spawnChances = new Dictionary<int, int>();
        spawnChances.Add(11, 1000);
        obtainableItem = ItemDatabaseObject.GetComponent<ItemDatabase>().items[11];
        intensity = transform.GetChild(0).GetComponent<Light>().intensity;
        TimeController = GameObject.Find("TimeManager").GetComponent<TimeEvent>();
        _light=transform.GetChild(0).gameObject.GetComponent<Light>();
        if (TimeController.isOutside) {
            currentIntensity = intensity*TimeController.GetPercentage();
            _light.intensity = currentIntensity;
            TimeController.SubscribeLight(GetComponent<TorchUse>());
        }
        itemID=11;

	}
    void Update() {
        if (isFlickering && TimeController.GetTime()/0.1 > flickerTime) {
            flickerTime +=1;
            float flicker = Random.Range(0.95f,1.05f);
            _light.intensity=currentIntensity*flicker;
        }
    }
    public override bool hit(Item item){
        if (item != null && item.stats.ContainsKey("Pickaxe Power") || item.stats.ContainsKey("Axe Power")) {
            spawnItem();
            if (TimeController.isOutside)
                TimeController.UnsubscribeLight(GetComponent<TorchUse>());
            return true;
        }
        return false;
    }
    public override bool Interact() {
        if (obtainItem()) {

            if (TimeController.isOutside) {
                TimeController.UnsubscribeLight(GetComponent<TorchUse>());
            }
            return true;
        }
        return false;
    }
    public override void UpdateLight(float percentage) {
        currentIntensity = intensity*percentage;
        transform.GetChild(0).GetComponent<Light>().intensity = currentIntensity;

    }
}