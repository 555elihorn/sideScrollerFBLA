using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    int startingSceneIndex;
    List<string> childrenList = new List<string>();

    //Awake is called at start of gameobject initalization
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

    //enables the onfinishloading
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    //disables the onfinishloading
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    //Ran whenever the level is loaded
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        List<string> originalList = null;

        if(SceneManager.GetActiveScene().name.Equals("Menu"))
        {
           //do nothing
        }
        else
        {
            originalList = FindObjectOfType<GameSession>().GetScenePersistChildList();
        }

        
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

    // Start is called before the first frame update
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

    //Gets the list of coins
    public void GetScenePersistChildren()
    {

        int children = transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            childrenList.Add(transform.GetChild(i).position.x.ToString());
        }
            
        FindObjectOfType<GameSession>().SetScenePersistChildList(childrenList);
    }

    public void destroySelf()
    {
        print("KILLING!");
        Destroy(this);
    }



}
