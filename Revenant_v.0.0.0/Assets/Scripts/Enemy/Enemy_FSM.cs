using UnityEngine;

public abstract class Enemy_FSM
{
    public enum EnemyState
    {
        Idle,
        Alert,
        Chase,
        Attack,
        MAX
    }

    public EnemyState _enemyState;
    
    public abstract void StartState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void NextPhase();
}