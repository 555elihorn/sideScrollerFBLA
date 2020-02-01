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
    Vector3 playerPositionVector;
    Vector3 playerScaleVector;
    List<string> tempChildList;
    List<string> persuadedNPCS = new List<string>();
    


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
            //SetPreviousScene(currentSceneIndex);
            DontDestroyOnLoad(gameObject);

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
        print("SETTING PREVIOUS SCENE!");
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

    public void resetPlayerLists()
    {
        playerPositionVector = new Vector3(0, 0, 0);
        playerScaleVector = new Vector3(0, 0, 0);
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

    public void changeWinCondition(int newCondition)
    {
        winCondition = newCondition;
        score = 0;
        scoreText.color = Color.white;
        scoreText.text = 0 + " / " + winCondition.ToString();
    }

    public void SetIfNewLevel(bool LevelChange)
    {
        newLevel = LevelChange;
    }

    public bool GetIfNewLevel()
    {
        return newLevel;
    }
}
