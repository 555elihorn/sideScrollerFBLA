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

        var originalList = FindObjectOfType<GameSession>().getScenePersistChildList();
        int childs = transform.childCount;
        var tempList = new List<Transform>();



        if (originalList != null)
        {
            for (int i = 0; i < childs; ++i)
            {
                print(i + ": " + transform.GetChild(i).position.x.ToString());
                tempList.Add(transform.GetChild(i));
                //print(i + ": " + childrenList[i].name);
            }

            print("THIS SHIT AINT EMPTY");

            // print("ChildrenCount:" + children);
            for (int i = 0; i < childs; ++i)
            {
                /*
                print(i + ": " + transform.GetChild(i).position.x.ToString());
                childrenList.Add(transform.GetChild(i).position.x.ToString());
                //print(i + ": " + childrenList[i].name);
                */

                Transform Currentchild = tempList[i];
                
                if(originalList.Contains(Currentchild.position.x.ToString()))
                {

                }
                else
                {
                    //Currentchild.gameObject.parent = null;
                    Destroy(Currentchild.gameObject);
                }


                /*
                for(int x = 0; x < children.Count; ++x)
                {
                    //firstlist.Contains(4)
                    if (tempChildrenList.get)
                    child.parent = null;
                    Destroy(child.gameObject);
                }
                */
            }

            

            print("0: " + originalList[0]);

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
            print(i + ": " + transform.GetChild(i).position.x.ToString());
            childrenList.Add(transform.GetChild(i).position.x.ToString());
            //print(i + ": " + childrenList[i].name);
        }
            
        FindObjectOfType<GameSession>().SetScenePersistChildList(childrenList);
    }

}
