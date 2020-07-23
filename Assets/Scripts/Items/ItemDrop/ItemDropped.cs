using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropped : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector2 dir;
    private float timer = 0.0f; 
    private float bounceTime = 0.25f;
    private int bounces = 2;
    private Rigidbody2D rb;
    
    public void SetDir(Vector2 d) {
        rb = transform.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = d;
        if (d.x == 0)
            rb.gravityScale=0;
        else
            rb.gravityScale=1.2f;
        rb.drag = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        if (bounces == 1 && timer > bounceTime) {
        	rb.drag = 0;
    		rb.gravityScale = 0;
    		rb.velocity=Vector2.zero;
    		bounceTime=0.1f;
    		timer=0;
    		bounces = 0;
        }
        else if (timer > bounceTime) {
        	if (bounces == 0) {
				transform.gameObject.tag = "Collectable";
				Destroy(transform.gameObject.GetComponent<ItemDropped>());
        	}
        	timer=0;
            if (rb.velocity.x==0)
                rb.velocity=new Vector2(0,rb.velocity.y+ -rb.velocity.y/2);
            else
        	   rb.velocity = new Vector2(rb.velocity.x*0.7f,-rb.velocity.y*0.5f);
        	bounces--;
            bounceTime-=.1f;
        }
    }
}
