using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDetect : MonoBehaviour
{
    public Vector2 p_NormalVec;

    private RaycastHit2D m_RayHit;
    
    void Update()
    {
        m_RayHit = Physics2D.Raycast(transform.position, -transform.up,
            1f, LayerMask.GetMask("Floor"));
        Debug.DrawRay(transform.position, -transform.up * 1f, new Color(0, 0, 1));
        p_NormalVec = m_RayHit.normal;

        p_NormalVec.x = Mathf.Round(p_NormalVec.x * 100) * 0.01f;
    }
}