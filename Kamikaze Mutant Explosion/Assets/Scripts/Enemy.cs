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
    // grenade prefab to be dropped on death
    public GameObject m_grenade;
    // values to set to the grenade that's dropped
    public int m_droppedGrenadeDamage = 1;
    public float m_droppedGrenadeExplosionRadius = 1f;
    public float m_droppedGrenadeTimer = 1f;
    public float m_droppedGrenadeExplosionScale = 1f;
    // how long to wait when reached the player until exploding
    public float m_timeToWaitBeforeExploding = 1f;
    // how much damage to deal when exploding on player
    public int m_damageDealtToPlayer = 1;

    // stores a reference to the NavMeshAgent on the enemy GameObject
    private NavMeshAgent m_agent;
    // reference to the audio source component of the GameObject
    private AudioSource m_audioSource;
    // stores a reference to the score manager
    private ScoreManager m_scoreManager;
    // stores the current health of the enemy
    private int m_nHealth;

    void Awake()
    {
        // get reference to score manager
        m_scoreManager = FindObjectOfType<ScoreManager>().GetComponent<ScoreManager>();
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
        // don't update if nav mesh agent is disabled
        if (!m_agent.enabled)
            return;

        // check if agent is close enough to damage the player
        if (Vector3.Distance(transform.position, Camera.main.transform.position) <= m_distanceToDamagePlayer)
        {
            // stop walking
            m_agent.enabled = false;
            // invoke explode function after n seconds
            Invoke("Explode", m_timeToWaitBeforeExploding);
            // stop animating
            if (GetComponent<Animator>())
                GetComponent<Animator>().enabled = false;
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
        // if dead and not already died
        if (m_nHealth <= 0
            && GetComponent<Collider>().enabled)
        {
            // cancel explosion function
            CancelInvoke();
            // drop grenade
            Grenade grenade = Instantiate(m_grenade, transform.position, Quaternion.Euler(Vector3.zero)).GetComponent<Grenade>();
            // set grenade values
            grenade.m_explodeRadius = m_droppedGrenadeExplosionRadius;
            grenade.m_explosionDamage = m_droppedGrenadeDamage;
            grenade.m_timeToExplode = m_droppedGrenadeTimer;
            grenade.m_explosionParticle.transform.localScale = new Vector3(m_droppedGrenadeExplosionScale, m_droppedGrenadeExplosionScale, m_droppedGrenadeExplosionScale);
            // set grenade to thrown
            grenade.m_bThrown = true;
            // set multiplier timer
            m_scoreManager.SetMultiplierTimer();
            // add score
            m_scoreManager.AddScore(m_scoreValue);
            // increment score multiplier
            m_scoreManager.IncrementMultiplier();
            // remove enemy
            GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>().RemoveEnemy();
            // disable (still plays audio clip)
            GetComponentInChildren<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 5); // destroy after 5 seconds
        }
    }

    private void Explode()
    {
        // don't explode if already dead
        if (!GetComponent<Collider>().enabled)
            return;

        // create grenade
        Grenade grenade = Instantiate(m_grenade, transform.position, Quaternion.Euler(Vector3.zero)).GetComponent<Grenade>();
        // set grenade values
        grenade.m_explodeRadius = m_droppedGrenadeExplosionRadius;
        grenade.m_explosionDamage = 0;
        grenade.m_timeToExplode = 0f;
        grenade.m_explosionParticle.transform.localScale = new Vector3(m_droppedGrenadeExplosionScale, m_droppedGrenadeExplosionScale, m_droppedGrenadeExplosionScale);
        // set grenade to thrown
        grenade.m_bThrown = true;
        // damage player
        Camera.main.GetComponent<PlayerController>().TakeDamage(m_damageDealtToPlayer);
        // remove enemy
        GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>().RemoveEnemy();
        // destroy enemy after short delay (to prevent player seeing enemy suddenly disappearing)
        Destroy(gameObject, 0.2f);
    }
}