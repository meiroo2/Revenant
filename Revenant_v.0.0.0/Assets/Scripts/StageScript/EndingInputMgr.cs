using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndingInputMgr : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(0);
        }
    }
}