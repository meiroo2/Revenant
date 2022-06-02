using UnityEngine;


public abstract class Enemy_FSM
{
    public abstract void StartState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void NextPhase();
}