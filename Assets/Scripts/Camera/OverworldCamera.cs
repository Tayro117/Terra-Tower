using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldCamera : MonoBehaviour
{

    public GameObject player;
    public GameObject currentBackground;


    private Vector3 offset;

    // Use this for initialization
    void Start()
    {

        transform.position = player.transform.position + new Vector3(0, 0, -10);

    }

    // Update is called once per frame
    void Update()
    {
        float cX = player.transform.position.x;
        float cY = player.transform.position.y;


        transform.position = player.transform.position + new Vector3(0, 0, -10);
    }

}
