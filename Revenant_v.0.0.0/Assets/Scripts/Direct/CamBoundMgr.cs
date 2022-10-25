using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBoundMgr : MonoBehaviour
{
    // 0, 1, 2, 3 -> ?? ?? ?? ??
    public CamBound[] m_Bounds;

    private Transform[] m_BoundTransforms;

    private CameraMgr m_CamMgr;

    private bool m_canMove = true;
    private Vector3 m_NearCamPos;

    private void Awake()
    {
        m_BoundTransforms = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            m_BoundTransforms[i] = m_Bounds[i].transform;
        }

        m_CamMgr = Camera.main.GetComponent<CameraMgr>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("@Player"))
        {
            m_CamMgr.ChangeCamBoundMgr(this);
        }
    }

    public bool canCamMove(Vector2 _pos)
    {
        m_canMove = true;
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    if (_pos.y > m_BoundTransforms[0].position.y)
                    {
                        m_canMove = false;
                    }
                    break;

                case 1:
                    if (_pos.y < m_BoundTransforms[1].position.y)
                    {
                        m_canMove = false;
                    }
                    break;

                case 2:
                    if (_pos.x < m_BoundTransforms[2].position.x)
                    {
                        m_canMove = false;
                    }
                    break;

                case 3:
                    if (_pos.x > m_BoundTransforms[3].position.x)
                    {
                        m_canMove = false;
                    }
                    break;
            }
        }
        return m_canMove;
    }
    public Vector3 getNearCamPos(Vector3 _pos)
    {
        m_NearCamPos = _pos;

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    if (m_NearCamPos.y > m_BoundTransforms[0].position.y)
                    {
                        m_NearCamPos.y = m_BoundTransforms[0].position.y - 0.02f;
                    }
                    break;

                case 1:
                    if (m_NearCamPos.y < m_BoundTransforms[1].position.y)
                    {
                        m_NearCamPos.y = m_BoundTransforms[1].position.y + 0.02f;
                    }
                    break;

                case 2:
                    if (m_NearCamPos.x < m_BoundTransforms[2].position.x)
                    {
                        m_NearCamPos.x = m_BoundTransforms[2].position.x + 0.02f;
                    }
                    break;

                case 3:
                    if (m_NearCamPos.x > m_BoundTransforms[3].position.x)
                    {
                        m_NearCamPos.x = m_BoundTransforms[3].position.x - 0.02f;
                    }
                    break;
            }
        }
        return m_NearCamPos;
    }
}