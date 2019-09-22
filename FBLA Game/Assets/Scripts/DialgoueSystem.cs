using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialgoueSystem : MonoBehaviour
{

    bool playerIsTouching = false;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerIsTouching)
        {
            print("Nice");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            playerIsTouching = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            playerIsTouching = false;
        }
    }
}
