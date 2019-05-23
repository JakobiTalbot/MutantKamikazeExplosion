using UnityEngine;

public class Grenade : MonoBehaviour
{
    // the radius of the explosion
    public float m_explodeRadius = 3f;
    // the time until the grenade explodes
    public float m_timeToExplode = 4f;
    // the damage dealt by the explosion
    public int m_explosionDamage = 1;
    // different explosion clip variants to play
    public AudioClip[] m_explosionAudioClips;
    public GameObject m_explosionParticle;

    // whether the grenade has been thrown or not
    [HideInInspector]
    public bool m_bThrown = false;

    // stores a reference to the GameObject's audio source
    private AudioSource m_audioSource;

    private void Awake()
    {
        // get reference to audio source
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // don't update if grenade not thrown
        if (!m_bThrown)
            return;

        // count down on timer
        m_timeToExplode -= Time.deltaTime;

        // explode bomb if timer has reached 0
        if (m_timeToExplode <= 0f)
        {
            // play random explosion audio clip
            m_audioSource.PlayOneShot(m_explosionAudioClips[Random.Range(0, m_explosionAudioClips.Length)]);
            // create particles (-90 x rotation because explosion prefab is weird)
            Destroy(Instantiate(m_explosionParticle, transform.position, Quaternion.Euler(-90, 0, 0)), 5f);
            // loop through each collider in a sphere radius
            foreach (Collider collider in Physics.OverlapSphere(transform.position, m_explodeRadius))
            {
                // if collider is an enemy
                if (collider.CompareTag("Enemy"))
                {
                    // deal damage to enemy
                    collider.GetComponent<Enemy>().TakeDamage(m_explosionDamage);
                }
            }
            // disable components
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
            Destroy(gameObject, 5); // destroy after 5 seconds (as to not cancel audio clip)
            enabled = false; // disable script
        }
    }
}