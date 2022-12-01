using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RigidBodyDestruction : MonoBehaviour
{
    [SerializeField] private Vector2 ForceDirection;
    [SerializeField] private float Torque;

    private Rigidbody2D Rb2D;

    private void Awake()
    {
        ForceDirection = new Vector2(Random.Range(-100,100), Random.Range(-100,100));
        Torque = Random.Range(-100.0f, 100.0f);
    }

    void Start()
    {
        Rb2D = GetComponent<Rigidbody2D>();
        Rb2D.AddForce(ForceDirection);
        Rb2D.AddTorque(Torque);
    }
}
