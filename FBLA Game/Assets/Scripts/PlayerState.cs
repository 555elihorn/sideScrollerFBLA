using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    Transform temporaryPlayerPosition = null;


    //Config
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);
    [SerializeField] float respawnDelayTime = 1;

    void OnEnable()
    {
    //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled.
        //Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        var temp = FindObjectOfType<GameSession>().GetTemporaryLocation();
        print("TEST: " + temp.x);
        if(temp.x != 0)
        {
            /*print("OnLevelFinishedLoading" + temp.position.x);
            transform.position = temp.position;
            transform.rotation = temp.rotation;
            transform.localScale = temp.localScale;
            */

            transform.position = new Vector3((float)temp.x, (float)temp.y, (float)temp.z);
        }
        else
        {
            /*
            print("OnLevelFinishedLoading: Null!");
            */
        }
    }

    void Start()
    {
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
        temporaryPlayerPosition = GetComponent<Transform>();

        GS = FindObjectOfType<GameSession>();

        //SetPlayerPosition();
        
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
        temporaryPlayerPosition = GetComponent<Transform>();
        FindObjectOfType<GameSession>().TemporarilyHoldPlayerPosition(temporaryPlayerPosition);
    }

}
