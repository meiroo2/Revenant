using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_BodyHotBox : MonoBehaviour, IHotBox
{
    Drone drone;

    private void Awake()
    {
        drone = GetComponentInParent<Drone>();
    }

    public void HitHotBox(IHotBoxParam _param)
    {
        drone.Attacked();
    }
}
