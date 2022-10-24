using System;
using UnityEngine;


public class Player_CheckPointMgr : MonoBehaviour
{
    // Member Variables
    private DataHandleManager m_DataMgr;


    // Constructors
    private void Start()
    {
        m_DataMgr = GameMgr.GetInstance().p_DataHandleMgr;

        if (!m_DataMgr.IsCheckPointActivated)
        {
            Debug.Log("플레이어 체크포인트 없어요" + m_DataMgr.m_OriginPlayerPos);
            SetPlayerInitPos(m_DataMgr.m_OriginPlayerPos);
        }
        else
        {
            Debug.Log("플레이어 체크포인트로 복귀!");
            SetPlayerInitPos(m_DataMgr.PlayerPositionVector);
        }
    }
    
    
    // Functions
    private void SetPlayerInitPos(Vector2 _pos)
    {
        transform.position = _pos;
    }
}