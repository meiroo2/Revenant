using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner_LayerRoomDoor : EnemySpawner
{
    // Member Variables
    [Space(20f)]
    public Animator p_DoorAnimator;

    // Constructor
    private new void Awake()
    {
        base.Awake();
    }
    private new void Start()
    {
        base.Start();
    }
    
    
    // Functions
    protected override IEnumerator SpawnCoroutine()
    {
        p_DoorAnimator.SetTrigger("Open");
        
        yield return new WaitForSeconds(p_SpawnDelay);
        SpawnAllEnemys();
        
        p_DoorAnimator.SetTrigger("Close");
    }
}