using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // reference to the score to be displayed when the game ends
    public Text m_scoreText;

    /*  @brief The function called when the retry button is pressed; restarts the level
    */
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    /*  @brief The function called when the main menu button is pressed; returns to the main menu
    */
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    /*  @brief The function called when the quit button is pressed; exits the game
    */
    public void Quit()
    {
        Application.Quit();
    }
}