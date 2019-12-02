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

    public void LoadInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void LoadStory()
    {
        SceneManager.LoadScene("Story");
    }

    public void LoadMechanics()
    {
        SceneManager.LoadScene("Mechanics");
    }

    public void LoadControls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false; //This temporary, just to stop game in editor
        Application.Quit();
    }
}

