using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // the furthest distance from the player at which the enemy can damage the player
    public float m_fDistanceToDamagePlayer = 2f;
    public int m_shotsToKill = 3;

    // stores a reference to the NavMeshAgent on the enemy GameObject
    private NavMeshAgent m_agent;
    private int m_nHealth;

    void Awake()
    {
        m_nHealth = m_shotsToKill;
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.SetDestination(Camera.main.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // check if agent is close enough to damage the player
        if (m_agent.remainingDistance <= m_fDistanceToDamagePlayer)
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
            Destroy(gameObject); // delete
    }
}