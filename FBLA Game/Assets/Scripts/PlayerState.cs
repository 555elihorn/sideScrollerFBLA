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

        if (numGameSessions.Length > 1) //if # of gamesessions is > 1
        {
            GameSession selectedGameSession;

            //Block determines the chosen game session
            if (numGameSessions[0].getPlayerXPos() != 0.0) //if player x position is not 0
            {
                selectedGameSession = numGameSessions[0]; //select the first game session
            }
            else
            {
                selectedGameSession = numGameSessions[1]; //select the 2nd game session
                
                if(numGameSessions[0].name.Equals(numGameSessions[1].name)) //if the game sessions are from the same level
                {
                    if(numGameSessions[0].getDefaultPosition() != numGameSessions[1].getDefaultPosition()) //if the default respawn points are not equal
                    {
                        numGameSessions[1].setDefaultPosition(numGameSessions[0].getDefaultPosition()); //set the respawn points equal to one another
                    }

                    numGameSessions[1].SetIfNewLevel(false); //since the game sessions are from the same level, the player has not entered a new level
                }
            }
            
            if (selectedGameSession.GetIfNewLevel()) //is this a new level
            {
                selectedGameSession.resetDefaultPosition(); //reset the respawn point to the default respawn point of the respective level
            }
            else
            {
                isLevelReturn = selectedGameSession.GetIsLevelReturn(); //check if the player is returning from a mini game
                Vector3 originalPlayerPosition = selectedGameSession.GetTemporaryLocation();
                Vector3 originalPlayerScale = selectedGameSession.GetTemporaryScale();

                if (originalPlayerPosition.x != 0 && isLevelReturn) //if the original player x position is not 0 and the player is returning from a mini game
                {
                    //return the player to the point of conversation
                    transform.position = new Vector3(originalPlayerPosition.x, originalPlayerPosition.y, originalPlayerPosition.z);
                    transform.localScale = new Vector3(originalPlayerScale.x, originalPlayerScale.y, originalPlayerScale.z);
                    isLevelReturn = false;
                    selectedGameSession.SetIsLevelReturn(false);
                }
                else
                {
                    //Return the player to the default position ie: checkpoint or start of the level
                    Vector3 defaultPosition = selectedGameSession.getDefaultPosition();
                    Vector3 defaultScale = selectedGameSession.getDefaultScale();

                    transform.position = new Vector3(defaultPosition.x, defaultPosition.y, defaultPosition.z);
                    transform.localScale = new Vector3(defaultScale.x, defaultScale.y, defaultScale.z);
                }
                
            }

        }
        else //if there is only one game session
        {
            if (FindObjectOfType<GameSession>().GetIfNewLevel()) //if completely fresh level
            {
                FindObjectOfType<GameSession>().SetIfNewLevel(false); //the level is no longer new
            }
            else //if the level is not new
            {
                isLevelReturn = FindObjectOfType<GameSession>().GetIsLevelReturn(); //check if the player is returning from a mini game
                Vector3 originalPlayerLocation = FindObjectOfType<GameSession>().GetTemporaryLocation();
                Vector3 originalPlayerScale = FindObjectOfType<GameSession>().GetTemporaryScale();

                if (originalPlayerLocation.x != 0 && isLevelReturn) //if the original player x position is not 0 and the player is returning from a mini game
                {
                    //return the player to the point of conversation
                    transform.position = new Vector3(originalPlayerLocation.x, originalPlayerLocation.y, originalPlayerLocation.z);
                    transform.localScale = new Vector3(originalPlayerScale.x, originalPlayerScale.y, originalPlayerScale.z);
                }
                else //if player is not returning from new level
                {
                    //Return the player to the default position ie: checkpoint or start of the level
                    Vector3 defaultPosition = FindObjectOfType<GameSession>().getDefaultPosition();
                    Vector3 defaultScale = FindObjectOfType<GameSession>().getDefaultScale();

                    transform.position = new Vector3(defaultPosition.x, defaultPosition.y, defaultPosition.z);
                    transform.localScale = new Vector3(defaultScale.x, defaultScale.y, defaultScale.z);
                }
            }

        }

    }

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

}
