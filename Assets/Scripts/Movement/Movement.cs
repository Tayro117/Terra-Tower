using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Animator animator;
    public bool menuActive;
    public GameObject SwitchTab;
    private bool canMove = false;
    public GameObject menu;
    public GameObject InventoryButtons;
    public float speed;             //******************This script is for overworld movement*******************************
    private Rigidbody2D rb2d;
    public int direction = 0;
    private GameObject PlaceableItemSquare;
    private List<string> menuTabs = new List<string>();
    private bool chestMenuActive = false;
    public GameObject chestMenu;
    private string chestName;
    private int floor = 0;
    private AudioSource audio;
    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
        PlaceableItemSquare = GameObject.Find("PlaceableItemSquare");
        menuTabs.Add("CraftingBackground");
        foreach (string s in menuTabs) {
            GameObject.Find("Canvas/Menu/"+s).SetActive(false);
        }
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        menu.SetActive(false);
        chestMenu.SetActive(false);
        for (int i = 10;i< 80;i++) {
            InventoryButtons.transform.GetChild(i).gameObject.SetActive(false);
        }
        //animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove) {
            return;
        }
        //Store the current horizontal input in the float moveHorizontal.
        if (Input.GetKeyDown("q") && !menuActive){
        	OpenMenu();

        }
        else if ((Input.GetKeyDown("q") || (Input.GetKeyDown("e") && InventoryButtons.GetComponent<DisplayHotBar>().getMenuTab() == "InventoryMenuTab")) && menuActive){
            if (chestMenuActive) {
                chestMenu.SetActive(false);
                chestMenuActive = false;
                SaveSystem.SaveChest(InventoryButtons, chestName);
                ClearChest();
                for (int i = 50;i< 80;i++) {
                    InventoryButtons.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        	menuActive = false;
        	menu.SetActive(false);
            if (InventoryButtons.GetComponent<DisplayHotBar>().IsHeldItem()) {
                InventoryButtons.GetComponent<DisplayHotBar>().DropHeldItem();
                PlaceableItemSquare.SetActive(false);
            }
            if (InventoryButtons.GetComponent<DisplayHotBar>().getMenuTab() == "InventoryMenuTab")
                for (int i = 10;i< GetComponent<UseItem>().GetInventorySize();i++) {
                    InventoryButtons.transform.GetChild(i).gameObject.SetActive(false);
                }
        }
        else if (Input.GetKeyDown("e") && !menuActive){
            OpenInventory();
        }
        else if (Input.GetKeyDown("e") && menuActive && InventoryButtons.GetComponent<DisplayHotBar>().getMenuTab() != "InventoryMenuTab") {
            SwitchTab.GetComponent<SwitchTab>().Switch();
        }

        if (!menuActive){
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");
        	move(moveHorizontal,moveVertical);
    	}

        



    }
    public int GetFloorNum() {
        return floor;
    }
    public bool GetChestMenuActive() {
        return chestMenuActive;
    }

    public bool GetCanMove() {
        return canMove;
    }
    public void SetCanMove(bool b) {
        canMove=b;
    }
    public void StopMovement() {
        animator.SetInteger("Direction", 3);
        rb2d.velocity = Vector2.zero;
    }

    private void ClearChest() {
        for (int i = 50; i < 80; i ++) {
            foreach(Transform child in InventoryButtons.transform.GetChild(i)) {
                Destroy(child.gameObject);
            }
        }
        InventoryButtons.GetComponent<DisplayHotBar>().ClearChestInventory();
    }
    public void OpenMenu() {
        menuActive = true;
        StopMovement();
        menu.SetActive(true);
        if (InventoryButtons.GetComponent<DisplayHotBar>().getMenuTab() == "InventoryMenuTab")
            for (int i = 10;i< GetComponent<UseItem>().GetInventorySize();i++) {
                InventoryButtons.transform.GetChild(i).gameObject.SetActive(true);
            }
    }
    public void OpenInventory() {
        menuActive = true;
        StopMovement();
        menu.SetActive(true);
        SwitchTab.GetComponent<SwitchTab>().Switch();
        if (InventoryButtons.GetComponent<DisplayHotBar>().getMenuTab() == "InventoryMenuTab")
            for (int i = 10;i< GetComponent<UseItem>().GetInventorySize();i++) {
                InventoryButtons.transform.GetChild(i).gameObject.SetActive(true);
            }
    }
    public void OpenChestMenu(string name) {
        menuActive = true;
        chestName=name;
        chestMenuActive = true;
        StopMovement();
        //menu.SetActive(true);
        chestMenu.SetActive(true);
        for (int i = 10;i< GetComponent<UseItem>().GetInventorySize();i++) {
            InventoryButtons.transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 50;i< 80;i++) {
            InventoryButtons.transform.GetChild(i).gameObject.SetActive(true);
        }
        ChestData cd = SaveSystem.LoadChest(name);
        if (cd!=null) {
            for (int i = 0; i<30; i++) {
                if (cd.chestItems[i] >= 0) {
                    InventoryButtons.GetComponent<DisplayHotBar>().AddToHotbarHelper(i+50, cd.chestItems[i], cd.inventoryItemCounts[i]);
                }
            }
        }
    }
    public string GetChestName() {
        return chestName;
    }


    private void move(float moveHorizontal, float moveVertical) {
        if (Mathf.Abs(moveHorizontal) > 0 && Mathf.Abs(moveVertical) > 0) {
            moveHorizontal = moveHorizontal/Mathf.Sqrt(2);
            moveVertical = moveVertical/Mathf.Sqrt(2);
        }
    	if (moveVertical > 0)
        {
            moveVertical = speed;
            animator.SetInteger("Direction", 4);
            direction = 4;
        }
        if (moveVertical < 0)
        {
            moveVertical = -speed;
            animator.SetInteger("Direction", 0);
            direction = 0;
        }
        if (moveHorizontal > 0)
        {
            moveHorizontal = speed;
            animator.SetInteger("Direction", 2);
            direction = 2;
        }
        if (moveHorizontal < 0)
        {
            moveHorizontal = -speed;
            animator.SetInteger("Direction", 1);
            direction = 1;
        }
        if (moveVertical == 0 && moveHorizontal == 0)
        {
            animator.SetInteger("Direction", 3);
            audio.Stop();
        }

        if ((moveHorizontal != 0 || moveVertical != 0) && !audio.isPlaying) {
            audio.Play();
        }

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        Vector3 pposition = transform.TransformPoint(GetComponent<BoxCollider2D>().offset);
        int playerX = Mathf.FloorToInt(pposition.x*100/32f);
        int playerY = Mathf.FloorToInt(pposition.y*100/32f);
        GetComponent<SpriteRenderer>().sortingOrder = -playerY;


        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        //rb2d. = new Vector2(0,0);
        //rb2d.AddForce(movement);
        //if (movement.magnitude > 0)
        rb2d.velocity = movement;
        //rb2d.MovePosition(rb2d.position + movement);
    }
}
