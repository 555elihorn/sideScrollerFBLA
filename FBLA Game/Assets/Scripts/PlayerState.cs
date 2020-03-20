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
    public bool isLevelReturn = true;

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
                print("CASE 1 selected");
                selectedGameSession = numGameSessions[1];
                
                
                if(numGameSessions[0].name.Equals(numGameSessions[1].name))
                {
                    print("names are equal");
                    numGameSessions[1].SetIfNewLevel(false);
                    /*
                    numGameSessions[1].SetTemporaryLocation(numGameSessions[0].GetTemporaryLocation());
                    numGameSessions[1].SetTemporaryScale(numGameSessions[0].GetTemporaryScale());
                    numGameSessions[1].SetIsLevelReturn(numGameSessions[0].GetIsLevelReturn());
                    */
                }
                
                
                
            }
            
            if (selectedGameSession.GetIfNewLevel())
            {
                print("IS NEW LEVEL");
                /*
                isLevelReturn = selectedGameSession.GetIsLevelReturn();
                Vector3 originalPlayerLocation = selectedGameSession.GetTemporaryLocation();
                Vector3 originalPlayerScale = selectedGameSession.GetTemporaryScale();

                transform.position = new Vector3(originalPlayerLocation.x, originalPlayerLocation.y, originalPlayerLocation.z);
                transform.localScale = new Vector3(originalPlayerScale.x, originalPlayerScale.y, originalPlayerScale.z);
                */
            }
            else
            {
                print("Returning from game!");
                isLevelReturn = selectedGameSession.GetIsLevelReturn();
                Vector3 originalPlayerPosition = selectedGameSession.GetTemporaryLocation();
                Vector3 originalPlayerScale = selectedGameSession.GetTemporaryScale();

                if (originalPlayerPosition.x != 0 && isLevelReturn)
                {
                    transform.position = new Vector3(originalPlayerPosition.x, originalPlayerPosition.y, originalPlayerPosition.z);
                    transform.localScale = new Vector3(originalPlayerScale.x, originalPlayerScale.y, originalPlayerScale.z);
                    isLevelReturn = false;
                    selectedGameSession.SetIsLevelReturn(false);
                }
                else
                {
                    Vector3 defaultPosition = selectedGameSession.getDefaultPosition();
                    Vector3 defaultScale = selectedGameSession.getDefaultScale();


                    print("LOADING DEFAULT POSITIONS");
                    transform.position = new Vector3(defaultPosition.x, defaultPosition.y, defaultPosition.z);
                    transform.localScale = new Vector3(defaultScale.x, defaultScale.y, defaultScale.z);
                }
                
            }

        }
        else
        {
            if (FindObjectOfType<GameSession>().GetIfNewLevel()) //IF completely fresh level
            {
                print("CHECK");
                FindObjectOfType<GameSession>().SetIfNewLevel(false);
            }
            else
            {
                print("RUNNING ELSE CASE!");
                isLevelReturn = FindObjectOfType<GameSession>().GetIsLevelReturn();
                Vector3 originalPlayerLocation = FindObjectOfType<GameSession>().GetTemporaryLocation();
                Vector3 originalPlayerScale = FindObjectOfType<GameSession>().GetTemporaryScale();

                if (originalPlayerLocation.x != 0 && isLevelReturn)
                {
                    transform.position = new Vector3(originalPlayerLocation.x, originalPlayerLocation.y, originalPlayerLocation.z);
                    transform.localScale = new Vector3(originalPlayerScale.x, originalPlayerScale.y, originalPlayerScale.z);
                }
                else
                {
                    Vector3 defaultPosition = FindObjectOfType<GameSession>().getDefaultPosition();
                    Vector3 defaultScale = FindObjectOfType<GameSession>().getDefaultScale();

                    print("LOADING DEFAULT POSITIONS");
                    transform.position = new Vector3(defaultPosition.x, defaultPosition.y, defaultPosition.z);
                    transform.localScale = new Vector3(defaultScale.x, defaultScale.y, defaultScale.z);
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
