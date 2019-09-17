using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerState : MonoBehaviour
{

    //State
    bool Life = true;

    //Cached components
    CapsuleCollider2D myBodyCollider;
    Animator myAnimator;
    BoxCollider2D myFeet;


    //Config
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);

    void Start()
    {
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        DeathCheck();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    private void DeathCheck() {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Obstacles")) || myFeet.IsTouchingLayers(LayerMask.GetMask("Enemy", "Obstacles")))
        {
            Kill();
            myAnimator.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            /*GameSession temp = FindObjectOfType<GameSession>();
            temp.ProcessPlayerDeath(); */
        }
    }

    private void Kill()
    {
        Life = false;
    }

    public bool isAlive()
    {
        return Life;
    }
}
