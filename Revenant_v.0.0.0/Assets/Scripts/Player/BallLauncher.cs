using System;
using System.Collections.Generic;
using UnityEngine;


public class BallLauncher : MonoBehaviour
{
    public GameObject p_Ball;
    public int p_ArrLength;
    public float p_DeadTime = 1f;
    public float p_Power = 1f;
    
    // Member Variables
    private List<Ball> m_BallList = new List<Ball>();
    private int m_Idx = 0;
    private RaycastHit2D m_RayHit2D;
    
    private void Awake()
    {
        for (int i = 0; i < p_ArrLength; i++)
        {
            m_BallList.Add(Instantiate(p_Ball, null).GetComponent<Ball>());
        }
    }

    public void LaunchBall(Vector2 _pos, Vector2 _direction)
    {
        int layermaskIdx = LayerMask.GetMask("BulletCollide");
        m_RayHit2D = Physics2D.Raycast(transform.position, _direction,
            5f, layermaskIdx);
        
        if (!m_RayHit2D.collider)
            return;
        
        
        m_BallList[m_Idx].InitBall(m_RayHit2D.point - (_direction * 0.1f), p_DeadTime);
        m_BallList[m_Idx].m_Rigid.AddForce(_direction * p_Power, ForceMode2D.Impulse);
        m_Idx++;

        if (m_Idx >= p_ArrLength)
        {
            m_Idx = 0;
        }
    }
}