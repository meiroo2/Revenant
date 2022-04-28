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
        m_PlayerSpawnPos = GameObject.Find("@PlayerSpawnPos");
        InstantiatedPlayer = Instantiate(m_PlayerPrefab);
        InstantiatedPlayer.transform.position = m_PlayerSpawnPos.transform.position;
        m_Player = InstantiatedPlayer.GetComponent<Player>();
    }
}