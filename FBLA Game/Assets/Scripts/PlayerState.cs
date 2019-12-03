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
    Transform playerTransform = null;


    //Config
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);
    [SerializeField] float respawnDelayTime = 1;

    void Start()
    {
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
        playerTransform = GetComponent<Transform>();

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
       FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    private void DeathCheck() {
        if(IsAlive())
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

    public bool IsAlive()
    {
        return Life;
    }

    public void RecordPlayerPosition()
    {
        print("GETTING THE Player Position");
        playerTransform = GetComponent<Transform>();
        print(playerTransform.position.x);
    }
}
