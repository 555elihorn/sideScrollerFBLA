using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField] Sprite greenFlag;
    [SerializeField] Sprite redFlag;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag.Equals("Player")) //if collision is with player
        {
            if(GetComponent<SpriteRenderer>().sprite != greenFlag) //if the checkpoint has not already been reached before
            {
                SetCheckPointPosition();
            }
        }
    }

    private void SetCheckPointPosition() //creates checkpoint
    {
        GetComponent<SpriteRenderer>().sprite = greenFlag;

        //Sets default position
        FindObjectOfType<GameSession>().setDefaultPosition(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        FindObjectOfType<GameSession>().setDefaultScale(new Vector3(1, 1, 1));
    }
}
