using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    int startingSceneIndex;
    List<string> childrenList = new List<string>();

    private void Awake()
    {
        int numScenePersist = FindObjectsOfType<ScenePersist>().Length;
        if (numScenePersist > 1)
        {
            //print( this.gameObject.name + ": Imma kill" + gameObject.name);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled.
        //Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {

        var originalList = FindObjectOfType<GameSession>().GetScenePersistChildList();
        int childs = transform.childCount;
        var tempList = new List<Transform>();

        if (originalList != null)
        {
            for (int i = 0; i < childs; ++i)
            {
                tempList.Add(transform.GetChild(i));
            }


            for (int i = 0; i < childs; ++i)
            {
                Transform Currentchild = tempList[i];
                
                if(originalList.Contains(Currentchild.position.x.ToString()))
                {

                }
                else
                {
                    //Currentchild.gameObject.parent = null;
                    Destroy(Currentchild.gameObject);
                }

            }

            FindObjectOfType<GameSession>().ResetCoinList();
        }
        else
        {

        }
    }

    void Start()
    {
        startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(currentSceneIndex != startingSceneIndex)
        {
            Destroy(gameObject);
        }
    }

    public void GetScenePersistChildren()
    {

        int children = transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            childrenList.Add(transform.GetChild(i).position.x.ToString());
        }
            
        FindObjectOfType<GameSession>().SetScenePersistChildList(childrenList);
    }

}
