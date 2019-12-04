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
    GameSession GameSes;
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
        Vector3 originalPlayerLocation = FindObjectOfType<GameSession>().GetTemporaryLocation();
        Vector3 originalPlayerScale = FindObjectOfType<GameSession>().GetTemporaryScale();

        if (originalPlayerLocation.x != 0)
        {
            transform.position = new Vector3(originalPlayerLocation.x, originalPlayerLocation.y, originalPlayerLocation.z);
            transform.localScale = new Vector3(originalPlayerScale.x, originalPlayerScale.y, originalPlayerScale.z);
        }
    }

    void Start()
    {
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
        temporaryPlayerPosition = GetComponent<Transform>();

        GameSes = FindObjectOfType<GameSession>();
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
