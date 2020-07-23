using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHotBar : MonoBehaviour
{
	public GameObject player;
    public GameObject prefab;
	public GameObject selector;
    public GameObject cursor;
    public GameObject ItemDatabaseObject;

    private GameObject PlaceableItemSquare;
    private GameObject room;
    private string menuTab = "InventoryMenuTab";
    private string menuBackground = "InventoryBackground";
	private Dictionary<int,Item> displayItems;
	private Dictionary<int,GameObject> hotbar = new Dictionary<int,GameObject>();
	private int currentItem = 0;
    private GameObject heldItem = null;
    private Item heldItemStats = null;
    private Vector3 lastPos = Vector3.zero;
    private int prevKey;
    private GridSystem grid;
    private int tileSize;
    private Color GreenHighlight = new Color32(0,255,46,68);
    private Color RedHighlight = new Color32(255, 0, 46, 68);
    private Color missing = new Color32(0,0,0,0);
    private GridSystem tiles;
    // Start is called before the first frame update
    void Start()
    {
        displayItems = player.GetComponent<UseItem>().inventoryItems;
        room = GameObject.Find("RoomManager");
        grid = room.GetComponent<Room>().startingRoom;
        PlaceableItemSquare = GameObject.Find("PlaceableItemSquare");
        PlaceableItemSquare.SetActive(false);
        tileSize=grid.getTileSize();
        tiles = room.GetComponent<Room>().floorGrid;
    }
    void LateUpdate()
    {

        if (!player.GetComponent<Movement>().GetCanMove()) {
            return;
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Set fake mouse Cursor
        //customCursor.transform.position = mousePos;
        currentItem = player.GetComponent<UseItem>().getCurrentItem();
        cursor.transform.position = new Vector3(mousePos.x,mousePos.y,cursor.transform.position.z);
        if (heldItem != null && !player.GetComponent<Movement>().menuActive && heldItemStats.stats.ContainsKey("Placeable Size")) {
            int gridX = Mathf.FloorToInt(mousePos.x*100/32f);
            int gridY = Mathf.FloorToInt(mousePos.y*100/32f);
            Vector2 boxColliderPos = player.transform.TransformPoint(player.GetComponent<BoxCollider2D>().offset);
            int playerX = Mathf.FloorToInt(boxColliderPos.x*100/32f);
            int playerY = Mathf.FloorToInt(boxColliderPos.y*100/32f);

            Vector3 loc = new Vector3(gridX*tileSize/100f+tileSize/200f, gridY*tileSize/100f+tileSize/200f, 0);
            PlaceableItemSquare.transform.position = loc;
            bool IsOpen = grid.checkGrid(gridX,gridY,heldItemStats.stats["Placeable Length"], heldItemStats.stats["Placeable Width"]);
            if (heldItemStats.stats.ContainsKey("Dynamic Type")) {
                if (tiles.getItem(gridX,gridY).GetComponent<Dirt>().isPlantable())
                    IsOpen=false;
                else
                    IsOpen=true;
            }

            int Xdiff = Mathf.Abs(gridX-playerX);
            int Ydiff = Mathf.Abs(gridY-playerY);

            if (!IsOpen && Xdiff < 3 && Ydiff < 3 && Xdiff + Ydiff >=1) {
                PlaceableItemSquare.GetComponent<SpriteRenderer>().color = GreenHighlight;
            }
            else {
                PlaceableItemSquare.GetComponent<SpriteRenderer>().color = RedHighlight;
            }

            if (Input.GetMouseButtonDown(0) && !IsOpen && Xdiff < 3 && Ydiff < 3 && Xdiff + Ydiff >=1) {
                //Dynamic type is only Seeds for now, but might extend to other types

                if (heldItemStats.stats.ContainsKey("Dynamic Type")) {
                    PlaceDynamicType(gridX,gridY,loc, heldItemStats, heldItem, -1);
                }
                else {
                    float dx = (float)(heldItemStats.stats["Sprite Tile Width"]-1)/2;
                    float dy = (float)(heldItemStats.stats["Sprite Tile Length"]-1)/2;
                    Vector3 spawnLoc = new Vector3((gridX+dx)*tileSize/100f+tileSize/200f, (dy+gridY)*tileSize/100f+tileSize/200f, 0);
                    GameObject placedItem = Instantiate(Resources.Load<GameObject>("Prefabs/In World Prefabs/"+heldItemStats.name),spawnLoc,Quaternion.identity);
                    grid.addToGrid(gridX,gridY,placedItem,heldItemStats.stats["Placeable Length"], heldItemStats.stats["Placeable Width"]);
                    var scr = ((OnUseItem)placedItem.GetComponent(typeof(OnUseItem)));
                    scr.SetGridPosition(gridX,gridY,heldItemStats.stats["Placeable Length"], heldItemStats.stats["Placeable Width"]);
                    placedItem.GetComponent<SpriteRenderer>().sortingOrder=-gridY;
                    placedItem.transform.SetParent(room.GetComponent<Room>().GetSceneParent().transform);
                    heldItemStats.stats["Current Count"]-=1;
                    UpdateHeldItemText(heldItem, heldItemStats.stats["Current Count"]);
                    if (heldItemStats.stats["Current Count"] < 1) {
                        Destroy(heldItem);
                        heldItem=null;
                        heldItemStats = null;
                        PlaceableItemSquare.SetActive(false);
                    }
                }
                
            }
        }
        else if (displayItems.ContainsKey(currentItem) && displayItems[currentItem].stats.ContainsKey("Placeable Size") && !player.GetComponent<Movement>().menuActive) {
            int gridX = Mathf.FloorToInt(mousePos.x*100/32f);
            int gridY = Mathf.FloorToInt(mousePos.y*100/32f);
            Vector2 boxColliderPos = player.transform.TransformPoint(player.GetComponent<BoxCollider2D>().offset);
            int playerX = Mathf.FloorToInt(boxColliderPos.x*100/32f);
            int playerY = Mathf.FloorToInt(boxColliderPos.y*100/32f);
            PlaceableItemSquare.SetActive(true);
            Vector3 loc = new Vector3(gridX*tileSize/100f+tileSize/200f, gridY*tileSize/100f+tileSize/200f, 0);
            PlaceableItemSquare.transform.position = loc;
            bool IsOpen = grid.checkGrid(gridX,gridY,displayItems[currentItem].stats["Placeable Length"],displayItems[currentItem].stats["Placeable Width"]);
            if (displayItems[currentItem].stats.ContainsKey("Dynamic Type")) {
                if (tiles.getItem(gridX,gridY).GetComponent<Dirt>().isPlantable())
                    IsOpen=false;
                else
                    IsOpen=true;
            }

            int Xdiff = Mathf.Abs(gridX-playerX);
            int Ydiff = Mathf.Abs(gridY-playerY);

            if (!IsOpen && Xdiff < 3 && Ydiff < 3 && Xdiff + Ydiff >=1) {
                PlaceableItemSquare.GetComponent<SpriteRenderer>().color = GreenHighlight;
            }
            else {
                PlaceableItemSquare.GetComponent<SpriteRenderer>().color = RedHighlight;
            }

            if (Input.GetMouseButtonDown(0) && !IsOpen && Xdiff < 3 && Ydiff < 3 && Xdiff + Ydiff >=1) {
                //Dynamic type is only Seeds for now, but might extend to other types

                if (displayItems[currentItem].stats.ContainsKey("Dynamic Type")) {
                    PlaceDynamicType(gridX,gridY,loc, displayItems[currentItem], hotbar[currentItem], -1);
                }
                else {
                    float dx = (float)(displayItems[currentItem].stats["Sprite Tile Width"]-1)/2;
                    float dy = (float)(displayItems[currentItem].stats["Sprite Tile Height"]-1)/2;
                    Vector3 spawnLoc = new Vector3((gridX+dx)*tileSize/100f+tileSize/200f, (dy+gridY)*tileSize/100f+tileSize/200f, 0);
                    GameObject placedItem = Instantiate(Resources.Load<GameObject>("Prefabs/In World Prefabs/"+displayItems[currentItem].name),spawnLoc,Quaternion.identity);
                    grid.addToGrid(gridX,gridY,placedItem,displayItems[currentItem].stats["Placeable Length"], displayItems[currentItem].stats["Placeable Width"]);
                    var scr = ((OnUseItem)placedItem.GetComponent(typeof(OnUseItem)));
                    scr.SetGridPosition(gridX,gridY,displayItems[currentItem].stats["Placeable Length"], displayItems[currentItem].stats["Placeable Width"]);
                    placedItem.GetComponent<SpriteRenderer>().sortingOrder=-gridY;

                    placedItem.transform.SetParent(room.GetComponent<Room>().GetSceneParent().transform);
                    displayItems[currentItem].stats["Current Count"]-=1;
                    UpdateInventoryText(currentItem, displayItems[currentItem].stats["Current Count"]);
                    if (displayItems[currentItem].stats["Current Count"] < 1) {
                        RemoveFromHotbar(currentItem);
                        displayItems[currentItem] = null;
                        displayItems.Remove(currentItem);
                        PlaceableItemSquare.SetActive(false);
                    }
                }
                
            }
        }
        else if (displayItems.ContainsKey(currentItem) && displayItems[currentItem].stats.ContainsKey("Consumable Type") && !player.GetComponent<Movement>().menuActive) {
            if (Input.GetMouseButtonDown(0)) {
                ConsumeItem(displayItems[currentItem], currentItem);
            }
        }
        if (heldItem == null && (!displayItems.ContainsKey(currentItem) || (displayItems.ContainsKey(currentItem) && !displayItems[currentItem].stats.ContainsKey("Placeable Size")))) {
            PlaceableItemSquare.SetActive(false);
        }
    }
    public Dictionary<int, GameObject> GetHotBar() {
        return hotbar;
    }
    public void PlaceDynamicType(int gridX, int gridY, Vector3 loc, Item item, GameObject itemObject, int inventoryLocation) {
        //Type refers to the item id it plants/places into the world
        int type = item.stats["Dynamic Type"];
        if (type >= 9 && type <= 40 && tiles.getItem(gridX,gridY).GetComponent<Dirt>().isPlantable()){
            string name = ItemDatabaseObject.GetComponent<ItemDatabase>().items[type].name;
            GameObject placedItem = Instantiate(Resources.Load<GameObject>("Prefabs/In World Prefabs/"+name),loc,Quaternion.identity);
            grid.addToGrid(gridX,gridY,placedItem, 1, 1);
            var scr = ((OnUseItem)placedItem.GetComponent(typeof(OnUseItem)));
            scr.SetGridPosition(gridX,gridY,1, 1);
            placedItem.GetComponent<SpriteRenderer>().sortingOrder=-gridY;


            item.stats["Current Count"]-=1;
            UpdateHeldItemText(itemObject, item.stats["Current Count"]);
            if (item.stats["Current Count"] < 1) {
                Destroy(itemObject);
                itemObject=null;
                item = null;
                PlaceableItemSquare.SetActive(false);
                if (displayItems.ContainsKey(inventoryLocation)) {
                    displayItems.Remove(inventoryLocation);
                    hotbar.Remove(inventoryLocation);
                }
            }
        }
    }
    public bool IsHeldItem() {
        return heldItem!=null;
    }
    public void setMenuTab(string name, string background) {
        menuTab = name;
        menuBackground = background;
    }
    public string getMenuTab() {
        return menuTab;
    }
    public string getMenuBackground() {
        return menuBackground;
    }

    public int GetInventorySize() {
        return player.GetComponent<UseItem>().GetInventorySize();
    }

    public void moveOneItem(int id) {
        if (heldItem == null && hotbar.ContainsKey(id)) {
            if (displayItems[id].stats["Current Count"] == 1) {
                setHeldItem(id);
                return;
            }

            prevKey = id;

            heldItemStats = new Item(displayItems[id]);
            heldItemStats.stats["Current Count"] = displayItems[id].stats["Current Count"]/2;
            displayItems[id].stats["Current Count"]-=heldItemStats.stats["Current Count"];

            heldItem = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            heldItem.transform.SetParent(cursor.transform, false);
            heldItem.gameObject.name = displayItems[id].name + " Inventory Item";
            heldItem.transform.localScale = Vector3.one;
            var reference = heldItem.AddComponent<DroppedItemReference>();
            reference.SetItem(heldItemStats);


            Vector3 p = cursor.transform.position;

            GameObject obj = heldItem.transform.GetChild(0).gameObject;
            obj.GetComponent<Image>().sprite = displayItems[id].icon;

            obj.transform.position = new Vector3(p.x,p.y,p.z-1);
            obj.gameObject.name = displayItems[id].name + " Inventory Item";


            
            GameObject text = heldItem.transform.GetChild(1).gameObject;
            text.transform.position = new Vector3(obj.transform.position.x+0.08f,obj.transform.position.y-0.1f,obj.transform.position.z);
            
            UpdateHeldItemText(heldItem, heldItemStats.stats["Current Count"]);
            UpdateInventoryText(id,displayItems[id].stats["Current Count"]);

        }
        else if (heldItem != null && !hotbar.ContainsKey(id)) {
            //heldItem.transform.position = new Vector3(heldItem.transform.position.x-0.1f,heldItem.transform.position.y+0.1f,heldItem.transform.position.z);
            //heldItem.transform.SetParent(transform.GetChild(id).transform, false);

            if (heldItemStats.stats["Current Count"] == 1) {
                setHeldItem(id);
                return;
            }

            Item newItem = new Item(heldItemStats);
            newItem.stats["Current Count"] = 1;
            heldItemStats.stats["Current Count"]--;

            GameObject pf = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            pf.transform.SetParent(transform.GetChild(id).transform);
            pf.gameObject.name = newItem.name + " Inventory Item";
            pf.transform.localScale = Vector3.one;
            var reference = pf.AddComponent<DroppedItemReference>();
            reference.SetItem(newItem);

            Vector3 p = transform.GetChild(id).transform.position;
            //pf.transform.position = new Vector3(p.x,p.y,p.z-1);

            GameObject obj = pf.transform.GetChild(0).gameObject;
            obj.GetComponent<Image>().sprite = newItem.icon;

            obj.transform.position = new Vector3(p.x,p.y,p.z-1);
            obj.gameObject.name = newItem.name + " Inventory Item";


            
            GameObject text = pf.transform.GetChild(1).gameObject;
            text.transform.position = new Vector3(obj.transform.position.x+0.08f,obj.transform.position.y-0.1f,obj.transform.position.z);
            

            hotbar.Add(id,pf);
            UpdateInventoryText(id,newItem.stats["Current Count"]);
            UpdateHeldItemText(heldItem,heldItemStats.stats["Current Count"]);
            player.GetComponent<UseItem>().addInventory(id, newItem);

        }
        else if (heldItem != null && hotbar.ContainsKey(id)) {
            if (heldItemStats.name == displayItems[id].name) {
                if (displayItems[id].stats["Current Count"] >= displayItems[id].stats["Max Count"]) {
                    return;
                }
                if (heldItemStats.stats["Current Count"] == 1) {
                    prevKey = id;
                    Destroy(heldItem);
                    heldItemStats = null;
                    displayItems[id].stats["Current Count"]++;
                    UpdateInventoryText(id,displayItems[id].stats["Current Count"]);
                    return;
                }
                heldItemStats.stats["Current Count"]--;
                displayItems[id].stats["Current Count"]++;
                UpdateInventoryText(id,displayItems[id].stats["Current Count"]);
                UpdateHeldItemText(heldItem,heldItemStats.stats["Current Count"]);

            }
        }
    }

    public void ShiftDown(int id, int start, int end) {
        if (player.GetComponent<UseItem>().CheckAddItem(displayItems[id], start, end)) {
            int firstEmpty = -1;
            for (int i = start; i < end; i++) {
                if (firstEmpty==-1 && !displayItems.ContainsKey(i))
                    firstEmpty=i;
                if (displayItems.ContainsKey(i) && displayItems[i].name == displayItems[id].name) {
                    if (displayItems[i].stats["Current Count"] >= displayItems[i].stats["Max Count"]) {
                        continue;
                    }
                    else if (displayItems[i].stats["Current Count"]+displayItems[id].stats["Current Count"] <= displayItems[i].stats["Max Count"]) {
                        displayItems[i].stats["Current Count"]+=displayItems[id].stats["Current Count"];
                        displayItems.Remove(id);
                        UpdateInventoryText(i,displayItems[i].stats["Current Count"]);
                        Destroy(hotbar[id]);
                        hotbar.Remove(id);
                        return;
                    }
                    displayItems[id].stats["Current Count"]=displayItems[i].stats["Max Count"] - displayItems[i].stats["Current Count"];
                    displayItems[i].stats["Current Count"]=displayItems[i].stats["Max Count"];
                    UpdateInventoryText(id,displayItems[id].stats["Current Count"]);
                    UpdateInventoryText(i,displayItems[i].stats["Current Count"]);
                }
            }
            if (firstEmpty > -1) {
                hotbar[id].transform.SetParent(transform.GetChild(firstEmpty),false);
                hotbar.Add(firstEmpty, hotbar[id]);
                hotbar.Remove(id);
                displayItems.Add(firstEmpty, displayItems[id]);
                displayItems.Remove(id);
                return;

            }
        }
    }

    public void setHeldItem(int id) {
        if (heldItem == null && hotbar.ContainsKey(id)) {
            prevKey = id;
            heldItem = hotbar[id];
            heldItemStats = displayItems[id];
            hotbar.Remove(id);
            heldItem.transform.SetParent(cursor.transform, false);
            player.GetComponent<UseItem>().removeInventory(id);
            heldItem.transform.position = new Vector3(heldItem.transform.position.x,heldItem.transform.position.y,heldItem.transform.position.z);
        }
        else if (heldItem != null && !hotbar.ContainsKey(id)) {
            heldItem.transform.position = new Vector3(heldItem.transform.position.x,heldItem.transform.position.y,heldItem.transform.position.z);
            heldItem.transform.SetParent(transform.GetChild(id).transform, false);
            hotbar.Add(id,heldItem);
            heldItem = null;
            player.GetComponent<UseItem>().addInventory(id,heldItemStats);
            heldItemStats = null;

        }
        else if (heldItem != null && hotbar.ContainsKey(id)) {
            if (heldItemStats.name == displayItems[id].name && displayItems[id].stats["Max Count"] > displayItems[id].stats["Current Count"]) {
                if (heldItemStats.stats["Current Count"] + displayItems[id].stats["Current Count"] <= heldItemStats.stats["Max Count"]) {
                    displayItems[id].stats["Current Count"] +=heldItemStats.stats["Current Count"];
                    UpdateInventoryText(id, displayItems[id].stats["Current Count"]);
                    heldItemStats = null;
                    Destroy(heldItem);
                    heldItem = null;
                }
                else {
                    int diff = displayItems[id].stats["Max Count"] - displayItems[id].stats["Current Count"];
                    displayItems[id].stats["Current Count"] = displayItems[id].stats["Max Count"];
                    heldItemStats.stats["Current Count"] = diff;
                    UpdateInventoryText(id, displayItems[id].stats["Current Count"]);
                    UpdateHeldItemText(heldItem,heldItemStats.stats["Current Count"]);
                }
                return;
            }
            var temp2 = displayItems[id];
            player.GetComponent<UseItem>().switchInventory(id,heldItemStats);
            heldItemStats = temp2;
            heldItem.transform.position = new Vector3(heldItem.transform.position.x,heldItem.transform.position.y,heldItem.transform.position.z);
            var temp = hotbar[id];
            heldItem.transform.SetParent(transform.GetChild(id).transform, false);
            hotbar[id]=heldItem;
            heldItem = temp;
            heldItem.transform.SetParent(cursor.transform, false);
            heldItem.transform.position = new Vector3(heldItem.transform.position.x,heldItem.transform.position.y,heldItem.transform.position.z);
        }
    }

    // Update is called once per frame
    public void switchSelector(int currentItem) {
        Vector3 p = transform.GetChild(currentItem).transform.position;
        selector.transform.position = new Vector3(p.x,p.y,selector.transform.position.z);
        player.GetComponent<UseItem>().setCurrentItem(currentItem);
    }
    public void AddToHotbar(int loc, Item item) {
        GameObject pf = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        var reference = pf.AddComponent<DroppedItemReference>();
        reference.SetItem(item);
        pf.transform.SetParent(transform.GetChild(loc).transform);
        pf.gameObject.name = item.name + " Inventory Item";
        pf.transform.localScale = Vector3.one;

        Vector3 p = transform.GetChild(loc).transform.position;
        //pf.transform.position = new Vector3(p.x,p.y,p.z-1);

        GameObject obj = pf.transform.GetChild(0).gameObject;
        obj.GetComponent<Image>().sprite = item.icon;

        obj.transform.position = new Vector3(p.x,p.y,p.z-1);
        obj.gameObject.name = item.name + " Inventory Item";


        
        GameObject text = pf.transform.GetChild(1).gameObject;
        //var t = text.GetComponent<Text>();
        text.transform.position = new Vector3(obj.transform.position.x+0.08f,obj.transform.position.y-0.1f,obj.transform.position.z);
        

        //t.alignment = TextAnchor.MiddleCenter;
        //t.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        //t.fontSize = 14;
        //text.gameObject.layer = 5;
        hotbar[loc] = pf;
        UpdateInventoryText(loc,item.stats["Current Count"]);
    }

    public void AddToHotbarHelper(int loc, int itemID, int amount) {
        Item item = new Item(ItemDatabaseObject.GetComponent<ItemDatabase>().items[itemID]);
        item.stats["Current Count"]=amount;
        AddToHotbar(loc, item);
        displayItems.Add(loc, item);

    }
    public void ClearChestInventory() {
        for (int i = 50;i<80;i++) {
            if (displayItems.ContainsKey(i)) {
                displayItems.Remove(i);
            }
        }
    }
    public void ClearInventory() {
        for (int i = 0;i<50;i++) {
            if (displayItems.ContainsKey(i)) {
                displayItems.Remove(i);
                Destroy(hotbar[i]);
                hotbar.Remove(i);
            }
        }
    }

    public bool CheckItemComponents(Recipe recipe, int count) {
        foreach (KeyValuePair<int, int> pair in recipe.components)
        {
            int missingComponents = pair.Value*count;
            for (int i = 0; i<GetInventorySize();i++) {
                if (displayItems.ContainsKey(i) && displayItems[i].id == pair.Key) {
                    missingComponents-=displayItems[i].stats["Current Count"];
                    if (missingComponents <= 0) {
                        break;
                    }

                }
            }
            if (missingComponents > 0)
                return false;
        }
        return true;
    }
    public int CheckMaxItemCraft(Recipe recipe) {
        int minCraft = 100;
        foreach (KeyValuePair<int, int> pair in recipe.components)
        {
            int componentCount = 0;
            for (int i = 0; i<GetInventorySize();i++) {
                if (displayItems.ContainsKey(i) && displayItems[i].id == pair.Key) {
                    componentCount+=displayItems[i].stats["Current Count"];
                }
            }
            minCraft = Mathf.Min(minCraft, componentCount/pair.Value);
        }
        return minCraft;
    }
    public void RemoveItemComponents(Recipe recipe, int count) {
        foreach (KeyValuePair<int, int> pair in recipe.components)
        {
            int missingComponents = pair.Value*count;
            for (int i = 0; i<GetInventorySize();i++) {
                if (displayItems.ContainsKey(i) && displayItems[i].id == pair.Key) {
                    if (displayItems[i].stats["Current Count"]-missingComponents < 0) {
                        missingComponents-=displayItems[i].stats["Current Count"];
                        displayItems[i].stats["Current Count"]=0;
                    }
                    else {
                        displayItems[i].stats["Current Count"]-=missingComponents;
                        missingComponents=0;

                    }
                    UpdateInventoryText(i, displayItems[i].stats["Current Count"]);
                    if (displayItems[i].stats["Current Count"] <= 0) {
                        RemoveFromHotbar(i);
                    }
                    if (missingComponents == 0){
                        break;
                    }

                }
            }
        }
    }
    public void UpdateInventoryText(int loc, int num) {
        hotbar[loc].transform.GetChild(1).GetComponent<Text>().text = "" + num;
        if (num > 1)
            hotbar[loc].transform.GetChild(1).gameObject.SetActive(true);
        else
            hotbar[loc].transform.GetChild(1).gameObject.SetActive(false);
    }
    public void UpdateHeldItemText(GameObject obj, int num) {
        obj.transform.GetChild(1).GetComponent<Text>().text = "" + num;
        if (num > 1)
            obj.transform.GetChild(1).gameObject.SetActive(true);
        else
            obj.transform.GetChild(1).gameObject.SetActive(false);
    }
    public void RemoveFromHotbar(int loc) {
        Destroy(hotbar[loc]);
        hotbar.Remove(loc);
        player.GetComponent<UseItem>().removeInventory(loc);
    }
    public void DropHeldItem(){
        if (heldItem != null) {
            GameObject item = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
            item.AddComponent<SpriteRenderer>();
            Item i = ItemDatabaseObject.GetComponent<ItemDatabase>().items[heldItemStats.id];
            item.GetComponent<SpriteRenderer>().sprite = i.icon;
            item.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Sprites/Materials/My diffuse");


            item.transform.parent = null;
            item.transform.localScale = new Vector3(0.25f,0.25f,1);
            Vector3 a = player.transform.TransformPoint((Vector2)player.GetComponent<BoxCollider2D>().offset);
            item.transform.position = new Vector3(a.x,a.y,0);

            item.GetComponent<SpriteRenderer>().sortingOrder=-Mathf.FloorToInt(a.y*100/32);

            item.gameObject.name = i.name +" "+ heldItemStats.stats["Current Count"];
            item.AddComponent<Rigidbody2D>();

            var r = item.AddComponent<DroppedItemReference>();
            r.SetItem(heldItemStats);

            var temp = item.AddComponent<ItemDropped>();

            Vector2 dir = new Vector2(0,1.5f);
            float d = player.GetComponent<Movement>().direction;
            if (d == 0)
                dir.y= -2f;
            else if (d == 1)
                dir.x = -2;
            else if (d == 2)
                dir.x = 2;
            else
                dir.y = 2f;
            temp.SetDir(dir);

            Destroy(heldItem);
            heldItemStats = null;
        }
    }
    private bool ConsumeItem(Item item, int numInInventory) {
        int type = item.stats["Consumable Type"];
        if (type==0) {
            //This type is being deprecated
            int recipeID = item.stats["Recipe ID"];
            FoundRecipes r = player.GetComponent<FoundRecipes>();
            if (!r.CheckIfFound(recipeID)) {
                r.AddRecipe(recipeID);
                List<string> displayText = new List<string>();
                displayText.Add("You now know how to make " + item.name);
                player.GetComponent<TextController>().TriggerTextEvent(displayText);
            }
            else {
                return false;
            }
        }
        item.stats["Current Count"]--;
        if (item.stats["Current Count"] == 0) {
            RemoveFromHotbar(numInInventory);
            return true;
        }
        UpdateInventoryText(numInInventory, item.stats["Current Count"]);
        return true;
    }

}
