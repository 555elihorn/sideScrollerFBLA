using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{

    [SerializeField] float LevelLoadDelay = 1f;
    [SerializeField] GameObject scenePersist;
    [SerializeField] GameObject gameSession;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (FindObjectOfType<GameSession>().ScoreIsEqualToWinCondition())
        {
            Destroy(scenePersist);
            Destroy(gameSession);
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(LevelLoadDelay);

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

}