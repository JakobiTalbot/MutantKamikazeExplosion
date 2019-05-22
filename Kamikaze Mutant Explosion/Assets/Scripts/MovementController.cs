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

    private void Awake()
    {
        // set starting position and rotation
        transform.position = m_points[0].transform.position;
        transform.rotation = m_points[0].transform.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        // move to next point if moving points
        if (m_bMovingPoints)
        {
            // increase lerp time
            m_fLerpTime += Time.deltaTime * m_moveSpeed;

            // set lerp time to 0 and moving points to false if we have moved all the way
            if (m_fLerpTime >= 1f)
            {
                m_fLerpTime = 1f;
                m_bMovingPoints = false;
            }

            int iPrevPoint;
            // set index of previous point
            if (m_iCurrentPoint != 0)
            {
                iPrevPoint = m_iCurrentPoint - 1;
            }
            else
            {
                iPrevPoint = m_points.Length - 1;
            }
            // lerp position
            transform.position = Vector3.Lerp(m_points[iPrevPoint].transform.position, m_points[m_iCurrentPoint].transform.position, m_fLerpTime);
            // lerp rotation
            transform.rotation = Quaternion.Lerp(m_points[iPrevPoint].transform.rotation, m_points[m_iCurrentPoint].transform.rotation, m_fLerpTime);
        }

        // debug
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveToNextPoint();
        }
    }

    public void MoveToNextPoint()
    {
        // if at end of array, move to first point, otherwise move to next point
        if (m_iCurrentPoint == m_points.Length - 1)
            m_iCurrentPoint = 0;
        else
            m_iCurrentPoint++;
        // reset lerp time
        m_fLerpTime = 0f;
        // enable moving points
        m_bMovingPoints = true;
    }
}