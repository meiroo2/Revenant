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
    
    bool isAlive = true;

    Animator animator;
    Rigidbody2D rigid;

    private void Awake()
    {
        targetBoardState = TargetBoardState.SPAWN;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isAlive)
        {
            //animator.SetBool("isAlive", true);
        }
        else
        {
            //animator.SetBool("isAlive", false);
        }
    }
    public void BulletHit(float _damage, HitPoints hitPoints)
    {
        hitPoint = hitPoints;

        isAlive = false;
        animBoolSet("isAlive", false);
    }

    public void setAliveToTrue()
    {

    }

    public void animBoolSet(string _anim, bool _input)
    {
        animator.SetBool(_anim, _input);
    }
}
