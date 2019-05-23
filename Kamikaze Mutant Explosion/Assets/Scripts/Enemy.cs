using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // the furthest distance from the player at which the enemy can damage the player
    public float m_distanceToDamagePlayer = 2f;
    // shots required to kill the enemy
    public int m_shotsToKill = 3;
    // the score value of killing the enemy
    public int m_scoreValue = 100;
    // clips played at random
    public AudioClip[] m_gruntAudioClips;
    // clips played when hit
    public AudioClip[] m_hitAudioClips;

    // stores a reference to the NavMeshAgent on the enemy GameObject
    private NavMeshAgent m_agent;
    // reference to the audio source component of the GameObject
    private AudioSource m_audioSource;
    // stores the current health of the enemy
    private int m_nHealth;

    void Awake()
    {
        // get reference to audio source
        m_audioSource = GetComponent<AudioSource>();
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
    public void TakeDamage(int nDamage)
    {
        // decrement health
        m_nHealth -= nDamage;
        // play enemy hit sound
        m_audioSource.PlayOneShot(m_hitAudioClips[Random.Range(0, m_hitAudioClips.Length)]);
        m_audioSource.PlayOneShot(m_gruntAudioClips[Random.Range(0, m_gruntAudioClips.Length)]);
        // check if dead
        if (m_nHealth <= 0)
        {
            // adds grenades
            Camera.main.GetComponent<PlayerController>().AddGrenades(1);
            // get reference to score manager
            ScoreManager manager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
            // set multiplier timer
            manager.SetMultiplierTimer();
            // add score
            manager.AddScore(m_scoreValue * manager.GetMultiplier());
            // increment score multiplier
            manager.IncrementMultiplier();
            // remove enemy
            GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>().RemoveEnemy();
            // disable (still plays audio clip)
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 5); // destroy after 5 seconds
        }
    }
}