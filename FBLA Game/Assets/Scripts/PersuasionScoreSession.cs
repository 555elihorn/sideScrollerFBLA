using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersuasionScoreSession : MonoBehaviour
{

    //Cache
    GameSession myGameSession;

    //Config
    [SerializeField] int winCondition = 10;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI scoreText = null;

    void Start()
    {
        myGameSession = FindObjectOfType<GameSession>();
        scoreText.text = score.ToString() + " / " + winCondition.ToString(); //creates the default score parameters (ex: 0 / 10)
    }

    public void AddToScore()
    {
        score += 1;
        if (score == winCondition)
        {
            myGameSession = FindObjectOfType<GameSession>();
            SceneManager.LoadScene(myGameSession.GetPreviousScene()); //If the player completes the win condition go back to the main level
            myGameSession.AddToScore(1000);
        }
        else
        {
            scoreText.text = score.ToString() + " / " + winCondition.ToString(); //else add +1 to the score
        }
    }

    public void Failure() //Exit play mode
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Destroy(gameObject);
    }



}
