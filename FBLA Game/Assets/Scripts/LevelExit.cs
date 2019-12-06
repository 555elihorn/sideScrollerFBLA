using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    //config
    [SerializeField] float LevelLoadDelay = 1f;
    [SerializeField] GameObject scenePersist;
    [SerializeField] GameObject gameSession;

    //Whenever the player enters collide run this
    void OnTriggerEnter2D(Collider2D other)
    {
        if (FindObjectOfType<GameSession>().ScoreIsEqualToWinCondition())
        {
            Destroy(scenePersist);
            Destroy(gameSession);
            StartCoroutine(LoadNextLevel());
        }
    }

    //Load next level
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(LevelLoadDelay);

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

}