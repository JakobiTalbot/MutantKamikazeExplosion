using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // texture for the crosshair on screen
    public Texture2D m_crosshairTexture;
    // mouse sensitivity multiplier
    public float m_mouseSensitivity = 1f;
    // texture for muzzle flash when firing
    public GameObject m_muzzleFlash;
    // time for the muzzle flash to be on screen
    public float m_muzzleFlashTime = 0.1f;
    // cooldown between shooting
    public float m_shootCooldown = 0.1f;

    // how many lives the player has
    private int m_nLives = 3;
    // timer to count until the muzzle flash disappears
    private float m_fMuzzleFlashTimer = 0f;
    // timer to count until the player can shoot again
    private float m_fShootTimer = 0f;


    void Awake()
    {
        // set cursor to crosshair
        Cursor.SetCursor(m_crosshairTexture, new Vector2(m_crosshairTexture.width * 0.5f, m_crosshairTexture.height * 0.5f), CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        // decrement timers
        m_fShootTimer -= Time.deltaTime;
        m_fMuzzleFlashTimer -= Time.deltaTime;

        // disable muzzle flash if still active and timer has expired
        if (m_muzzleFlash.activeSelf
            && m_fMuzzleFlashTimer <= 0f)
            m_muzzleFlash.SetActive(false);
        else if (m_muzzleFlash.activeSelf) // set position to follow cursor if muzzle flash still enabled
            m_muzzleFlash.transform.position = Input.mousePosition;

        // if the player clicks and can shoot
        if (Input.GetMouseButtonDown(0)
            && m_fShootTimer <= 0f)
        {
            // display muzzle flash, rotate muzzle flash randomly, set timer
            m_muzzleFlash.transform.position = Input.mousePosition;
            m_muzzleFlash.SetActive(true);
            m_muzzleFlash.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
            m_fMuzzleFlashTimer = m_muzzleFlashTime;

            // create raycast hit data
            RaycastHit hit = new RaycastHit();

            // check if hit an enemy
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)
                && hit.collider.CompareTag("Enemy"))
            {
                // hit enemy
            }

            // set shoot timer
            m_fShootTimer = m_shootCooldown;
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
}