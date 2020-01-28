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
            StartCoroutine(LoadNextLevel());
        }
    }

    //Load next level
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(LevelLoadDelay);

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        if(SceneManager.GetSceneByBuildIndex(currentSceneIndex + 1).name.Equals("Level 1"))
        {
            print("check");
        }
        else if (SceneManager.GetSceneByBuildIndex(currentSceneIndex + 1).name.Equals("Level 2"))
        {
            
            FindObjectOfType<GameSession>().changeWinCondition(2000);
            print("level 2: check!!");
        }
        else if (SceneManager.GetSceneByBuildIndex(currentSceneIndex + 1).name.Equals("Level 3"))
        {
            FindObjectOfType<GameSession>().changeWinCondition(3000);
        }
        else
        {
            //FindObjectOfType<GameSession>().changeWinCondition(4000);
        }

        SceneManager.LoadScene(currentSceneIndex + 1);
    }

}