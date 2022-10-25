using System;
using UnityEngine;


public class ForceCheckPointReset : MonoBehaviour
{
  private void Awake()
  {
    GameMgr.GetInstance().p_DataHandleMgr.ResetCheckPoint();
  }
}