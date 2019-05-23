using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// serialised 2d array
[System.Serializable]
public class MDArray
{
    public GameObject[] enemySpawners;
}

public class EnemySpawner : MonoBehaviour
{
    // array of references to different enemy prefabs
    public GameObject[] m_enemyTypes;
    // 2d array of enemy spawn points per movement point
    public MDArray[] m_points;
    // time between the spawning of enemies for waves
    public float m_timeBetweenWavesSpawning = 5f;

    // stores the amount of waves there currently is to spawn
    private int m_nWavesToSpawn = 1;
    // stores the index of the current point
    private int m_iCurrentPoint = 0;

    /* @brief Spawns enemies at the spawn points assigned to the point index
       @param The index of the point to spawn enemies at
    */
    public void SpawnEnemies(int iPointIndex)
    {
        // set current point index
        m_iCurrentPoint = iPointIndex;
        // invoke instantiating enemies every x amount of seconds for each wave
        for (int i = 0; i < m_nWavesToSpawn; ++i)
        {
            Invoke("InstantiateEnemies", m_timeBetweenWavesSpawning * i);
        }
    }

    /*  @brief Spawns random enemy on each point
    */
    private void InstantiateEnemies()
    {
        // loop through each spawn point
        foreach (GameObject spawnPoint in m_points[m_iCurrentPoint].enemySpawners)
        {
            // spawn random enemy type
            Instantiate(m_enemyTypes[Random.Range(0, m_enemyTypes.Length)], spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }

    /*  @brief Increments the waves to be spawn for each point by 1
    */
    public void IncrementWavesToSpawn()
    {
        m_nWavesToSpawn++;
    }
}