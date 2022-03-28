using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour, IUseableObj
{
    // Visible Member Variables
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.OBJECT;
    public bool m_isOn { get; set; } = false;
    public int m_LoadSceneIdx = 0;

    // Member Variables


    // Constructors


    // Updates


    // Physics


    // Functions
    public bool useObj()
    {
        SceneManager.LoadScene(m_LoadSceneIdx);
        return true;
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
