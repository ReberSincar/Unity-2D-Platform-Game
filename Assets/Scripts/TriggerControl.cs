using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerControl : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerController;
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "Floor")
        {
            playerController.isOnFloor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            playerController.isOnFloor = false;
        }
    }
}
