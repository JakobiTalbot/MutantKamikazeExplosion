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
    public GameObject[] m_enemyTypes;
    public MDArray[] m_points;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* @brief Spawns enemies at the spawn points assigned to the point index
       @param The index of the point to spawn enemies at
    */
    public void SpawnEnemies(int iPointIndex)
    {
        foreach (GameObject spawnPoint in m_points[iPointIndex].enemySpawners)
        {
            Instantiate(m_enemyTypes[Random.Range(0, m_enemyTypes.Length)], spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }
}