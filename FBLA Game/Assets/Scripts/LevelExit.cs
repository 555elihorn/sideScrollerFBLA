using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{

    [SerializeField] float LevelLoadDelay = 1f;
    [SerializeField] GameObject scenePersist;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (FindObjectOfType<GameSession>().ScoreIsEqualToWinCondition())
        {
            Destroy(scenePersist);
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