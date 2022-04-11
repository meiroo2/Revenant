using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairPos : MonoBehaviour
{
    public bool m_isGoingUp;
    private Player m_Player;
    private Stair m_ParentStair;

    private void Start()
    {
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
        m_ParentStair = GetComponentInParent<Stair>();
    }

    public int StairDetectorDetected(int _objID)
    {
        // OnStair = 1, OffStair = 0, Other = 2
        int temp = m_ParentStair.isOnStair(_objID);
        if (temp == 1)
        {
            return 1;
        }
        else if (temp == 0)
        {
            return 0;
        }
        return 2;
    }
}
