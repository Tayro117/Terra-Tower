using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronUse : OnUseItem
{
    private int health = 50;
    public IronUse() {
        spawnChances = new Dictionary<int, int>();
        spawnChances.Add(18, 1000);
        itemID=18;
    }
    void Start() {
    }
    public override bool hit(Item item){
        if (item != null && item.stats.ContainsKey("Pickaxe Power")) {
            StartCoroutine(playAudio());
            health-=item.stats["Pickaxe Power"];
            if (health <= 0) {
                int randomAmount = Random.Range(1, 5);
                for (int i = 0;i<randomAmount;i++) {
                    spawnItem();
                }
                return true;
            }
        }
        return false;
    }
}
