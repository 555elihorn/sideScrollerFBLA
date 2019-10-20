using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    GameSession GS;


    //Config
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);
    [SerializeField] float respawnDelayTime = 1;

    void Start()
    {
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
        GS = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        DeathCheck();
    }

    //Corutine that waits X amount of time before respawn
    IEnumerator Respawn()
    {

       yield return new WaitForSeconds(respawnDelayTime);
       GS.ProcessPlayerDeath();
    }



    private void DeathCheck() {
        if(isAlive())
        {
            if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Obstacles")) || myFeet.IsTouchingLayers(LayerMask.GetMask("Enemy", "Obstacles")))
            {
                Kill();
                myAnimator.SetTrigger("Dying");
                GetComponent<Rigidbody2D>().velocity = deathKick;
                StartCoroutine(Respawn());
            }
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
