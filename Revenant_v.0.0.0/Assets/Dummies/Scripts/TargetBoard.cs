using System.Collections;
using UnityEngine;

public enum TargetBoardState
{
    SPAWN,
    CANHIT,
    DEAD
}

public class TargetBoard : MonoBehaviour
{
    float Hp = 1;
    public TargetBoardState targetBoardState { get; set; }

    bool isAlive = true;
    Rigidbody2D rigid;

    private void Awake()
    {
        targetBoardState = TargetBoardState.SPAWN;
    }

    private void Update()
    {
        
    }

}
