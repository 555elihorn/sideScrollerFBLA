using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    //config
    [SerializeField] float LevelLoadDelay = 1f;
    [SerializeField] GameObject scenePersist;
    

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
        int nextSceneIndex = currentSceneIndex + 1;
        string nextSceneName = NameOfSceneByBuildIndex(nextSceneIndex);

        print(nextSceneName);

        
        
        if (nextSceneName.Equals("Level 1"))
        {
            print("check");
        }
        else if (nextSceneName.Equals("Level 2"))
        {
            
            FindObjectOfType<GameSession>().changeWinCondition(2000);
            print("level 2: check!!");
        }
        else if (nextSceneName.Equals("Level 3"))
        {
            FindObjectOfType<GameSession>().changeWinCondition(3000);
        }
        else
        {
            //FindObjectOfType<GameSession>().changeWinCondition(4000);
        }
        
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public static string NameOfSceneByBuildIndex(int buildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }

}