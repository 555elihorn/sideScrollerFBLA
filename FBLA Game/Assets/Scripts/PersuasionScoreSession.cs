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

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        print("check");
        scoreText.text = score.ToString() + " / " + winCondition.ToString();
    }

    public void AddToScore()
    {
        score += 1;
        scoreText.text = score.ToString() + " / " + winCondition.ToString();
    }

    public void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
