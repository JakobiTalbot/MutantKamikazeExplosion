using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // array of points to move between
    public GameObject[] m_points;
    // speed to move between points
    public float m_moveSpeed = 1f;
    public float m_rotateSpeed = 1f;

    // float for storing current normalised distance between 2 points
    private float m_fPosLerpTime = 0f;
    private float m_fRotLerpTime = 0f;
    // bool to store whether we are moving or not
    [HideInInspector]
    public bool m_bMovingPoints = false;
    // int to to store the index of the current point
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
            m_fPosLerpTime += Time.deltaTime * m_moveSpeed;
            m_fRotLerpTime += Time.deltaTime * m_rotateSpeed;

            // set lerp time to 0 and moving points to false if we have moved all the way
            if (m_fPosLerpTime >= 1f)
            {
                m_fPosLerpTime = 1f;
            }
            if (m_fRotLerpTime >= 1f)
            {
                m_fRotLerpTime = 1f;
            }
            if (m_fPosLerpTime >= 1f
                && m_fRotLerpTime >= 1f)
            {
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
            transform.position = Vector3.Lerp(m_points[iPrevPoint].transform.position, m_points[m_iCurrentPoint].transform.position, m_fPosLerpTime);
            // lerp rotation
            transform.rotation = Quaternion.Euler(Vector3.Lerp(m_points[iPrevPoint].transform.rotation.eulerAngles, m_points[m_iCurrentPoint].transform.rotation.eulerAngles, m_fRotLerpTime)
                + GetComponent<PlayerController>().m_v3AddedRotation);
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
        m_fPosLerpTime = 0f;
        m_fRotLerpTime = 0f;
        // enable moving points
        m_bMovingPoints = true;
    }

    // return current point
    public GameObject GetCurrentPoint() => m_points[m_iCurrentPoint];
}