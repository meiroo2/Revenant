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

public class TargetBoard : Enemy
{
    public PARTS hitParts { get; set; } = PARTS.NONE;
    //float Hp = 1;
    public TargetBoardState targetBoardState { get; set; }

    Animator animator;
    Rigidbody2D rigid;
    [SerializeField]
    PolygonCollider2D[] hitColliders;

    public override void Idle()
    {

    }


    public void Attacked()
    {
        if(hitParts == PARTS.HEAD)
        {
            Debug.Log("head");
        }
        else if(hitParts == PARTS.BODY)
        {
            Debug.Log("body");
        }
        hitParts = PARTS.NONE;
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
