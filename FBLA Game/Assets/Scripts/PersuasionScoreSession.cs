using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersuasionScoreSession : MonoBehaviour
{

    [SerializeField] int winCondition = 10;
    [SerializeField] int score = 0;

    [SerializeField] TextMeshProUGUI scoreText = null;

    void Start()
    {
        print("check");
        scoreText.text = score.ToString() + " / " + winCondition.ToString();
    }

    public void AddToScore()
    {
        score += 1;
        if (score == winCondition)
        {
            SceneManager.LoadScene(FindObjectOfType<GameSession>().GetPreviousScene());
        }
        else
        {
            scoreText.text = score.ToString() + " / " + winCondition.ToString();
        }
    }

    public void ResetGameSession()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Destroy(gameObject);
    }

}
