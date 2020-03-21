using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameSession : MonoBehaviour
{

    //variables
    int previousScene;
    Transform playerPosition;
    Vector3 playerDefaultScaleVector;
    Vector3 playerDefaultPositionVector;
    Vector3 playerPositionVector;
    Vector3 playerScaleVector;
    List<string> tempChildList;
    List<string> persuadedNPCS = new List<string>();
    bool isLevelReturn = false;
    


    //cache
    GameObject player;


    //config
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI livesText = null;
    [SerializeField] TextMeshProUGUI scoreText = null;
    [SerializeField] int winCondition = 5000;
    [SerializeField] bool newLevel = true;

    //tests
    [SerializeField] int index = 0;
    [SerializeField] float playerPosX = 0;

    //Awake is called at start of gameobject initalization
    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            DontDestroyOnLoad(gameObject);
        }

        //Determines the default respawn point of the player once they enter a level
        if(SceneManager.GetActiveScene().name.Equals("Level 1"))
        {
            setDefaultPosition(new Vector3(-10.52f, 8.96f, 0f));
            setDefaultScale(new Vector3(1, 1, 1));
        }
        else if(SceneManager.GetActiveScene().name.Equals("Level 2"))
        {
            setDefaultPosition(new Vector3(-5.41f, 0.497f, 0f));
            setDefaultScale(new Vector3(1, 1, 1));
        }
        else if (SceneManager.GetActiveScene().name.Equals("Level 3"))
        {
            setDefaultPosition(new Vector3(-64.68f, -10.25f, 0f));
            setDefaultScale(new Vector3(1, 1, 1));
        }
        else if (SceneManager.GetActiveScene().name.Equals("Level 4"))
        {
            setDefaultPosition(new Vector3(-28.56f, -15.56f, 0f));
            setDefaultScale(new Vector3(1, 1, 1));
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (ScoreIsEqualToWinCondition())
        {
            scoreText.color = Color.green;
        }
        else
        {
            scoreText.color = Color.white;
        }
    }

    //Called on the first frame
    private void Start()
    {
        scoreText.text = score.ToString() + " / " + winCondition.ToString();
        livesText.text = playerLives.ToString();
    }

    //method adds to score
    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString() + " / " + winCondition.ToString();
    }

    //Method checks the state of the player
    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            //do nothing
        }
        else
        {
            ResetGameSession();
        }
    }

    //take a player life
    private void TakeLife()
    {

        playerLives--;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();

    }

    //Method that resets game session
    public void ResetGameSession()
    {
        SceneManager.LoadScene("Menu");
        Destroy(gameObject);
    }

    //Sets previous scnee
    public void SetPreviousScene(int sceneIndex)
    {
        index = previousScene;
        previousScene = sceneIndex;
    }

    //Gets previous scene
    public int GetPreviousScene()
    {
        return previousScene;
    }

    //Checks if the score is equal
    public bool ScoreIsEqualToWinCondition()
    {
        return score >= winCondition;
    }

    //Temporarily holds player position while in mini game
    public void TemporarilyHoldPlayerPosition(Transform newPosition)
    {
        playerPosition = newPosition;

        playerPosX = newPosition.position.x;

        playerPositionVector = new Vector3(newPosition.position.x, newPosition.position.y, newPosition.position.z);
        playerScaleVector = new Vector3(newPosition.localScale.x, newPosition.localScale.y, newPosition.localScale.z);
    }

    //gets player vector position
    public Vector3 GetTemporaryLocation()
    {
        return playerPositionVector;
    }

    public float getPlayerXPos()
    {
        return playerPosX;
    }

    //gets player vector scale
    public Vector3 GetTemporaryScale()
    {
        return playerScaleVector;
    }

    public void SetTemporaryScale(Vector3 newVector)
    {
        playerScaleVector = newVector;
    }

    //Sets list of player vectors
    public void SetScenePersistChildList(List<string> newList)
    {
        tempChildList = newList;
    }

    //get list of player vectors
    public List<string> GetScenePersistChildList()
    {
        return tempChildList;
    }

    //methods resets coin list
    public void ResetCoinList()
    {
        tempChildList = null;
    }

    //Adds persuaded npc to persuaded npc list
    public void AddPersuadedNPC(string npcPosititon)
    {
        persuadedNPCS.Add(npcPosititon);
    }

    //get persuaded npc list
    public List<string> GetPersuadedNPCList()
    {
        return persuadedNPCS;
    }

    //chanege win condition on level transition
    public void changeWinCondition(int newCondition)
    {
        winCondition = newCondition;
        score = 0;
        scoreText.color = Color.white;
        scoreText.text = 0 + " / " + winCondition.ToString();
    }

    //Set if the player has entered a new level
    public void SetIfNewLevel(bool LevelChange)
    {
        newLevel = LevelChange;
    }

    //Set if the player 
    public void SetIsLevelReturn(bool LevelReturn)
    {
        isLevelReturn = LevelReturn;
    }

    //Check if the player returning from a mini game
    public bool GetIsLevelReturn()
    {
        return isLevelReturn;
    }

    //Check if player has entered new new level
    public bool GetIfNewLevel()
    {
        return newLevel;
    }

    //change the default position of the player
    //used for checkpoint system
    public void setDefaultPosition(Vector3 newVector)
    {
        playerDefaultPositionVector = newVector;
    }

    //change the default scale of the player
    //used for checkpoint system
    public void setDefaultScale(Vector3 newVector)
    {
        playerDefaultScaleVector = newVector;
    }

    //get default position of player
    public Vector3 getDefaultPosition()
    {
        return playerDefaultPositionVector;
    }

    //get default scale of player
    public Vector3 getDefaultScale()
    {
        return playerDefaultScaleVector;
    }

    //Resets Default Position to predetermined locations
    public void resetDefaultPosition()
    {
        if (SceneManager.GetActiveScene().name.Equals("Level 1"))
        {
            setDefaultPosition(new Vector3(-10.52f, 8.96f, 0f));
            setDefaultScale(new Vector3(1, 1, 1));
        }
        else if (SceneManager.GetActiveScene().name.Equals("Level 2"))
        {
            setDefaultPosition(new Vector3(-5.41f, 0.497f, 0f));
            setDefaultScale(new Vector3(1, 1, 1));
        }
        else if (SceneManager.GetActiveScene().name.Equals("Level 3"))
        {
            setDefaultPosition(new Vector3(-64.68f, -10.25f, 0f));
            setDefaultScale(new Vector3(1, 1, 1));
        }
        else if (SceneManager.GetActiveScene().name.Equals("Level 4"))
        {
            setDefaultPosition(new Vector3(-28.56f, -15.56f, 0f));
            setDefaultScale(new Vector3(1, 1, 1));
        }
    }
}
