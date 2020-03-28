using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{

    //variables
    bool isGreen = false;

    //Serializable fields
    [SerializeField] Sprite greenFlag;
    [SerializeField] Sprite redFlag;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        if(FindObjectOfType<GameSession>().getCheckPointStatus(gameObject.name) == null) //if checkpoint is not found in dictionary
        {
           //do nothing
        }
        else if(FindObjectOfType<GameSession>().getCheckPointStatus(gameObject.name) == true)
        {
            SetGreen(true);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag.Equals("Player")) //if collision is with player
        {
            if(GetComponent<SpriteRenderer>().sprite != greenFlag) //if the checkpoint has not already been reached before
            {
                SetCheckPointPosition();
            }
        }
    }

    private void SetCheckPointPosition() //creates checkpoint
    {
        //set sprite to green
        SetGreen(true);

        //set status to activated
        FindObjectOfType<GameSession>().setCheckpointKey(gameObject.name, true);


        //Sets default position
        FindObjectOfType<GameSession>().setDefaultPosition(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        FindObjectOfType<GameSession>().setDefaultScale(new Vector3(1, 1, 1));
    }

    public void SetGreen(bool colorChange)
    {
        isGreen = colorChange; //if colorchange variable is true, set the flag to green
        GetComponent<SpriteRenderer>().sprite = greenFlag;
    }

    public bool IsGreen()
    {
        return isGreen;
    }
}
