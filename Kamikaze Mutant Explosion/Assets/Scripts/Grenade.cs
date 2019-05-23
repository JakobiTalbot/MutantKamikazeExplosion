using UnityEngine;

public class Grenade : MonoBehaviour
{
    // the radius of the explosion
    public float m_explodeRadius = 3f;
    // the time until the grenade explodes
    public float m_timeToExplode = 4f;
    // the damage dealt by the explosion
    public int m_explosionDamage = 1;

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
        // count down on timer
        m_timeToExplode -= Time.deltaTime;

        // explode bomb if timer has reached 0
        if (m_timeToExplode <= 0f)
        {
            // play explosion audio
            m_audioSource.PlayOneShot(m_audioSource.clip);
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