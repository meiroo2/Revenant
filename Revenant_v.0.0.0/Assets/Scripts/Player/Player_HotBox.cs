using UnityEditor;
using UnityEngine;


public class Player_HotBox : MonoBehaviour, IHotBox
{
    private Player m_Player;
    public int m_hotBoxType { get; set; } = 0;
    public bool m_isEnemys { get; set; } = false;

    private void Start()
    {
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
    }

    public void HitHotBox(IHotBoxParam _param)
    {
        m_Player.Attacked(_param);
    }
}