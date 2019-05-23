using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // the furthest distance from the player at which the enemy can damage the player
    public float m_distanceToDamagePlayer = 2f;
    public int m_shotsToKill = 3;
    public int m_scoreValue = 100;

    // stores a reference to the NavMeshAgent on the enemy GameObject
    private NavMeshAgent m_agent;
    // stores the current health of the enemy
    private int m_nHealth;

    void Awake()
    {
        // initialise health
        m_nHealth = m_shotsToKill;
        // get reference to nav mesh agent
        m_agent = GetComponent<NavMeshAgent>();
        // start moving towards player
        m_agent.SetDestination(Camera.main.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // check if agent is close enough to damage the player
        if (m_agent.remainingDistance <= m_distanceToDamagePlayer)
        {
            // damage player
            Camera.main.GetComponent<PlayerController>().TakeDamage();
        }
    }

    /* @brief Takes damage, dies if has no more health
    */
    public void TakeDamage()
    {
        // decrement health
        m_nHealth--;
        // check if dead
        if (m_nHealth <= 0)
        {
            // get reference to score manager
            ScoreManager manager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
            // set multiplier timer
            manager.SetMultiplierTimer();
            // add score
            manager.AddScore(m_scoreValue);
            // increment score multiplier
            manager.IncrementMultiplier();
            // remove enemy
            GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>().RemoveEnemy();
            Destroy(gameObject); // delete
        }
    }
}