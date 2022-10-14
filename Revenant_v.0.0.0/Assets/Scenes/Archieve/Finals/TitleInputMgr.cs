using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class TitleInputMgr : MonoBehaviour
{
  private void Update()
  {
    if (Input.anyKeyDown)
    {
      SceneManager.LoadScene(1);
    }
  }
}