﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // stores a reference to the text on the UI which displays the player's score
    public Text m_scoreText;
    // stores a reference to the text on the UI which displays the player's score multiplier
    public Text m_multiplierText;
    // time before multiplier is reset to 1
    public float m_timeToResetMultiplier = 0.5f;

    // the score the player currently has
    private int m_nCurrentScore = 0;
    // the current score multiplier
    private int m_nScoreMulti = 1;
    // timer to reset multiplier
    private float m_fResetMultiTimer = 0;

    private void Update()
    {
        // counts down on the multiplier reset timer
        m_fResetMultiTimer -= Time.deltaTime;
        // reset multiplier if less than 0
        if (m_fResetMultiTimer <= 0)
            SetMultiplier(1);
    }

    /*  @brief Adds score the the current score then sets the text to the new value
        @param The score to add to the current score
    */
    public void AddScore(int nScore)
    {
        // increases score by parameter amount
        m_nCurrentScore += nScore;
        // sets score text to new score
        m_scoreText.text = m_nCurrentScore.ToString();
    }

    /*  @brief Sets the multiplier to a new value
        @param The new multiplier
    */
    public void SetMultiplier(int nMultiplier)
    {
        // set multiplier to parameter
        m_nScoreMulti = nMultiplier;
        // sets multiplier text to new multiplier
        m_multiplierText.text = m_nScoreMulti.ToString();
    }

    /*  @brief Increments the multiplier value by one
    */
    public void IncrementMultiplier()
    {
        // increment multiplier value
        m_nScoreMulti++;
        // sets multiplier text to new multiplier value
        m_multiplierText.text = m_nScoreMulti.ToString();
    }

    /*  @brief Sets the score multiplier timer to the time required to reset timer
               Should be called every time an enemy is hit
    */
    public void SetMultiplierTimer()
    {
        // set timer
        m_fResetMultiTimer = m_timeToResetMultiplier;
    }
}