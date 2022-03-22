using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatior : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void setBool(string param, bool _input)
    {
        anim.SetBool(param, _input);
    }
}
