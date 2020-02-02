using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

    //enables the onfinishloading
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    //disables the onfinishloading
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    //Run whenever the level is loaded

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {

        GameSession[] numGameSessions = FindObjectsOfType<GameSession>();

        if (numGameSessions.Length > 1)
        {
            GameSession selectedGameSession;
            if (numGameSessions[0].getPlayerXPos() != 0.0)
            {
                selectedGameSession = numGameSessions[0];
            }
            else
            {
                selectedGameSession = numGameSessions[1];
            }
            
            if (selectedGameSession.GetIfNewLevel())
            {
                //do nothing
            }
            else
            {
                Vector3 originalPlayerLocation = selectedGameSession.GetTemporaryLocation();
                Vector3 originalPlayerScale = selectedGameSession.GetTemporaryScale();

                if (originalPlayerLocation.x != 0)
                {
                    transform.position = new Vector3(originalPlayerLocation.x, originalPlayerLocation.y, originalPlayerLocation.z);
                    transform.localScale = new Vector3(originalPlayerScale.x, originalPlayerScale.y, originalPlayerScale.z);
                }
            }

        }
        else
        {
            if (FindObjectOfType<GameSession>().GetIfNewLevel())
            {
                //do nothing
            }
            else
            {
                Vector3 originalPlayerLocation = FindObjectOfType<GameSession>().GetTemporaryLocation();
                Vector3 originalPlayerScale = FindObjectOfType<GameSession>().GetTemporaryScale();

                if (originalPlayerLocation.x != 0)
                {
                    transform.position = new Vector3(originalPlayerLocation.x, originalPlayerLocation.y, originalPlayerLocation.z);
                    transform.localScale = new Vector3(originalPlayerScale.x, originalPlayerScale.y, originalPlayerScale.z);
                }
            }


        }

    }

    /*
    private async Task WaitOneSecondAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(0.01));
        Debug.Log("Finished waiting.");
    }
    */

    // Start is called before the first frame update
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

    //checks if the player dies 
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

    //kills player
    private void Kill()
    {
        Life = false;
    }

    //checks if player isalive
    public bool IsAlive()
    {
        return Life;
    }

    

    //records player position before mini game
    

}
