using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Animator : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


}
