using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void StartFirstLevel()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void loadInstruction()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false; //This temporary, just to stop game in editor
        Application.Quit();
    }
}

