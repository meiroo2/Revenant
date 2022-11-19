using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class TitleInputMgr : MonoBehaviour
{
  public int p_JumpSceneIdx = 2;
  private void Update()
  {
    if (Input.anyKeyDown)
    {
      GameMgr.GetInstance().RequestLoadScene(p_JumpSceneIdx, true);
    }
  }
}