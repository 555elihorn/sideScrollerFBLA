using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    int startingSceneIndex;
    List<GameObject> childrenList = new List<GameObject>();

    private void Awake()
    {
        int numScenePersist = FindObjectsOfType<ScenePersist>().Length;
        if (numScenePersist > 1)
        {
            print(gameObject.name);
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
        /*
        if (originalPlayerLocation.x != 0)
        {
            transform.position = new Vector3(originalPlayerLocation.x, originalPlayerLocation.y, originalPlayerLocation.z);
            transform.localScale = new Vector3(originalPlayerScale.x, originalPlayerScale.y, originalPlayerScale.z);
        }
        */

        var temp = FindObjectOfType<GameSession>().getScenePersistChildList();

        if (temp != null)
        {
            print("THIS SHIT AINT EMPTY");
            print("0: " + temp[0].name);
        }
        else
        {
            print("THIS SHIT EMPTY");
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
       // print("ChildrenCount:" + children);
        for (int i = 0; i < children; ++i)
        {
            childrenList.Add(transform.GetChild(i).gameObject);
            //print(i + ": " + childrenList[i].name);
        }
            
        FindObjectOfType<GameSession>().SetScenePersistChildList(childrenList);
    }

}
