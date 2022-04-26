using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBoundMgr : MonoBehaviour
{
    // 0, 1, 2, 3 -> ╩С го аб ©Л
    public CamBound[] m_Bounds;

    private Transform[] m_BoundTransforms;
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
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    if (_pos.y < m_BoundTransforms[0].position.y)
                        return true;
                    break;

                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
        return false;
    }
}