using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// GameMgr에 붙어있는 Player_Manager입니다. Player를 찾아서 멤버변수로 소유합니다.
/// </summary>
public class Player_Manager : MonoBehaviour
{
    // Member Variables
    private Player m_Player = null;

    
    // Constructors
    private void Awake()
    {
        Debug.Log("PlayerMgr Awake");
        ResetPlayer();
    }
    
    
    // Functions

    /// <summary>
    /// 멤버 변수인 m_Player를 태그 기반으로 찾아서 할당합니다.
    /// </summary>
    public void ResetPlayer()
    {
        Debug.Log("PlayerManager OnSceneLoaded");
        GameObject findPlayer = GameObject.FindWithTag("@Player");

        if (ReferenceEquals(findPlayer, null))
        {
            m_Player = null;
            Debug.Log("ERR : Player_Manager에서 Player 찾지 못함. 컷신으로 취급");
            return;
        }
        else
        {
            if (findPlayer.TryGetComponent(out Player player))
            {
                Debug.Log("Player_Manager에서 Player 발견");
                m_Player = player;
            }
            else
            {
                Debug.LogError("ERR : Player_Manager에서 Player TryGetComponent 실패");
            }
        }
    }

    /// <summary>
    /// Player를 리턴합니다.
    /// </summary>
    /// <returns></returns>
    public Player GetPlayer()
    {
        if(!m_Player)
            Debug.Log("ERR : Player_Manager에서 Null Player를 리턴");
        
        return m_Player;
    }


    /*
    private void Awake()
    {
        // 체크포인트가 활성화 되지 않았을 때 (시작할 때)
        if (!DataHandleManager.Instance.IsCheckPointActivated)
        {
            m_PlayerSpawnPos = GameObject.Find("@PlayerSpawnPos");
            SetUpPlayer(InstantiatedPlayer, m_PlayerSpawnPos.transform.position);
        }
        else
        {
            SetUpPlayer(InstantiatedPlayer, DataHandleManager.Instance.PlayerPositionVector);
        }
    }

    void SetUpPlayer(GameObject PlayerPrefab, Vector2 PlayerLocation)
    {
        PlayerPrefab = Instantiate(m_PlayerPrefab);
        PlayerPrefab.transform.position = PlayerLocation;
        m_Player = PlayerPrefab.GetComponent<Player>();
    }
    */
}