using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void StartFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void loadInstruction()
    {
        SceneManager.LoadScene(2);
    }

    public void loadMain()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false; //This temporary, just to stop game in editor
        Application.Quit();
    }
}

