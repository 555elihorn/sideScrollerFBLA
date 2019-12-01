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


    //config
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI livesText = null;
    [SerializeField] TextMeshProUGUI scoreText = null;
    [SerializeField] int winCondition = 5000;

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
            SetPreviousScene(currentSceneIndex);
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
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
}
