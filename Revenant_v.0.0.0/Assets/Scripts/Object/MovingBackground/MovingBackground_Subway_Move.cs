using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground_Subway_Move : MovingBackground_Move
{
    // 브레이크 시간. 숫자가 높을 수록 이동속도가 느려짐. 
    [SerializeField] private float BrakingTime = 10.0f;
    
    private Vector2 vel = Vector2.zero;

    protected override void Move()
    {
        transform.position = Vector2.SmoothDamp(transform.position, EndTransform.position, ref vel, BrakingTime);
    }
}
