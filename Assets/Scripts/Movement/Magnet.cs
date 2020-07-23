using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{

	private float range = 0.5f;
	private List<Item> itemList;
    // Start is called before the first frame update
    void Start()
    {
    	itemList = GameObject.Find("ItemDatabaseObject").GetComponent<ItemDatabase>().items;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Collectable");
        if (pickups.Length > 0) {
        	Vector2 playerPosition = transform.TransformPoint((Vector2)GetComponent<BoxCollider2D>().offset);

	        foreach (GameObject pickup in pickups) {
	        	if (GetComponent<UseItem>().CheckAddItem(pickup.GetComponent<DroppedItemReference>().GetItem(), 0, GetComponent<UseItem>().GetInventorySize())) {
		        	float d = Vector2.Distance(playerPosition, (Vector2)pickup.transform.position);
		        	if (d < range) {
		        		if (d < 0.24f) {
		        			string n = pickup.gameObject.name;
		        			if (GetComponent<UseItem>().AddItem(pickup.GetComponent<DroppedItemReference>().GetItem())) {
		        				Destroy(pickup.gameObject);
		        			}
		        			continue;
		        		}
		        		float dx = (-playerPosition.x +pickup.transform.position.x);
		        		float dy = (-playerPosition.y +pickup.transform.position.y);
		        		float total = Mathf.Abs(dx)+Mathf.Abs(dy);
		        		dx = dx/total/50/(d+0.1f);
		        		dy = dy/total/50/(d+0.1f);

		        		pickup.transform.position = new Vector3(pickup.transform.position.x-dx,pickup.transform.position.y-dy,pickup.transform.position.z);
		        		
		        	}
		        }
	        }
        }

    }
}
