using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /*  @brief The function called when the start button is pressed, loads the game scene
    */
    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    /*  @brief The function called when the quit button is pressed, exits the application
    */
    public void QuitButton()
    {
        Application.Quit();
    }
}