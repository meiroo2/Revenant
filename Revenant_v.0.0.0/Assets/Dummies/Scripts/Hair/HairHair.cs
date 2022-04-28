using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairHair : MonoBehaviour
{
    //Quaternion purpseRot = new Quaternion(0, 0, 180, 1);
    //bool input = false;

    Vector3 leftScale = new Vector3(-1, 1, 1);
    Vector3 rightScale = new Vector3(1, 1, 1);

    Rigidbody2D rigid;
    HingeJoint2D joint;
    JointAngleLimits2D limts2D;
    
    void Start()
    {
        //transform.rotation = purpseRot;
        rigid = GetComponent<Rigidbody2D>();
        joint = GetComponentInChildren<HingeJoint2D>();
        limts2D = joint.limits;
    }
    private void FixedUpdate()
    {
        rigid.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        if (rigid.velocity.x < 0)
        {
            
            if (transform.localScale != leftScale)
                transform.localScale = leftScale;
        }
        else if (rigid.velocity.x >0)
        {
            if (transform.localScale != rightScale)
                transform.localScale = rightScale;
        }
            
        //Debug.Log(rigid.velocity);
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    input = true;
        //}

        //if(input)
        //{
        //    transform.rotation = purpseRot; Quaternion.Lerp(transform.rotation, purpseRot, 0.1f);
        //}

        //molamola
    }
}
