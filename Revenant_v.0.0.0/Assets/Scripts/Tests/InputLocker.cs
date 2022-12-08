using System;
using UnityEngine;


public class InputLocker : MonoBehaviour
{
    public void Start()
    {
        GameMgr.GetInstance().p_PlayerInputMgr.p_MoveInputLock = true;
        GameMgr.GetInstance().p_PlayerInputMgr.p_RollLock = true;
        GameMgr.GetInstance().p_PlayerInputMgr.p_SideAttackLock = true;
    }
}