using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    //loads first level
    public void StartFirstLevel()
    {
        SceneManager.LoadScene("Level 1");
    }

    //loads instruction
    public void LoadInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    //loads story
    public void LoadStory()
    {
        SceneManager.LoadScene("Story");
    }

    //loads mechanics
    public void LoadMechanics()
    {
        SceneManager.LoadScene("Mechanics");
    }

    //loads controls
    public void LoadControls()
    {
        SceneManager.LoadScene("Controls");
    }

    //loads main menu
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    
    //exit game
    public void ExitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false; //This temporary, just to stop game in editor
        Application.Quit();
    }
}

