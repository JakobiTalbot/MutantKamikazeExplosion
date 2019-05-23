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
    // the current number of enemies alive
    private int m_nEnemyCount = 0;
    // stores a reference the the movement controller
    private MovementController m_movementController;
    // stores the count of waves spawned
    [HideInInspector]
    public int m_nWavesSpawned = 0;

    private void Awake()
    {
        m_movementController = Camera.main.GetComponent<MovementController>();
    }

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
            m_nWavesSpawned++;
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
            m_nEnemyCount++;
        }
    }

    /*  @brief Increments the waves to be spawn for each point by 1
    */
    public void IncrementWavesToSpawn()
    {
        m_nWavesToSpawn++;
    }

    /*  @brief Decrements enemy count
               Makes the movement controller move to the next point if all enemies are dead and all waves have been spawned
    */
    public void RemoveEnemy()
    {
        // decrement enemy count
        m_nEnemyCount--;
        //Debug.Log(m_nEnemyCount);
        Debug.Log(m_nWavesToSpawn);
        // checks if enemy count is 0 and all waves have been spawned
        if (m_nEnemyCount <= 0
            && m_nWavesSpawned == m_nWavesToSpawn)
        {
            // reset waves spawned to 0
            m_nWavesSpawned = 0;
            // move to next point
            m_movementController.MoveToNextPoint();
        }
    }
}