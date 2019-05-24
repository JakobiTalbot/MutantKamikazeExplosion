using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // reference to the score to be displayed when the game ends
    public Text m_scoreText;
    // reference to the text that shows the highscore
    public Text m_highScoreText;
    // reference to the object which displays when the player gets a new highscore
    public GameObject m_newHighScore;

    public void Display(int nScore)
    {
        ScoreManager manager = FindObjectOfType<ScoreManager>();
        m_scoreText.text = nScore.ToString();
        // if we beat the highscore record
        if (nScore > manager.LoadHighscore())
        {
            // display as new highscore
            m_highScoreText.text = nScore.ToString();
            // save new highscore
            manager.SaveHighscore(nScore);
            // enable text congratulating player
            m_newHighScore.SetActive(true);
        }
        else // display highscore
            m_highScoreText.text = manager.LoadHighscore().ToString();
    }

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