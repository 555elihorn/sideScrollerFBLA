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

    GameObject player;


    //config
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI livesText = null;
    [SerializeField] TextMeshProUGUI scoreText = null;
    [SerializeField] int winCondition = 5000;

    private void Awake()
    {
        //print(gameObject.name + " : Awake Check");
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SetPreviousScene(currentSceneIndex);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if(ScoreIsEqualToWinCondition())
        {
            scoreText.color = Color.green;
        }
        else
        {
            scoreText.color = Color.white;
        }
    }

   private void Start()
    {
        scoreText.text = score.ToString() + " / " + winCondition.ToString();
        livesText.text = playerLives.ToString();
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString() + " / " + winCondition.ToString();
    }

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

    private void TakeLife()
    {

        playerLives--;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
        
    }

    public void ResetGameSession()
    {
        SceneManager.LoadScene("Menu");
        Destroy(gameObject);
    }

    public void SetPreviousScene(int sceneIndex)
    {
        previousScene = sceneIndex;
    }

    public int GetPreviousScene()
    {
        return previousScene;
    }
    public bool ScoreIsEqualToWinCondition()
    {
        return score >= winCondition;
    }

    public void TemporarilyHoldPlayerTransform(Transform newPosition)
    {
        playerPosition = newPosition;

        playerPositionVector = new Vector3(newPosition.position.x, newPosition.position.y, newPosition.position.z);
        playerScaleVector = new Vector3(newPosition.localScale.x, newPosition.localScale.y, newPosition.localScale.z);

    }

    public Vector3 GetTemporaryPosition()
    {
        return playerPositionVector;
    }

    public Vector3 GetTemporaryScale()
    {
        return playerScaleVector;
    }
}
