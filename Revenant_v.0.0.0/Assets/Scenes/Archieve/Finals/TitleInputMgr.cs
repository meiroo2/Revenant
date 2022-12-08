using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleInputMgr : MonoBehaviour
{
  public int p_JumpSceneIdx = 2;

  private Coroutine m_Coroutine = null;
  private bool m_InputOn = false;
  
  private void Update()
  {
    if (Input.anyKeyDown)
    {
      if (m_InputOn)
        return;

      m_InputOn = true;

      m_Coroutine = StartCoroutine(waitnLoad());
    }
  }


  private IEnumerator waitnLoad()
  {
    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Title_Ui/Title_Start");
    yield return new WaitForSeconds(1.8f);
    GameMgr.GetInstance().p_SoundPlayer.BGMusicStop();
    GameMgr.GetInstance().RequestLoadScene(p_JumpSceneIdx, true);
  }
  
}