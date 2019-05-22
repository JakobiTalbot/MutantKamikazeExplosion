using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public GameObject[] m_points;
    public float m_moveSpeed = 1f;

    private float m_fLerpTime;
    private bool m_bMovingPoints = false;
    private int m_iCurrentPoint = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bMovingPoints)
        {
            m_fLerpTime += Time.deltaTime * m_moveSpeed;
        }
    }

    public void MoveToNextPoint()
    {
        m_iCurrentPoint++;
    }
}