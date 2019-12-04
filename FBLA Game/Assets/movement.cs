using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class movement : MonoBehaviour
{

    //Variables
    private bool isInWinArea;
    private bool isPressingButton; //created to stop button spam
    private int count = 0;

    //Cache
    Rigidbody2D myRigidBody;
    PersuasionScoreSession scoreSystem;

    //Configs
    [SerializeField] float moveSpeed = 0;
    [SerializeField] AudioClip successSound = null;
    [SerializeField] AudioClip failSound = null;


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        scoreSystem = FindObjectOfType<PersuasionScoreSession>();
        isInWinArea = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (myRigidBody.transform.position.x > -6.44)
        {
            

            count++;
            if (count == 4)
            {
                if (!(moveSpeed < 0))
                {
                    moveSpeed -= 3;
                }
                count = 0;
            }

            if (isInWinArea)
            {
                scoreSystem.AddToScore();
            }
        } else
            {
            moveSpeed = 0;
            }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {

            moveSpeed += 4;
        }
        myRigidBody.velocity = new Vector2(moveSpeed, 0f); //moves the arrow
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if the pointer is in the green
        if(collision.gameObject.tag.Equals("GREEN"))
        {
            isInWinArea = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //check if the pointer is not in the green
        if(collision.gameObject.tag.Equals("GREEN"))
        {
            isInWinArea = false;
        }

    }

}
