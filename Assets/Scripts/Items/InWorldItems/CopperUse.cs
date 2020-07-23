using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopperUse : OnUseItem
{
    private int health = 30;
    public CopperUse() {
        spawnChances = new Dictionary<int, int>();
        spawnChances.Add(17, 1000);
        itemID=17;
    }
    void Start() {
    }
    public override bool hit(Item item){
        if (item != null && item.stats.ContainsKey("Pickaxe Power")) {
            StartCoroutine(playAudio());
            health-=item.stats["Pickaxe Power"];
            if (health <= 0) {
                int randomAmount = Random.Range(1, 6);
                for (int i = 0;i<randomAmount;i++) {
                    spawnItem();
                }
                return true;
            }
        }
        return false;
    }
}
