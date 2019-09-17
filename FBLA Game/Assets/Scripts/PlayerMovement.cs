using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{

    //config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    //Cached components
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeet;
    PlayerState myState;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        myState = GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!myState.isAlive())
        {
            return;
        }

        Run();
        FlipSprite();
        Jump();
    }

    private void Run()
    {
        float directionFinder = CrossPlatformInputManager.GetAxis("Horizontal"); //value between -1 to 1 (neg = left movement)
        Vector2 playerVelocity = new Vector2(directionFinder * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        //For jumping animation
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void Jump()
    {
        //designed to stop wall climbing
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Foreground"))) { return; } // if the feet collider is not touching the ground return

        
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void FlipSprite()
    {
        //if player is moving horizontally, reverse scaling of x axis.

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }

    }
}
