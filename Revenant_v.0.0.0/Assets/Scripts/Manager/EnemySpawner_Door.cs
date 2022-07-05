using System;
using System.Collections;
using UnityEngine;


public class EnemySpawner_Door : EnemySpawner
{
    // Member Variables
    [Space(20f)]
    [Header("Door Sprite List")]
    public Animator[] p_DoorAnimator;

    private static readonly int IsOpen = Animator.StringToHash("IsOpen");


    // Constructor
    private new void Awake()
    {
        base.Awake();
        p_DoorAnimator = GetComponentsInChildren<Animator>();
    }
    private new void Start()
    {
        base.Start();
    }
    
    
    // Functions
    protected override IEnumerator SpawnCoroutine()
    {
        foreach (var vAnimator in p_DoorAnimator)
        {
            vAnimator.SetInteger(IsOpen, 1);
        }
        yield return new WaitForSeconds(p_SpawnDelay);
        SpawnAllEnemys();
        foreach (var vAnimator in p_DoorAnimator)
        {
            vAnimator.SetInteger(IsOpen, 0);
        }
    }
}