using System.Collections;
using UnityEngine;

public enum TargetBoardState
{
    SPAWN,
    CANHIT,
    DEAD
}
public enum PARTS
{
    NONE, HEAD, BODY
}

public class TargetBoard_Controller : Enemy
{
    public TutoEnemyMgr m_tutoEnemyMgr { get; set; }

    public PARTS hitParts { get; set; } = PARTS.NONE;
    //float Hp = 1;
    public TargetBoardState targetBoardState { get; set; }


    [field: SerializeField]
    public TargetBoard_Animator m_targetboard_animator { get; private set; }

    [field: SerializeField]
    public BoxCollider2D[] m_hotboxes { get; set; }

    public bool m_canRespawn { get; set; } =false;
    [field: SerializeField]
    public float m_respawnTime { get; set; } = 2f;

    private void Awake()
    {
        gameObject.SetActive(false);
        m_tutoEnemyMgr = GameObject.Find("TutoEnemyMgr").GetComponent<TutoEnemyMgr>();
    }

    public override void Idle() {}


    public void Attacked()
    {
        if (!m_canRespawn)
        {
            m_tutoEnemyMgr.PlusDieCount();
        }

        else if (m_canRespawn)
            Die();
    }

    public void Die()
    {
        Invoke(nameof(Respawn), m_respawnTime);
    }

    public void Respawn()
    {
        m_targetboard_animator.RespawnAni();
    }
    
    public void HotBoxToggle(bool _input)
    {
        foreach (var h in m_hotboxes)
            h.enabled = _input;
    }
}
