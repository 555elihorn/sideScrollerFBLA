using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    //config
    [SerializeField] float LevelLoadDelay = 1f;
    [SerializeField] ScenePersist sc;

    

    //Whenever the player enters collide run this
    void OnTriggerEnter2D(Collider2D other)
    {
        if (FindObjectOfType<GameSession>().ScoreIsEqualToWinCondition())
        {
            FindObjectOfType<GameSession>().SetIfNewLevel(true);
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

        


        if (nextSceneName.Equals("Level 1"))
        {
            //do nothing
        }
        else if (nextSceneName.Equals("Level 2"))
        {
            
            FindObjectOfType<GameSession>().changeWinCondition(2000);
        }
        else if (nextSceneName.Equals("Level 3"))
        {
            FindObjectOfType<GameSession>().changeWinCondition(3000);
        }
        else
        {
            //FindObjectOfType<GameSession>().changeWinCondition(4000);
        }


        GameObject newGO = new GameObject();
        FindObjectOfType<ScenePersist>().transform.parent = newGO.transform;

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