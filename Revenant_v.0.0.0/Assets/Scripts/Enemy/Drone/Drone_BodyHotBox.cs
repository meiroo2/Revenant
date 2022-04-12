using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_BodyHotBox : MonoBehaviour, IHotBox
{
    public int m_hotBoxType { get; set; } = 0;
    Drone_Controller drone;


    BoxCollider2D bodyCollider;

    [SerializeField]
    BoxCollider2D targetCollider;

    

    private void Awake()
    {
        drone = GetComponentInParent<Drone_Controller>();
        bodyCollider = GetComponent<BoxCollider2D>();
    }

    public void HitHotBox(IHotBoxParam _param)
    {
        
        drone.BodyAttacked();

        bodyCollider.enabled = false;
        targetCollider.enabled = false;

    }
}
