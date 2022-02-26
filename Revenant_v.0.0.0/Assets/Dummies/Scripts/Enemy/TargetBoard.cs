using System.Collections;
using UnityEngine;

public enum TargetBoardState
{
    SPAWN,
    CANHIT,
    DEAD
}


public class TargetBoard : MonoBehaviour, IBulletHit
{
    float Hp = 1;
    public TargetBoardState targetBoardState { get; set; }

    HitPoints hitPoint;

    Animator animator;
    Rigidbody2D rigid;
    [SerializeField]
    PolygonCollider2D[] hitColliders;

    //HJTEST
    private SoundMgr m_SoundMgr;
    private TargetGameMgr m_TargetMgr;


    private void Awake()
    {
        //targetBoardState = TargetBoardState.SPAWN;
        hitPoint = HitPoints.OTHER;
        animator = GetComponent<Animator>();
        animSet("isAlive", true);

        //HJETST
        m_SoundMgr = GameObject.FindWithTag("SoundMgr").GetComponent<SoundMgr>();
        m_TargetMgr = GameObject.Find("TargetGameMgr").GetComponent<TargetGameMgr>();
    }

    private void Update()
    {

    }

    public void BulletHit(float _damage, Vector2 _contactPoint, HitPoints _hitPoints)
    {
        hitPoint = _hitPoints;
        if (hitPoint == HitPoints.HEAD)
        {
            animator.SetTrigger("isHead");

            //HJTEST
            m_SoundMgr.playBulletHitSound(MatType.Target_Head, _contactPoint);
            if (m_SoundMgr.m_isEnd == false)
                m_TargetMgr.getScore(true);
        }
        else if (hitPoint == HitPoints.BODY)
        {
            animator.SetTrigger("isBody");

            //HJTEST
            m_SoundMgr.playBulletHitSound(MatType.Target_Body, _contactPoint);
            if (m_SoundMgr.m_isEnd == false)
                m_TargetMgr.getScore(false);
        }

        //animSet("isAlive", false);
        hitColliders[0].enabled = false;
        hitColliders[1].enabled = false;

        Invoke(nameof(setAliveToTrue), 1.5f);
        
    }
    
    public void setAliveToTrue()
    {
        animSet("isAlive", true);
        hitColliders[0].enabled = true;
        hitColliders[1].enabled = true;
    }

    public void setDown()
    {
        animSet("isAlive", false);
    }

    public void animSet(string _anim, bool _input)
    {
        
        animator.SetBool(_anim, _input);
    }
}
