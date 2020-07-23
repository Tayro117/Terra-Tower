using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

abstract public class OnUseItem : MonoBehaviour, IEventSystemHandler
{
	public GameObject ItemDatabaseObject;
	public Dictionary<int, int> spawnChances;
    public Item obtainableItem;
    public int itemID;
    public int growthNum = -1;
    public int startX;
    public int startY;
    public int length;
    public int width;
    public GameObject audio;

    void Awake() {
        ItemDatabaseObject = GameObject.Find("ItemDatabaseObject");
    }
    abstract public bool hit(Item item);
    public void SetGridPosition(int x, int y, int l, int w) {
        startX=x;
        startY=y;
        length=l;
        width=w;
    }
    virtual public void UpdateDay() {

    }
    virtual public void UpdateLight(float percentage) {

    }
    virtual public bool Interact() {
        return false;
    }
    public bool obtainItem() {
        if (obtainableItem == null) {
            return false;
        }
        var UseItemController = GameObject.Find("Player").GetComponent<UseItem>();
        if (UseItemController.CheckAddItem(obtainableItem, 0, UseItemController.GetInventorySize())) {
            UseItemController.AddItem(new Item(obtainableItem));
            DestroyItem();
        }
        else {
            spawnObtainableItem();
        }
        return true;


    }
    virtual public void SetGrowth(int n) {

    }
    public void spawnItem() {
        System.Random rand = new System.Random();
        foreach (KeyValuePair<int, int> pair in spawnChances) {
            if (rand.Next(1,1000)> pair.Value)
                continue;
            int id = pair.Key;
    		GameObject item = new GameObject();
        	item.AddComponent<SpriteRenderer>();
        	Item stone = new Item(ItemDatabaseObject.GetComponent<ItemDatabase>().items[id]);
        	item.GetComponent<SpriteRenderer>().sprite = stone.icon;
            item.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Sprites/Materials/My diffuse");
        	item.GetComponent<SpriteRenderer>().sortingOrder = -Mathf.FloorToInt(transform.position.y*100/32);
            item.transform.parent = transform.parent;
        	item.transform.localScale = new Vector3(0.25f,0.25f,1);
        	item.transform.position = transform.position;
        	item.gameObject.name = stone.name + " " + 1;
        	item.AddComponent<Rigidbody2D>();
            var r = item.AddComponent<DroppedItemReference>();
            r.SetItem(stone);
        	var temp = item.AddComponent<ItemDropped>();
            Vector2 dir = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(1.0f, 1.5f));
            temp.SetDir(dir);
        }
    }
    public void spawnObtainableItem() {
        System.Random rand = new System.Random();
        int id = obtainableItem.id;
        GameObject item = new GameObject();
        item.AddComponent<SpriteRenderer>();
        Item stone = new Item(ItemDatabaseObject.GetComponent<ItemDatabase>().items[id]);
        item.GetComponent<SpriteRenderer>().sprite = stone.icon;
        item.transform.parent = transform.parent;
        item.transform.localScale = new Vector3(0.25f,0.25f,1);
        item.transform.position = transform.position;
        item.gameObject.name = stone.name + " " + 1;
        item.AddComponent<Rigidbody2D>();
        var r = item.AddComponent<DroppedItemReference>();
        r.SetItem(stone);
        var temp = item.AddComponent<ItemDropped>();
        Vector2 dir = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(1.0f, 1.5f));
        temp.SetDir(dir);
    }
    public void DestroyItem() {
        Destroy(gameObject);
    }
    public IEnumerator playAudio()
    {
        if (audio != null) {
            GameObject a = Instantiate(audio, transform.position, Quaternion.identity);
            while(a.GetComponent<AudioSource>().isPlaying) {
                yield return null;
            }
            Destroy(a);
        }
    }
}
