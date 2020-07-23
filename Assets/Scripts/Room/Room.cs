using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Room: MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject rock;
    public GameObject log;
    public GameObject thatch;
    public GameObject tree;
    public GameObject pond;
    public GameObject copper;
    public GameObject iron;
    public GameObject backgroundTile;
    public GameObject background;
    public GameObject player;
    public GameObject UILoadingScreen;
    public GameObject hotbar;
    public GameObject TimeManager;
    public List<Item> items;
    public GridSystem startingRoom = new GridSystem(100,100,32);
    public GridSystem floorGrid = new GridSystem(100,100,32);

    private GameObject sceneParent;
    private string currentScene = "Nothing";
    private bool switchScreen = false;
    private int tileSize;
    private int floor;

    void Awake() {
        UILoadingScreen.SetActive(false);
        items = GameObject.Find("ItemDatabaseObject").GetComponent<ItemDatabase>().items;
        LoadCustomScene("Nothing");
        tileSize = startingRoom.getTileSize();
    }
    void Update() {
        if (switchScreen) {
            UnloadCustomScene(currentScene);
            LoadCustomScene("Nothing");
            UILoadingScreen.SetActive(false);
            switchScreen = false;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (floor == 0) {
                SaveSystem.SavePlayer(player.GetComponent<UseItem>(), player.GetComponent<FoundRecipes>());
                SaveSystem.SaveGrid(startingRoom, floorGrid);
            }
            UILoadingScreen.SetActive(true);
            switchScreen = true;
            floor++;
            GameObject.Find("FloorTracker").GetComponent<Text>().text = "" +floor;
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            SaveSystem.SavePlayer(player.GetComponent<UseItem>(), player.GetComponent<FoundRecipes>());
            SaveSystem.SaveGrid(startingRoom, floorGrid);
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            floor = 0;
            GameObject.Find("FloorTracker").GetComponent<Text>().text = "" +floor;
            hotbar.GetComponent<DisplayHotBar>().ClearInventory();
            player.GetComponent<FoundRecipes>().ClearRecipes();
            UnloadCustomScene(currentScene);
            LoadInventoryFromSaveFile();
            LoadGridFromSaveFile();
            UILoadingScreen.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            //TODO make it not redo recipe loading
            floor = 0;
            GameObject.Find("FloorTracker").GetComponent<Text>().text = "" +floor;
            UnloadCustomScene(currentScene);
            LoadGridFromSaveFile();
            UILoadingScreen.SetActive(false);
        }
    }
    public string GetCurrentScene() {
        return currentScene;
    }
    public GameObject GetSceneParent() {
        return sceneParent;
    }
    private void SpawnObjects(string name) {//add from scratch lloading method

        GameObject parentObjects = Instantiate(background, new Vector3(16,16,0.1f), Quaternion.identity);
        parentObjects.name = name;
        sceneParent = parentObjects;

        BuildRandomLayer(pond, 0, startingRoom, 1, parentObjects, 4.5f, 4.5f, 10, 10);
        BuildRandomLayer(rock, 0, startingRoom, 250, parentObjects, 0, 0, 1, 1);
        BuildRandomLayer(log, 0, startingRoom, 250, parentObjects, 0, 0, 1, 1);
        BuildRandomLayer(thatch, 0, startingRoom, 250, parentObjects, 0, 0, 1, 1);
        BuildRandomLayer(tree, 0, startingRoom, 250, parentObjects, 0, 2f, 1, 1);
        BuildRandomLayer(copper, 0, startingRoom, 200, parentObjects, 0, 0, 1, 1);
        BuildRandomLayer(iron, 0, startingRoom, 150, parentObjects, 0, 0, 1, 1);
        BuildLayer(backgroundTile, 0.1f, floorGrid, parentObjects);

        int row = startingRoom.getRowCount();
        int col = startingRoom.getColCount();
        int tileSize = startingRoom.getTileSize();
        int spawnXPlayer = Random.Range(0,row);
        int spawnYPlayer = Random.Range(0,col);
        while (startingRoom.checkGrid(spawnXPlayer,spawnYPlayer, 1, 1)) {
            spawnXPlayer = Random.Range(0,row);
            spawnYPlayer = Random.Range(0,col);
        }

        
        player.transform.position = new Vector3(spawnXPlayer*tileSize/100,spawnYPlayer*tileSize/100,-0.1f);

    }

    private void LoadGridFromSaveFile() {
        GridData d = SaveSystem.LoadGrid();
        //TODO lots of work to put in grid loading
        GameObject parentObjects = Instantiate(background, new Vector3(16,16,0.1f), Quaternion.identity);
        sceneParent=parentObjects;
        parentObjects.name=currentScene;
        BuildLayer(backgroundTile, 0.1f, floorGrid, parentObjects);

        for (int i = 0;i<100;i++) {
            for (int j = 0;j<100;j++) {
                if (d.grid_1[i,j] >= 0) {
                    if (!startingRoom.checkGrid(i,j,1,1)) {
                        PlaceItem(d.grid_1[i,j], d.gridProgression_1[i,j], i, j, parentObjects);
                        startingRoom.getItem(i,j).GetComponent<OnUseItem>().SetGrowth(d.gridProgression_1[i,j]);
                    }

                }
                if (d.gridProgression_2[i,j] >= 1) {
                    floorGrid.getItem(i,j).GetComponent<OnUseItem>().SetGrowth(d.gridProgression_2[i,j]);
                }
            }
        }
    }
    private void PlaceItem(int id, int growth, int x, int y, GameObject parent) {
        Item item = items[id];
        float dx = (float)(item.stats["Sprite Tile Width"]-1)/2;
        float dy = (float)(item.stats["Sprite Tile Height"]-1)/2;
        Vector3 loc = new Vector3((x+dx)*tileSize/100f+tileSize/200f, (y+dy)*tileSize/100f+tileSize/200f, 0);
        GameObject placedItem = Instantiate(Resources.Load<GameObject>("Prefabs/In World Prefabs/"+item.name),loc,Quaternion.identity);
        startingRoom.addToGrid(x,y,placedItem, item.stats["Placeable Length"], item.stats["Placeable Width"]);
        placedItem.GetComponent<OnUseItem>().SetGridPosition(x,y,item.stats["Placeable Length"], item.stats["Placeable Width"]);
        placedItem.transform.SetParent(parent.transform);
        placedItem.GetComponent<OnUseItem>().SetGrowth(growth);
        placedItem.GetComponent<SpriteRenderer>().sortingOrder = -y;
    }

    private void LoadInventoryFromSaveFile() {
        PlayerData d = SaveSystem.LoadPlayer();
        var UseItemController = player.GetComponent<UseItem>();
        var CraftingMenuController = player.GetComponent<FoundRecipes>();
        if (d!=null) {
            Debug.Log("Loaded from save file!");
            for (int i =0;i<50;i++) {
                if (d.inventoryItems[i]>=0) {
                    int count = d.inventoryItemCounts[i];
                    UseItemController.AddItemIndex(d.inventoryItems[i], i, count);
                }
            }
            for (int i =0;i<100;i++) {
                if (d.unlockedRecipes[i] >= 0)
                    CraftingMenuController.AddRecipe(d.unlockedRecipes[i]);
            }

        } 
        else {
            UseItemController.AddDefaultItems();
            CraftingMenuController.AddDefaultRecipes();
        }
    }

    private void LoadCustomScene(string name) {
        //player.GetComponent<UseItem>().SetPause(true);
        currentScene=name;
        SpawnObjects(name);
        //player.GetComponent<UseItem>().SetPause(false);

    }
    private void UnloadCustomScene(string name) {
        Destroy(GameObject.Find(name));
        startingRoom.emptyGrid();
        floorGrid.emptyGrid();
        TimeManager.GetComponent<TimeEvent>().UnsubscribeAll();
    }

    private void BuildRandomLayer(GameObject obj, float layer, GridSystem grid, int count, GameObject po, float dx, float dy, int x, int y) {
        int row = grid.getRowCount();
        int col = grid.getColCount();
        int tileSize = grid.getTileSize();
        for (int numRocks = 0; numRocks < count; numRocks += 1) {
            int spawnX = Random.Range(0,row);
            int spawnY = Random.Range(0,col);
            if (!grid.checkGrid(spawnX,spawnY, x, y)) {
                GameObject temp = Instantiate(obj, new Vector3((spawnX+dx)*tileSize/100f+tileSize/200f,(spawnY+dy)*tileSize/100f+tileSize/200f,layer),Quaternion.identity);
                grid.addToGrid(spawnX, spawnY, temp, x, y);
                temp.GetComponent<OnUseItem>().SetGridPosition(spawnX,spawnY,x,y);
                temp.transform.SetParent(po.transform);
                temp.GetComponent<SpriteRenderer>().sortingOrder = -spawnY;
            }
            else{
                numRocks-=1;
            }
        }
    }
    private void BuildLayer(GameObject obj, float layer, GridSystem grid, GameObject po) {
        int row = grid.getRowCount();
        int col = grid.getColCount();
        int tileSize = grid.getTileSize();
        for (int i = 0; i < row; i ++) {
            for (int j = 0; j < col; j ++)  {
                GameObject temp = Instantiate(obj, new Vector3(i*tileSize/100f+tileSize/200f,j*tileSize/100f+tileSize/200f,layer),Quaternion.identity);
                grid.addToGrid(i, j, temp, 1, 1);
                temp.GetComponent<OnUseItem>().SetGridPosition(i,j,1,1);
                temp.transform.SetParent(po.transform);
                temp.GetComponent<SpriteRenderer>().sortingLayerName = "DirtLayer";
            }
        }
    }

}
