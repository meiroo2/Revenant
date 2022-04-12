using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_TargetHotBox : MonoBehaviour, IHotBox
{
    public int m_hotBoxType { get; set; } = 0;
    Drone_Controller drone;
    [SerializeField]
    GameObject targetParts;

    BoxCollider2D targetCollider;

    private void Awake()
    {
        drone = GetComponentInParent<Drone_Controller>();
        targetCollider = GetComponent<BoxCollider2D>();
    }

    public void HitHotBox(IHotBoxParam _param)
    {
        drone.TargetAttacked();
        targetParts.SetActive(true);

        targetCollider.enabled = false;
    }
}
