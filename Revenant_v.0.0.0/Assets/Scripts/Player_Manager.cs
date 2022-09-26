using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    public Player m_Player { get; private set; }
    [field: SerializeField] private GameObject m_PlayerPrefab;
    private GameObject m_PlayerSpawnPos;

    private GameObject InstantiatedPlayer;

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
}