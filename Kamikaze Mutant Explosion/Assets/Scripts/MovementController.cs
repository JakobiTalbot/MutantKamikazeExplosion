using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public GameObject[] m_points;
    public float m_moveSpeed = 1f;

    private float m_fLerpTime = 0f;
    private bool m_bMovingPoints = false;
    private int m_iCurrentPoint = 0;

    // Update is called once per frame
    void Update()
    {
        if (m_bMovingPoints)
        {
            // increase lerp time
            m_fLerpTime += Time.deltaTime * m_moveSpeed;

            // set lerp time to 1 and moving points to false if we have moved all the way
            if (m_fLerpTime >= 1f)
            {
                m_fLerpTime = 1f;
                m_bMovingPoints = false;
            }

            // lerp position
            transform.position = Vector3.Lerp(m_points[m_iCurrentPoint - 1].transform.position, m_points[m_iCurrentPoint].transform.position, m_fLerpTime);
            // lerp rotation
            transform.rotation = Quaternion.Lerp(m_points[m_iCurrentPoint - 1].transform.rotation, m_points[m_iCurrentPoint].transform.rotation, m_fLerpTime);
        }
    }

    public void MoveToNextPoint()
    {
        m_iCurrentPoint++;
    }
}