using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PointerMovement : MonoBehaviour
{

    //Variables
    private bool isInWinArea;
    private bool isPressingButton; //created to stop button spam

    //Cache
    Rigidbody2D myRigidBody;
    PersuasionScoreSession scoreSystem;

    //Configs
    [SerializeField] float moveSpeed = 1f;
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
        if (CrossPlatformInputManager.GetButtonDown("Jump") && !isPressingButton)
        {
            isPressingButton = true; //prevents button mashing
            if (isInWinArea)
            {
                AudioSource.PlayClipAtPoint(successSound, Camera.main.transform.position, 0.5f);
                scoreSystem.AddToScore();
            }
            else
            {
                AudioSource.PlayClipAtPoint(failSound, Camera.main.transform.position, 0.5f);
                //scoreSystem.Failure();
            }
        }

        myRigidBody.velocity = new Vector2(moveSpeed, 0f); //moves the arrow
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player is not specifically detecting the green area, change direction
        if(collision.gameObject.tag.Equals("GREEN"))
        {
            isPressingButton = false;
            isInWinArea = true;
        }
        else
        {
            moveSpeed *= -1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if the pointer leaves the green area, the player is no longer in the win area and can press the button again
        if(collision.gameObject.tag.Equals("GREEN"))
        {
            isPressingButton = false;
            isInWinArea = false;
        }

    }

}
