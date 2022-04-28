using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBoundMgr : MonoBehaviour
{
    // 0, 1, 2, 3 -> 상 하 좌 우
    public CamBound[] m_Bounds;

    private Transform[] m_BoundTransforms;

    private bool m_canMove = true;

    private void Awake()
    {
        m_BoundTransforms = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            m_BoundTransforms[i] = m_Bounds[i].transform;
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
                        Debug.Log("카메라 상 걸림");

                        m_canMove = false;
                    }
                    break;

                case 1:
                    if (_pos.y < m_BoundTransforms[1].position.y)
                    {
                        Debug.Log("카메라 하 걸림");

                        m_canMove = false;
                    }
                    break;

                case 2:
                    if (_pos.x < m_BoundTransforms[2].position.x)
                    {
                        Debug.Log("카메라 좌 걸림");

                        m_canMove = false;
                    }
                    break;

                case 3:
                    if (_pos.x > m_BoundTransforms[3].position.x)
                    {
                        Debug.Log("카메라 우 걸림");

                        m_canMove = false;
                    }
                    break;
            }
        }
        return m_canMove;
    }

}