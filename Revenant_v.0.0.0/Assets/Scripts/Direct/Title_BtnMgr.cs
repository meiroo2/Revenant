using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Title_BtnMgr : MonoBehaviour
{
    // Visible Member Variables
   // public int p_LoadSceneIdx;
    public GameObject p_Btn;
    
    
    // Member Variables
    
    
    // Constructors
    private void Awake()
    {
        p_Btn.SetActive(false);
    }


    // Functions
    public void ActiveBtn(bool _isActive)
    {
        p_Btn.SetActive(_isActive);
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(2);
    }
}