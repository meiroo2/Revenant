using UnityEditor;
using UnityEngine;


public class Player_HotBox : MonoBehaviour, IHotBox
{
    private Player m_Player;

    private void Start()
    {
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
    }

    public void HitHotBox(IHotBoxParam _param)
    {
        m_Player.Attacked(_param);
    }
}