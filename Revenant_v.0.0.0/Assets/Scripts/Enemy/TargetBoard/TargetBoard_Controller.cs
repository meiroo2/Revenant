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

    public PARTS hitParts { get; set; } = PARTS.NONE;
    //float Hp = 1;
    public TargetBoardState targetBoardState { get; set; }


    [field: SerializeField]
    public TargetBoard_Animator m_targetboard_animator { get; private set; }

    Rigidbody2D rigid;
    [SerializeField]
    PolygonCollider2D[] hitColliders;

    [field: SerializeField]
    public BoxCollider2D[] m_hotboxes { get; }

    public override void Idle()
    {

    }

    public void Respawn()
    {
        foreach (var h in m_hotboxes)
            h.enabled = false;
    }
}
