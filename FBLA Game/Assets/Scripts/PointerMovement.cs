using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PointerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidBody;

    bool isTouchingEnd = false;
    bool isInWinArea = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            print(isInWinArea); 
            if(isInWinArea)
            {
                print("WIN!");
            }
            else
            {
                print("YOU SHOULD HAVE LOST!");
            }
        }

        myRigidBody.velocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player is not specifically detecting the green area, do not change direction
        if ((collision.gameObject.tag.Equals("GREEN")))
        {
            //print("CHECK!");
            bool isInWinArea = true;
        }
        else
        {
            moveSpeed *= -1;
        }
    }

}
