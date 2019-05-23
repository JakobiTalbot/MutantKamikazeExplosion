using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // texture for the crosshair on screen
    public GameObject m_crosshair;
    // mouse sensitivity multiplier
    public float m_mouseSensitivity = 300f;
    // texture for muzzle flash when firing
    public GameObject m_muzzleFlash;
    // time for the muzzle flash to be on screen
    public float m_muzzleFlashTime = 0.1f;
    // cooldown between shooting
    public float m_shootCooldown = 0.1f;
    // amount to move the camera by if rotating camera
    public float m_cameraRotationCoefficient = 500f;
    // range of the pitch of audio source
    public Vector2 m_minMaxPitchRange = new Vector2(1, 2);
    // damage dealt with shots
    public int m_bulletDamage = 1;
    // prefab for grenade
    public GameObject m_grenade;
    // the amount of grenades the player has at the start of the game
    public int m_startGrenadeCount = 3;
    // the maximum amount of grenades the player can have
    public int m_maxGrenadeCount = 3;
    // the velocity for the grenade to be thrown
    public float m_grenadeThrowVelocity = 100f;
    // number of points for the trajectory line renderer to have
    public int m_trajectoryPointCount = 30;
    // stores a reference to the grenades on the UI
    public GameObject[] m_grenadeUIs;

    // rotation based off camera movement to be added to base rotation
    [HideInInspector]
    public Vector3 m_v3AddedRotation;

    // how many lives the player has
    private int m_nLives = 3;
    // timer to count until the muzzle flash disappears
    private float m_fMuzzleFlashTimer = 0f;
    // timer to count until the player can shoot again
    private float m_fShootTimer = 0f;
    // the last position of the crosshair
    private Vector3 m_v3LastCrosshairPos;
    // reference to the audiosource on this gameobject
    private AudioSource m_audioSource;
    // the current amount of grenades the player has
    private int m_nGrenadeCount;
    // whether the player is holding grenade out or not
    private bool m_bHoldingGrenade = false;
    // held grenade object
    private GameObject m_heldGrenade;
    // reference to line renderer component
    private LineRenderer m_lineRenderer;

    void Awake()
    {
        // get reference to line renderer
        m_lineRenderer = GetComponent<LineRenderer>();
        // set position count
        m_lineRenderer.positionCount = m_trajectoryPointCount;
        // disable line renderer
        m_lineRenderer.enabled = false;
        // set starting grenade count
        m_nGrenadeCount = m_startGrenadeCount;
        // get reference to audio source
        m_audioSource = GetComponent<AudioSource>();
        // set cursor to crosshair
        Cursor.lockState = CursorLockMode.Locked;
        // initialise last crosshair position
        m_v3LastCrosshairPos = m_crosshair.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // decrement timers
        m_fShootTimer -= Time.deltaTime;
        m_fMuzzleFlashTimer -= Time.deltaTime;

        // move crosshair
        Vector3 v3NewPos = m_crosshair.transform.position;
        v3NewPos += new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * m_mouseSensitivity * Time.deltaTime;
        // clamp to screen boundaries
        v3NewPos.x = Mathf.Clamp(v3NewPos.x, 0, Screen.width);
        v3NewPos.y = Mathf.Clamp(v3NewPos.y, 0, Screen.height);
        // set crosshair position
        m_crosshair.transform.position = v3NewPos;

        // rotate camera
        m_v3AddedRotation.x -= (m_crosshair.transform.position.y - m_v3LastCrosshairPos.y) * m_cameraRotationCoefficient / Screen.width * Time.deltaTime;
        m_v3AddedRotation.y += (m_crosshair.transform.position.x - m_v3LastCrosshairPos.x) * m_cameraRotationCoefficient / Screen.height * Time.deltaTime;
        Vector3 rot;
        rot = m_v3AddedRotation + GetComponent<MovementController>().GetCurrentPoint().transform.rotation.eulerAngles;
        if (!GetComponent<MovementController>().m_bMovingPoints)
            transform.localRotation = Quaternion.Euler(rot);

        // set last crosshair position
        m_v3LastCrosshairPos = m_crosshair.transform.position;
        // disable muzzle flash if still active and timer has expired
        if (m_muzzleFlash.activeSelf
            && m_fMuzzleFlashTimer <= 0f)
            m_muzzleFlash.SetActive(false);
        else if (m_muzzleFlash.activeSelf) // set position to follow crosshair if muzzle flash still enabled
            m_muzzleFlash.transform.position = m_crosshair.transform.position;

        // if the player left clicks and can shoot
        if (Input.GetMouseButtonDown(0)
            && m_fShootTimer <= 0f
            && !m_bHoldingGrenade)
        {
            // display muzzle flash, rotate muzzle flash randomly, set timer
            m_muzzleFlash.transform.position = m_crosshair.transform.position;
            m_muzzleFlash.SetActive(true);
            m_muzzleFlash.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
            m_fMuzzleFlashTimer = m_muzzleFlashTime;

            // set random weapon pitch
            m_audioSource.pitch = Random.Range(m_minMaxPitchRange.x, m_minMaxPitchRange.y);
            // play gunshot sound
            m_audioSource.PlayOneShot(m_audioSource.clip);

            // create raycast hit data
            RaycastHit hit = new RaycastHit();

            // check if hit an enemy
            if (Physics.Raycast(Camera.main.ScreenPointToRay(m_crosshair.transform.position), out hit)
                && hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<Enemy>().TakeDamage(m_bulletDamage);
            }

            // set shoot timer
            m_fShootTimer = m_shootCooldown;
        }

        // get direction for grenade to be thrown using raycast
        Ray ray = Camera.main.ScreenPointToRay(m_crosshair.transform.position);
        Vector3 v3GrenadeStartPos = ray.origin + (ray.direction * 1f);
        if (m_bHoldingGrenade)
        {
            // set new grenade position
            m_heldGrenade.transform.position = v3GrenadeStartPos;
            // calculate and draw trajectory line
            UpdateTrajectory(v3GrenadeStartPos, transform.forward * m_grenadeThrowVelocity);
        }

        // throw grenade if right mouse button clicked and we have a grenade available
        if (Input.GetMouseButton(1)
            && m_nGrenadeCount > 0
            && !m_bHoldingGrenade)
        {
            // create grenade object
            m_heldGrenade = Instantiate(m_grenade, v3GrenadeStartPos, Quaternion.Euler(Vector3.zero));
            // disable shooting
            m_crosshair.SetActive(false);
            m_muzzleFlash.SetActive(false);
            m_bHoldingGrenade = true;
            // enable kinematic state to prevent gain of velocity from gravity
            m_heldGrenade.GetComponent<Rigidbody>().isKinematic = true;
            // enable trajectory renderer
            m_lineRenderer.enabled = true;
        }

        if (Input.GetMouseButtonUp(1)
            && m_nGrenadeCount > 0)
        {
            // add velocity to grenade
            m_heldGrenade.GetComponent<Rigidbody>().velocity += transform.forward * m_grenadeThrowVelocity;
            // decrement grenade count
            m_nGrenadeCount--;
            // disable grenade on UI
            m_grenadeUIs[m_nGrenadeCount].SetActive(false);
            // allow shooting again
            m_crosshair.SetActive(true);
            m_muzzleFlash.SetActive(true);
            m_bHoldingGrenade = false;
            // disable kinematic state
            m_heldGrenade.GetComponent<Rigidbody>().isKinematic = false;
            // disable trajectory renderer
            m_lineRenderer.enabled = false;
            // set grenade state to thrown
            m_heldGrenade.GetComponent<Grenade>().m_bThrown = true;
        }
    }

    /* @brief Takes damage and dies if the player has no more lives
    */
    public void TakeDamage()
    {
        // decrement lives
        m_nLives--;
        // if dead
        if (m_nLives == 0)
            return;
    }

    /*  @brief Calculates and draws the trajectory of grenade toss
        @param The initial position of the grenade
        @param The initial velocity of the grenade
    */
    private void UpdateTrajectory(Vector3 v3InitialPos, Vector3 v3InitialVelocity)
    {
        float timeDelta = 1.0f / v3InitialVelocity.magnitude;

        // copy position and velocity
        Vector3 position = v3InitialPos;
        Vector3 velocity = v3InitialVelocity;
        for (int i = 0; i < m_trajectoryPointCount; ++i)
        {
            m_lineRenderer.SetPosition(i, position);

            position += velocity * timeDelta + 0.5f * Physics.gravity * timeDelta * timeDelta;
            velocity += Physics.gravity * timeDelta;
        }
    }

    /*  @brief Adds a number of grenades to the player's current grenade count
        @param The number of grenades to add to the player's grenade count
    */
    public void AddGrenades(int nGrenades)
    {
        for (int i = 0; i < nGrenades; ++i)
        {
            // don't add grenade if at max grenade count
            if (m_nGrenadeCount == m_maxGrenadeCount)
                break;
            // enable grenade on UI
            m_grenadeUIs[m_nGrenadeCount].SetActive(true);
            // increment grenade count
            m_nGrenadeCount++;
        }
    }
}