using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public Dictionary<int, Item> inventoryItems = new Dictionary<int, Item>();
    public GameObject ItemDatabaseObject;
    public GameObject hotbar;
    public GameObject gridObject;
    
    private GridSystem grid;
    private GridSystem tiles;
    private int inventorySize = 50;
    private int currentItem = 0;
    private List<Item> listOfItems;
    private Dictionary<int, GameObject> hotbarGameObjects;
    private int tileSize = 0;

    void Start()
    {
        grid = gridObject.GetComponent<Room>().startingRoom;
        tiles = gridObject.GetComponent<Room>().floorGrid;
        tileSize=grid.getTileSize();
        hotbarGameObjects = hotbar.GetComponent<DisplayHotBar>().GetHotBar();
    	listOfItems = ItemDatabaseObject.GetComponent<ItemDatabase>().items;

        hotbar.GetComponent<DisplayHotBar>().switchSelector(0);
        AddDefaultItems();
        // AddItem(new Item(listOfItems[12]));
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<Movement>().GetCanMove()) {
            return;
        }
        if (!player.GetComponent<Movement>().menuActive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                hotbar.GetComponent<DisplayHotBar>().switchSelector(9);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                hotbar.GetComponent<DisplayHotBar>().switchSelector(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                hotbar.GetComponent<DisplayHotBar>().switchSelector(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                hotbar.GetComponent<DisplayHotBar>().switchSelector(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                hotbar.GetComponent<DisplayHotBar>().switchSelector(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                hotbar.GetComponent<DisplayHotBar>().switchSelector(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6)) {
                hotbar.GetComponent<DisplayHotBar>().switchSelector(5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7)) {
                hotbar.GetComponent<DisplayHotBar>().switchSelector(6);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8)) {
                hotbar.GetComponent<DisplayHotBar>().switchSelector(7);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9)) {
                hotbar.GetComponent<DisplayHotBar>().switchSelector(8);
            }
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0f)
            {
                currentItem--;
                if (currentItem < 0) {currentItem=9;}
                hotbar.GetComponent<DisplayHotBar>().switchSelector(currentItem);
            }
            else if (scroll < 0f)
            {
                currentItem++;
                if (currentItem > 9) {currentItem=0;}
                hotbar.GetComponent<DisplayHotBar>().switchSelector(currentItem);
            }
        	else if (Input.GetMouseButtonDown(0) && inventoryItems.ContainsKey(currentItem) && !hotbar.GetComponent<DisplayHotBar>().IsHeldItem()){
				Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int gridX = Mathf.FloorToInt(clickPos.x*100/32f);
                int gridY = Mathf.FloorToInt(clickPos.y*100/32f);
                Vector2 boxColliderPos = transform.TransformPoint(GetComponent<BoxCollider2D>().offset);
                int playerX = Mathf.FloorToInt(boxColliderPos.x*100/32f);
                int playerY = Mathf.FloorToInt(boxColliderPos.y*100/32f);

                if (Mathf.Abs(gridX-playerX) < 2 && Mathf.Abs(gridY-playerY) < 2) {
                    if (inventoryItems[currentItem].stats.ContainsKey("Dynamic Type")) {
                        Vector3 loc = new Vector3(gridX*tileSize/100f+tileSize/200f, gridY*tileSize/100f+tileSize/200f, 0);
                        hotbar.GetComponent<DisplayHotBar>().PlaceDynamicType(gridX,gridY,loc,inventoryItems[currentItem],hotbarGameObjects[currentItem], currentItem);
                    }
                    else if (inventoryItems[currentItem].stats.ContainsKey("Hoe Power") || inventoryItems[currentItem].stats.ContainsKey("Watering Power")) {
                        GameObject hit = tiles.getItem(gridX,gridY);
                        if (hit.GetComponent(typeof(OnUseItem)) != null){
                            ((OnUseItem)hit.GetComponent(typeof(OnUseItem))).hit(inventoryItems[currentItem]);
                        }
                    }
                    else if (grid.checkGrid(gridX,gridY, 1, 1)) {
                        GameObject hit = grid.getItem(gridX,gridY);
                        if (hit.GetComponent(typeof(OnUseItem)) != null){
                            if (((OnUseItem)hit.GetComponent(typeof(OnUseItem))).hit(inventoryItems[currentItem])) {
                                var scr = (OnUseItem)hit.GetComponent(typeof(OnUseItem));
                                grid.removeItem(scr.startX,scr.startY, scr.length,scr.width);   

                                Destroy(hit);
                            }
                        }
                    }
                }
                else {
                    gridX = playerX;
                    gridY = playerY;
                    if (Mathf.Abs(clickPos.x-transform.position.x) < Mathf.Abs(clickPos.y-transform.position.y)) {
                        if (clickPos.y-transform.position.y < 0)
                            gridY -=1;
                        else
                            gridY +=1;
                    }
                    else {
                        if (clickPos.x-transform.position.x < 0)
                            gridX -=1;
                        else
                            gridX +=1;
                    }
                    if (inventoryItems[currentItem].stats.ContainsKey("Hoe Power") || inventoryItems[currentItem].stats.ContainsKey("Watering Power")) {
                        GameObject hit = tiles.getItem(gridX,gridY);
                        if (hit.GetComponent(typeof(OnUseItem)) != null){
                            ((OnUseItem)hit.GetComponent(typeof(OnUseItem))).hit(inventoryItems[currentItem]);
                        }
                    }
                    else if (grid.checkGrid(gridX,gridY, 1, 1)) {
                        GameObject hit = grid.getItem(gridX,gridY);
                        if (hit.GetComponent(typeof(OnUseItem)) != null){
                            if (((OnUseItem)hit.GetComponent(typeof(OnUseItem))).hit(inventoryItems[currentItem])) {
                                var scr = (OnUseItem)hit.GetComponent(typeof(OnUseItem));
                                grid.removeItem(scr.startX,scr.startY, scr.length,scr.width);   

                                Destroy(hit);
                            }
                        }
                    }
                }
    		}
            else if (Input.GetMouseButtonDown(1)) {
                Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int gridX = Mathf.FloorToInt(clickPos.x*100/32f);
                int gridY = Mathf.FloorToInt(clickPos.y*100/32f);
                Vector2 boxColliderPos = transform.TransformPoint(GetComponent<BoxCollider2D>().offset);
                int playerX = Mathf.FloorToInt(boxColliderPos.x*100/32f);
                int playerY = Mathf.FloorToInt(boxColliderPos.y*100/32f);
                //TODO add in logic to search around you to pull from
                if (Mathf.Abs(gridX-playerX) <= 1 && Mathf.Abs(gridY-playerY) <= 1) {
                    GameObject use = grid.getItem(gridX,gridY);
                    if (use && use.GetComponent(typeof(OnUseItem)) != null){
                        if (((OnUseItem)use.GetComponent(typeof(OnUseItem))).Interact()) {
                            var scr = (OnUseItem)use.GetComponent(typeof(OnUseItem));
                            grid.removeItem(scr.startX,scr.startY, scr.length,scr.width);
                        }
                    }
                }
            }
        }
    }
    public void AddDefaultItems() {
        AddItem(new Item(listOfItems[1]));
        AddItem(new Item(listOfItems[5]));
        AddItem(new Item(listOfItems[6]));
        AddItem(new Item(listOfItems[7]));
        AddItem(new Item(listOfItems[8]));
        AddItem(new Item(listOfItems[8]));
        AddItem(new Item(listOfItems[8]));
        AddItem(new Item(listOfItems[8]));
        AddItem(new Item(listOfItems[8]));
        AddItem(new Item(listOfItems[8]));
        AddItem(new Item(listOfItems[8]));
        AddItem(new Item(listOfItems[8]));
        AddItem(new Item(listOfItems[11]));
        AddItem(new Item(listOfItems[11]));
        AddItem(new Item(listOfItems[11]));
        AddItem(new Item(listOfItems[14]));
    }

    public int getCurrentItem() {
        return currentItem;
    }
    public void setCurrentItem(int num) {
        currentItem = num;
    }
    public bool CheckAddItem(Item item, int start, int end) {

        for (int i = start; i<end;i++) {
            if (!inventoryItems.ContainsKey(i)) {
                return true;
            }
            else if (inventoryItems[i].name==item.name && inventoryItems[i].stats["Max Count"] > inventoryItems[i].stats["Current Count"]) {
                return true;
            }
        }
        return false;
    }
    public bool AddItem(Item item) {
        int firstEmptySpace = -1;
        List<int> objs = new List<int>();

        for (int i = 0; i<inventorySize;i++) {
            if (firstEmptySpace == -1 && !inventoryItems.ContainsKey(i)) {
                firstEmptySpace = i;
            }
            else if (!inventoryItems.ContainsKey(i)) {
                continue;
            }
            else if (inventoryItems[i].name==item.name) {
                objs.Add(i);
            }
        }
        foreach (int i in objs) {
            if (inventoryItems[i].stats["Max Count"] > inventoryItems[i].stats["Current Count"]) {
                if (inventoryItems[i].stats["Max Count"] < inventoryItems[i].stats["Current Count"]+item.stats["Current Count"]) {
                    item.stats["Current Count"]-=(inventoryItems[i].stats["Max Count"]-inventoryItems[i].stats["Current Count"]);
                    inventoryItems[i].stats["Current Count"] = inventoryItems[i].stats["Max Count"];
                    hotbar.GetComponent<DisplayHotBar>().UpdateInventoryText(i,inventoryItems[i].stats["Current Count"]);
                }
                else {
                    inventoryItems[i].stats["Current Count"] += item.stats["Current Count"];
                    item.stats["Current Count"]=0;
                    hotbar.GetComponent<DisplayHotBar>().UpdateInventoryText(i,inventoryItems[i].stats["Current Count"]);
                    return true;
                }
            }
        }

        if (firstEmptySpace == -1) {
            return false;
        }

        inventoryItems.Add(firstEmptySpace,item);
        hotbar.GetComponent<DisplayHotBar>().AddToHotbar(firstEmptySpace,item);
        return true;
        
    }
    public void AddItemIndex(int id, int loc, int count) {
        Item item = new Item(listOfItems[id]);
        item.stats["Current Count"]=count;
        inventoryItems.Add(loc,item);
        hotbar.GetComponent<DisplayHotBar>().AddToHotbar(loc,item);
    }
    public void switchInventory(int loc, Item item) {
        removeInventory(loc);
        addInventory(loc,item);
    }
    public void addInventory(int loc, Item item) {
        inventoryItems.Add(loc,item);
    }
    public void removeInventory(int loc) {
        inventoryItems.Remove(loc);
    }
    public int GetInventorySize() {
        return inventorySize;
    }
}
